using AH.Save;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundsSO _soundsSO;
    [SerializeField] private PoolManagerSO _poolManager;
    [SerializeField] private PoolTypeSO _soundObjectTypeSO;
    [Space]
    [SerializeField] private IntSaveDataSO _bgmData;
    [SerializeField] private IntSaveDataSO _sfxData;
    private float bgmVolume =>_bgmData.data * 0.01f;
    private float sfxVolume => _sfxData.data * 0.01f;

    private void OnEnable() {
        GameEvents.BgmChangeEvnet += ChangedBGMVolume;
        SaveDataEvents.SaveGameEvent += AllBGMStop;
    }

    private void OnDisable() {
        GameEvents.BgmChangeEvnet -= ChangedBGMVolume;
        SaveDataEvents.SaveGameEvent -= AllBGMStop;
    }

    private Dictionary<SoundType, SoundObject> _loopingSounds = new();

    public AudioSource PlayBGM(SoundType sound, float pitch = 1)
    {
        SoundList soundList = _soundsSO.sounds[(int)sound];
        AudioClip[] clips = soundList.sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];

        SoundObject soundObj = _poolManager.Pop(_soundObjectTypeSO) as SoundObject;
        AudioSource audioSource = soundObj.AudioSource;

        if (audioSource)
        {
            audioSource.Stop();

            audioSource.outputAudioMixerGroup = soundList.mixer;
            audioSource.clip = randomClip;

            audioSource.pitch = pitch;
            audioSource.loop = true;

            StopBGM(sound);
            _loopingSounds[sound] = soundObj;
            audioSource.Play();

            // Volume
            audioSource.volume = 0;
            VolumeChange(audioSource, soundList.volume * bgmVolume);
        }

        return audioSource;
    }

    public AudioSource PlaySFX(SoundType sound, float pitch = 1, bool is3DSound = false)
    {
        SoundList soundList = _soundsSO.sounds[(int)sound];
        AudioClip[] clips = soundList.sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];

        SoundObject soundObj = _poolManager.Pop(_soundObjectTypeSO) as SoundObject;
        AudioSource audioSource = soundObj.AudioSource;

        if (audioSource)
        {
            audioSource.Stop();

            audioSource.outputAudioMixerGroup = soundList.mixer;
            audioSource.clip = randomClip;

            // Pitch
            audioSource.pitch = pitch;

            // 3D
            audioSource.spatialBlend = is3DSound? 1f : 0f;

            audioSource.loop = false;
            audioSource.Play();

            // Volume
            audioSource.volume = 0;
            VolumeChange(audioSource, soundList.volume * sfxVolume);

            soundObj.PushObject(false);
        }

        return audioSource;
    }

    public AudioSource PlaySound(string soundName, bool loop = false, float pitch = 1)
    {
        if (Enum.TryParse(soundName, true, out SoundType soundType))
        {
            if (loop)
                return PlayBGM(soundType, pitch);
            else
                return PlaySFX(soundType, pitch);
        }

        return null;
    }

    public void StopBGM(SoundType sound)
    {
        if (_loopingSounds.TryGetValue(sound, out SoundObject soundObj))
        {
            soundObj.AudioSource.Stop();
            soundObj.PushObject(true);

            _loopingSounds.Remove(sound);
        }
    }

    public void AllBGMStop(string sceneName)
    {
        foreach (var sound in _loopingSounds)
        {
            sound.Value.AudioSource.Stop();
            sound.Value.PushObject(true, 0f);
        }

        _loopingSounds.Clear();
    }

    private void VolumeChange(AudioSource audioSource, float targetValue, Action callback = null)
    {
        StartCoroutine(VolumeChangeRoutine(audioSource, targetValue, callback));
    }

    private IEnumerator VolumeChangeRoutine(AudioSource audioSource, float targetValue, Action callback)
    {
        float currentTime = 0;
        float duration = 0.2f;
        while (currentTime <= duration)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / duration;

            if (audioSource != null)
                audioSource.volume = targetValue * t;

            yield return null;
        }
        if (audioSource != null)
            audioSource.volume = targetValue;
        callback?.Invoke();
    }

    public void ChangedBGMVolume()
    {
        foreach (KeyValuePair<SoundType, SoundObject> sound in _loopingSounds)
        {
            sound.Value.AudioSource.volume = bgmVolume;
        }
    }
}

[Serializable]
public struct SoundList
{
    [HideInInspector] public string name;
    [Range(0, 1)] public float volume;
    public AudioMixerGroup mixer;
    public AudioClip[] sounds;
}