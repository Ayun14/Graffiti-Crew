using UnityEngine;

public class ComputerSceneBGM : MonoBehaviour
{
    private AudioSource _bgmAudioSource;

    private void Start()
    {
        _bgmAudioSource = GameManager.Instance.SoundSystemCompo.PlaySound(SoundType.Request, true);
    }

    private void OnDisable()
    {
        _bgmAudioSource?.GetComponent<SoundObject>().PushObject();
    }
}
