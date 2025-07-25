using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : Observer<GameStateController>
{
    [Header("Values")]
    [SerializeField] private float _shakeForce = 1f;

    // Fight
    private CinemachineCamera _graffitiCamera;
    private CinemachineImpulseSource _impulseSource;

    // Activity
    private CinemachineCamera _sprayNoneCamera;

    private void Awake()
    {
        Attach();

        mySubject.OnBlindEvent += BlindEventHandle;

        _graffitiCamera = transform.Find("Camera_PlayerGraffiti").GetComponent<CinemachineCamera>();
        _impulseSource = _graffitiCamera.GetComponent<CinemachineImpulseSource>();

        _sprayNoneCamera = transform.Find("Camera_SprayNone")?.GetComponent<CinemachineCamera>();
    }

    private void OnDestroy()
    {
        mySubject.OnBlindEvent -= BlindEventHandle;

        Detach();
    }

    public override void NotifyHandle()
    {
        if (mySubject != null)
        {
            // Fight
            bool isGraffitiCameraOff = mySubject.GameState == GameState.Result || mySubject.GameState == GameState.NextStage;
            _graffitiCamera.Priority.Value = isGraffitiCameraOff ? 0 : 2;

            if (isGraffitiCameraOff && mySubject.IsPlayerWin == false)
            {
                if (_sprayNoneCamera != null)
                    _sprayNoneCamera.Priority.Value = 5;
            }
        }
    }

    private void BlindEventHandle()
    {
        // Camera Shake
        _impulseSource.GenerateImpulseWithForce(_shakeForce);

        // Rotate
        Vector3 startRotation = transform.rotation.eulerAngles;

        float randX = Random.Range(-2, 2f);
        float randY = Random.Range(-0.2f, 0.2f);
        float randZ = Random.Range(-2f, 2f);
        transform.DORotate(new Vector3(startRotation.x + randX, startRotation.y + randY, startRotation.z + randZ), 0.25f)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                transform.DORotate(startRotation, 0.25f) // 원래 각도로 돌아감
                    .SetEase(Ease.InOutSine);
            });
    }
}
