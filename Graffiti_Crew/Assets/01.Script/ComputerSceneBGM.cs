using UnityEngine;

public class ComputerSceneBGM : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.SoundSystemCompo.PlaySound(SoundType.Request, true);
    }

    private void OnDisable()
    {
        GameManager.Instance.SoundSystemCompo.StopLoopSound(SoundType.Request);
    }
}
