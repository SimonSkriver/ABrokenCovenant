using UnityEngine;
using System.Collections.Generic;
using System;

public class MultiBeamScript : MonoBehaviour
{
    [Header("Beam Setup")]
    [SerializeField] private Transform rayBase;
    [SerializeField] private LineRenderer lineRenderer;

    [Header("Other References")]
    [SerializeField] private TubeRenderer tubeRenderer;

    [Header("Beam Settings")]
    [SerializeField] private float maxDistance = 25f;
    [SerializeField] private int maxBounces = 10;
    [SerializeField] private LayerMask layersToHit;
    [SerializeField] private float surfaceOffset = 0.02f;

    [Header("Fog Stuff")]
    [SerializeField] private HashSet<FogClearSurface> pastFogClearererer = new HashSet<FogClearSurface>();
    [SerializeField] private HashSet<CrossSurface> pastCrossClearererer = new HashSet<CrossSurface>();


    void Start()
    {
        if (rayBase == null)
            rayBase = transform;

        if (lineRenderer == null)
            lineRenderer = GetComponentInChildren<LineRenderer>();

        DrawBeam();
    }

    void Update()
    {
        DrawBeam();
    }

    private void DrawBeam()
    {
        HashSet<FogClearSurface> currentFogClearererer = new HashSet<FogClearSurface>();
        HashSet<CrossSurface> currentCrossClearererer = new HashSet<CrossSurface>();

        List<Vector3> points = new List<Vector3>();

        Vector3 currentOrigin = rayBase.position;
        Vector3 currentDirection = rayBase.forward.normalized;

        points.Add(currentOrigin);

        for (int bounce = 0; bounce < maxBounces; bounce++)
        {
            Ray ray = new Ray(currentOrigin, currentDirection);

            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layersToHit, QueryTriggerInteraction.Collide))
            {
                points.Add(hit.point);

                Debug.DrawRay(currentOrigin, currentDirection * hit.distance, Color.red);

                MirrorSurface mirror = hit.collider.GetComponentInParent<MirrorSurface>();
                LockedMirrorSurface lockedMirror = hit.collider.GetComponentInParent<LockedMirrorSurface>();
                FogClearSurface fogClearSurface = hit.collider.GetComponentInParent<FogClearSurface>();
                CrossSurface crossSurface = hit.collider.GetComponentInParent<CrossSurface>();

                if (crossSurface != null)
                {
                    Vector3 endPoint = hit.point;
                    points.Add(endPoint);
                    crossSurface.CrossAction();
                    currentCrossClearererer.Add(crossSurface);
                    continue;
                }

                if (fogClearSurface != null)
                {
                    fogClearSurface.DisableParticles();
                    currentFogClearererer.Add(fogClearSurface);
                    //continue;
                }

                if (mirror != null)
                {
                    currentDirection = Vector3.Reflect(currentDirection, hit.normal).normalized;
                    currentOrigin = hit.point + currentDirection * surfaceOffset;
                    continue;
                }
                
                if (lockedMirror != null)
                {
                    Transform beamEmitter = lockedMirror.GetTransform();
                    currentOrigin = beamEmitter.position; //Might get changed to hitpoint if it looks too weird
                    currentDirection = beamEmitter.forward.normalized;
                    continue;
                }

                

            }
            else
            {
                Vector3 endPoint = currentOrigin + currentDirection * maxDistance;
                points.Add(endPoint);

                Debug.DrawRay(currentOrigin, currentDirection * maxDistance, Color.red);
                break;
            }
        }

        //Fog enabler if sunbeam has left mirror
        foreach (FogClearSurface fogScript in pastFogClearererer)
        {
            if (!currentFogClearererer.Contains(fogScript))
            {
                fogScript.EnableParticles();
            }
        }
        pastFogClearererer = currentFogClearererer;

        foreach (CrossSurface crossScript in pastCrossClearererer)
        {
            if (!currentCrossClearererer.Contains(crossScript))
            {
                crossScript.CrossAction();
            }
        }
        pastCrossClearererer = currentCrossClearererer;


        if (tubeRenderer != null)
        {
          tubeRenderer.SetPositions(points.ToArray()); 
        }

        //lineRenderer.positionCount = points.Count;
        //lineRenderer.SetPositions(points.ToArray());
    }


}
