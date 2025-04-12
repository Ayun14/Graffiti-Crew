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

            // Volume
            audioSource.volume = 0;
            audioSource.DOFade(volume * soundList.volume, 0.2f)
                .OnComplete(() => audioSource.volume = volume * soundList.volume);

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
            return PlaySound(soundType, loop, pitch, volume);

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