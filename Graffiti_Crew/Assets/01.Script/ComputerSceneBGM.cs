using UnityEngine;

public class ComputerSceneBGM : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.SoundSystemCompo.PlayBGM(SoundType.Request);
    }

    private void OnDisable()
    {
        GameManager.Instance.SoundSystemCompo.StopBGM(SoundType.Request);
    }
}
