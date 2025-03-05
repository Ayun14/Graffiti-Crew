using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : Observer<GameStateController>
{
    [Header("Values")]
    [SerializeField] private float _shakeForce = 1f;

    // Fight
    private CinemachineCamera _graffitiCamera;
    private CinemachineCamera _rivalGraffitiCamera;
    private CinemachineImpulseSource _impulseSource;

    // SprayBox
    private CinemachineCamera _sprayBoxCamera;

    private void Awake()
    {
        Attach();

        mySubject.OnBlindEvent += BlindEventHandle;
        mySubject.OnSprayEmptyEvent += SprayEmptyEventHandle;
        mySubject.OnSprayChangeEvent += SprayChangeEventHandle;

        _graffitiCamera = transform.Find("Camera_PlayerGraffiti").GetComponent<CinemachineCamera>();
        _rivalGraffitiCamera = transform.Find("Camera_RivalGraffiti").GetComponent<CinemachineCamera>();
        _impulseSource = _graffitiCamera.GetComponent<CinemachineImpulseSource>();

        _sprayBoxCamera = transform.Find("Camera_SprayBox").GetComponent<CinemachineCamera>();
    }

    private void OnDestroy()
    {
        mySubject.OnBlindEvent -= BlindEventHandle;
        mySubject.OnSprayEmptyEvent -= SprayEmptyEventHandle;
        mySubject.OnSprayChangeEvent -= SprayChangeEventHandle;

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
            if (mySubject.GameState == GameState.Result)
            {
                _graffitiCamera.Priority.Value = mySubject.IsPlayerWin ? 1 : 0;
                _rivalGraffitiCamera.Priority.Value = mySubject.IsPlayerWin ? 0 : 1;
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

    private void SprayEmptyEventHandle()
    {
        // 카메라가 상자를 쳐다보게

        _sprayBoxCamera.Priority.Value = 2;
    }

    private void SprayChangeEventHandle()
    {
        _sprayBoxCamera.Priority.Value = 0;
    }
}
