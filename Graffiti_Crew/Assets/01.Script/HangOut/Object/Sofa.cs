using System;
using Unity.Cinemachine;
using UnityEngine;

public class Sofa : InteractionObject
{
    [SerializeField] private CinemachineCamera _cam;
    private float _zoomSpeed = 10f;

    private void Update()
    {
        //if (playerState == PlayerStateEnum.Sit)
            Zoom();
        //else
            _cam.Lens.FieldOfView = 12;
    }

    private void Zoom()
    {
        float distance = Input.GetAxis("Mouse ScrollWheel") * -1 * _zoomSpeed;
        float camFOV = _cam.Lens.FieldOfView + distance;
        if (distance != 0 && camFOV >= 4 && camFOV <= 12)
        {
            _cam.Lens.FieldOfView += distance;
        }
    }
}
