using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField] private SoundsSO _soundsSO;
    [SerializeField] private PoolManagerSO _poolManager;
    [SerializeField] private PoolTypeSO _soundObjectTypeSO;

    public GameObject PlaySound(SoundType sound, bool loop = false, float volume = 1)
    {
        SoundList soundList = _soundsSO.sounds[(int)sound];
        AudioClip[] clips = soundList.sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];

        IPoolable poolable = _poolManager.Pop(_soundObjectTypeSO);
        AudioSource source = poolable.GameObject.GetComponent<AudioSource>();
        if (source)
        {
            source.outputAudioMixerGroup = soundList.mixer;
            source.clip = randomClip;
            source.volume = volume * soundList.volume;
            source.loop = loop;
            source.Play();
        }

        if (!loop)
            StartCoroutine(ReturnToPool(poolable, randomClip.length));

        return poolable.GameObject;
    }

    private IEnumerator ReturnToPool(IPoolable poolable, float delay)
    {
        yield return new WaitForSeconds(delay);
        poolable.GameObject.GetComponent<SoundObject>().PushObject();
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