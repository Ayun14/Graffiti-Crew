using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundsSO _soundsSO;
    [SerializeField] private PoolManagerSO _poolManager;
    [SerializeField] private PoolTypeSO _soundObjectTypeSO;

    public AudioSource PlaySound(SoundType sound, bool loop = false, float pitch = 1, float volume = 1)
    {
        SoundList soundList = _soundsSO.sounds[(int)sound];
        AudioClip[] clips = soundList.sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];

        AudioSource source = _poolManager.Pop(_soundObjectTypeSO).GameObject.GetComponent<AudioSource>();
        if (source)
        {
            source.outputAudioMixerGroup = soundList.mixer;
            source.clip = randomClip;

            // Pitch
            source.pitch = pitch;

            // Volume
            source.volume = 0;

            source.loop = loop;
            source.Play();

            source.DOFade(volume * soundList.volume, 0.3f);

            if (!loop) StartCoroutine(ReturnToPool(source, randomClip.length));
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

    private IEnumerator ReturnToPool(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);
        source.DOFade(0, 0.3f)
            .OnComplete(() => source.gameObject.GetComponent<SoundObject>().PushObject());
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