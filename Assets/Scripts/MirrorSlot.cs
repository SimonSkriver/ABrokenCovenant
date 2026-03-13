using UnityEngine;

public class MirrorSlot : MonoBehaviour
{
    [SerializeField] Transform mirrorSocket;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mirror"))
        {
            SlotMirror(other.transform);
        }
    }

    void SlotMirror(Transform mirror)
    {
        mirror.SetParent(mirrorSocket);
        mirror.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        PlayerMovement.Instance.currentState = PlayerState.Normal;
    }
}
