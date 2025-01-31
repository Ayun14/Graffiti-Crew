using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : Observer<GameStateController>
{
    [Header("Values")]
    [SerializeField] private float _shakeForce = 1f;

    private CinemachineCamera _graffitiCamera;
    private CinemachineImpulseSource _impulseSource;

    private CinemachineCamera _resultCamera;

    private void Awake()
    {
        Attach();

        mySubject.OnBlindEvent += BlindEventHandle;

        _graffitiCamera = transform.Find("Camera_Graffiti").GetComponent<CinemachineCamera>();
        _impulseSource = _graffitiCamera.GetComponent<CinemachineImpulseSource>();

        _resultCamera = transform.Find("Camera_Result").GetComponent<CinemachineCamera>();
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
            bool isgraffitiCameraOn = mySubject.GameState == GameState.Fight || mySubject.GameState == GameState.CountDown;
            _graffitiCamera.Priority.Value = isgraffitiCameraOn ? 1 : 0;

            // Result
            _resultCamera.Priority.Value = mySubject.GameState == GameState.Result ? 1 : 0;
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
                transform.DORotate(startRotation, 0.25f) // ���� ������ ���ư�
                    .SetEase(Ease.InOutSine);
            });
    }
}
