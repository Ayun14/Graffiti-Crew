using UnityEngine;

public class CanvasLookCamera : MonoBehaviour
{
    private Camera _mainCam;

    private void Awake()
    {
        _mainCam = Camera.main;
    }

    private void Update()
    {
        transform.rotation = _mainCam.transform.rotation;
    }
}