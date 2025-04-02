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

    private Dictionary<SoundType, AudioSource> _loopingSounds = new();

    public AudioSource PlaySound(SoundType sound, bool loop = false, float pitch = 1, float volume = 1)
    {
        SoundList soundList = _soundsSO.sounds[(int)sound];
        AudioClip[] clips = soundList.sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];

        AudioSource source = _poolManager.Pop(_soundObjectTypeSO).GameObject.GetComponent<AudioSource>();

        if (source)
        {
            source.Stop();

            source.outputAudioMixerGroup = soundList.mixer;
            source.clip = randomClip;

            // Pitch
            source.pitch = pitch;

            source.loop = loop;
            source.Play();

            // Volume
            source.volume = 0;
            source.DOFade(volume * soundList.volume, 0.2f)
                .OnComplete(() => source.volume = volume * soundList.volume);

            if (loop)
            {
                StopLoopSound(sound);
                _loopingSounds[sound] = source;
            }
            else
                source.GetComponent<SoundObject>().PushObject(false);
        }

        return source;
    }

    public AudioSource PlaySound(string soundName, bool loop = false, float pitch = 1, float volume = 1)
    {
        if (Enum.TryParse(soundName, true, out SoundType soundType))
        {
            return PlaySound(soundType, loop, pitch, volume);
        }
        return null;
    }

    public void StopLoopSound(SoundType sound)
    {
        if (_loopingSounds.TryGetValue(sound, out AudioSource source))
        {
            source.Stop();
            source.GetComponent<SoundObject>().PushObject(true);

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