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

    [Header("Sets")]
    private HashSet<FogClearSurface> pastFogClearererer = new HashSet<FogClearSurface>(); // FOG
    private HashSet<CrossSurface> pastCrossClearererer = new HashSet<CrossSurface>(); // CROSS
    private HashSet<MirrorSurface> pastMirrorHits = new HashSet<MirrorSurface>(); // NORMAL MIRRORS
    private HashSet<LockedMirrorSurface> pastLockedMirrorHits = new HashSet<LockedMirrorSurface>(); // LOCKED MIRRORS

    private HashSet<FogClearSurface> currentFogClearererer;
    private HashSet<CrossSurface> currentCrossClearererer;
    private HashSet<MirrorSurface> currentMirrorHits;
    private HashSet<LockedMirrorSurface> currentLockedMirrorHits;


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
        currentFogClearererer = new HashSet<FogClearSurface>(); //FOG
        currentCrossClearererer = new HashSet<CrossSurface>(); // CROSS
        currentMirrorHits = new HashSet<MirrorSurface>(); // NORMAL MIRRORS
        currentLockedMirrorHits = new HashSet<LockedMirrorSurface>(); // LOCKED MIRRORS

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
                    currentMirrorHits.Add(mirror);
                    continue;
                }
                
                if (lockedMirror != null)
                {
                    Transform beamEmitter = lockedMirror.GetTransform();
                    currentOrigin = beamEmitter.position; //Might get changed to hitpoint if it looks too weird
                    currentDirection = beamEmitter.forward.normalized;
                    currentLockedMirrorHits.Add(lockedMirror);
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

        //Play sound effect on new mirrors hit
        PlaySFXOnNewMirrorHits();

        pastMirrorHits = new HashSet<MirrorSurface>(currentMirrorHits);
        pastLockedMirrorHits = new HashSet<LockedMirrorSurface>(currentLockedMirrorHits);

        //Fog enabler if sunbeam has left mirror
        foreach (FogClearSurface fogScript in pastFogClearererer)
        {
            if (!currentFogClearererer.Contains(fogScript))
            {
                fogScript.EnableParticles();
            }
        }
        pastFogClearererer = new HashSet<FogClearSurface>(currentFogClearererer);

        foreach (CrossSurface crossScript in pastCrossClearererer)
        {
            if (!currentCrossClearererer.Contains(crossScript))
            {
                crossScript.CrossAction();
            }
        }
        pastCrossClearererer = new HashSet<CrossSurface>(currentCrossClearererer);

        if (tubeRenderer != null)
        {
          tubeRenderer.SetPositions(points.ToArray()); 
        }

        //lineRenderer.positionCount = points.Count;
        //lineRenderer.SetPositions(points.ToArray());
    }


    private void PlaySFXOnNewMirrorHits()
    {
        foreach (MirrorSurface mirror in currentMirrorHits)
        {
            if (!pastMirrorHits.Contains(mirror))
            {
                mirror.PlayHitSFX();
            }
        }

        foreach (LockedMirrorSurface lockedMirror in currentLockedMirrorHits)
        {
            if (!pastLockedMirrorHits.Contains(lockedMirror))
            {
                lockedMirror.PlayHitSFX();
            }
        }
    }


}
