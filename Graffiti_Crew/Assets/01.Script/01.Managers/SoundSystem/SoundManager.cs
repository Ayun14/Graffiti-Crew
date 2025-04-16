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

    private float bgmVolume => _bgmData.data * 0.01f;
    private float sfxVolume => _sfxData.data * 0.01f;

    private Dictionary<SoundType, SoundObject> _loopingSounds = new();

    public AudioSource PlaySound(SoundType sound, bool loop = false, float pitch = 1, float volume = 1)
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

            audioSource.loop = loop;
            audioSource.Play();
            Debug.Log(sfxVolume);
            // Volume
            audioSource.volume = 0;
            audioSource.DOFade(volume * soundList.volume * sfxVolume, 0.2f)
                .OnComplete(() => audioSource.volume = volume * soundList.volume * sfxVolume);

            if (loop)
            {
                StopLoopSound(sound);
                _loopingSounds[sound] = soundObj;
            }
            else soundObj.PushObject(false);
        }

        return audioSource;
    }

    public AudioSource PlaySound(string soundName, bool loop = false, float pitch = 1, float volume = 1)
    {
        if (Enum.TryParse(soundName, true, out SoundType soundType))
            return PlaySound(soundType, loop, pitch, volume * sfxVolume);

        return null;
    }

    public void StopLoopSound(SoundType sound)
    {
        if (_loopingSounds.TryGetValue(sound, out SoundObject soundObj))
        {
            soundObj.AudioSource.Stop();
            soundObj.PushObject(true);

            _loopingSounds.Remove(sound);
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