using System;
using UnityEngine;

public class LockedMirrorSurface : MonoBehaviour
{
    [SerializeField] private Transform beamEmitter;
    
    public Transform GetTransform()
    {
        return beamEmitter;
    }
}
