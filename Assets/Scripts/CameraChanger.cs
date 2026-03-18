using UnityEngine;
using Unity.Cinemachine;

public class CameraChanger : MonoBehaviour
{
    [Header("Camera References")]
    [SerializeField] private Camera playerCam;
    [SerializeField] private Camera otherCam;


    public void SwapToPlayerCamera()
    {
        if (playerCam != null && otherCam != null) 
        {
        playerCam.enabled = true;
        otherCam.enabled = false;
        }
    }
    public void SwapToOtherCamera()
    {
        if (playerCam != null && otherCam != null) 
        {
        otherCam.enabled = true;
        playerCam.enabled = false;
        }
    }

}