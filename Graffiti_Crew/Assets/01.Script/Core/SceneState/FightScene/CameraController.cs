using Unity.Cinemachine;
using UnityEngine;

public class CameraController : Observer<GameStateController>
{
    [SerializeField] private CinemachineCamera _graffitiCamera;
    [SerializeField] private CinemachineCamera _resultCamera;

    private void Awake()
    {
        Attach();
    }

    private void OnDestroy()
    {
        Detach();
    }

    public override void NotifyHandle()
    {
        if (mySubject != null)
        {
            // Fight
            bool isgraffitiCameraOn = mySubject.GameState == GameState.Fight || mySubject.GameState == GameState.CountDown;
            _graffitiCamera.Priority.Value = isgraffitiCameraOn ? 1 : 0;

            // Result
            _resultCamera.Priority.Value = mySubject.GameState == GameState.Result ? 1 : 0;
        }
    }
}
