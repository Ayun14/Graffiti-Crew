using Unity.Cinemachine;
using UnityEngine;

public class DialogueCameraController : MonoBehaviour
{
    private CinemachineImpulseSource _impulseSource;

    private void Start()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void PlayCamEffect()
    {
        _impulseSource.GenerateImpulse();
    }
}
