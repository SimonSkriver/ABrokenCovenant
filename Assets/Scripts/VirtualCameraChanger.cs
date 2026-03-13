using UnityEngine;
using Unity.Cinemachine;

public class VirtualCameraChanger : MonoBehaviour
{
    [Header("Camera References")]
    [SerializeField] private CinemachineCamera playerCam;
    [SerializeField] private CinemachineCamera virtualCam;

    private void SwapToPlayerCamera()
    {
        if (playerCam != null && virtualCam != null) 
        {
        virtualCam.Priority = 1;
        playerCam.Priority = 10;
        }
    }
    private void SwapToVirtualCamera()
    {
        if (playerCam != null && virtualCam != null) 
        {
        playerCam.Priority = 1;
        virtualCam.Priority = 10;
        }
    }

}
