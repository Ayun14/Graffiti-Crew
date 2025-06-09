using AH.SaveSystem;
using DG.Tweening;
using System;
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

            // Pitch
            audioSource.pitch = pitch;

            audioSource.loop = true;
            audioSource.Play();

            // Volume
            audioSource.volume = 0;
            audioSource.DOFade(soundList.volume, 0.2f)
                .OnComplete(() => audioSource.volume = soundList.volume);

            StopBGM(sound);
            _loopingSounds[sound] = soundObj;
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
            audioSource.DOFade(soundList.volume * sfxVolume, 0.2f)
                .OnComplete(() => audioSource.volume = soundList.volume * sfxVolume);

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

    private void AllBGMStop(string sceneName)
    {
        foreach (var sound in _loopingSounds)
        {
            sound.Value.AudioSource.Stop();
            sound.Value.PushObject(true);
        }

        _loopingSounds.Clear();
    }

    public void ChangedBGMVolume()
    {
        foreach (KeyValuePair<SoundType, SoundObject> sound in _loopingSounds)
            sound.Value.AudioSource.volume = bgmVolume;
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