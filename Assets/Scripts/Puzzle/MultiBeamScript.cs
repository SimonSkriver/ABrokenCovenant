using UnityEngine;
using System.Collections.Generic;

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

    [Header("Sets")] // Used to check if new mirrors are hit or if beam has moved away from mirrors again.
    private HashSet<FogClearSurface> pastFogClearererer = new HashSet<FogClearSurface>(); // FOG
    private HashSet<CrossSurface> pastCrossClearererer = new HashSet<CrossSurface>(); // CROSS
    private HashSet<MirrorSurface> pastMirrorHits = new HashSet<MirrorSurface>(); // NORMAL MIRRORS
    private HashSet<LockedMirrorSurface> pastLockedMirrorHits = new HashSet<LockedMirrorSurface>(); // LOCKED MIRRORS

    // Currently hit mirrors which are constantly being replaced/updated in drawbeam
    private HashSet<FogClearSurface> currentFogClearererer;
    private HashSet<CrossSurface> currentCrossClearererer;
    private HashSet<MirrorSurface> currentMirrorHits;
    private HashSet<LockedMirrorSurface> currentLockedMirrorHits;


    void Start()
    {
        // Reassures it doesnt break if gone from inspector
        if (rayBase == null)
            rayBase = transform;

        if (lineRenderer == null)
            lineRenderer = GetComponentInChildren<LineRenderer>();

        // Initialize Beam
        DrawBeam();
    }

    void Update()
    {
        DrawBeam();
    }


    private void DrawBeam()
    {
        currentFogClearererer = new HashSet<FogClearSurface>(); // FOG
        currentCrossClearererer = new HashSet<CrossSurface>(); // CROSS
        currentMirrorHits = new HashSet<MirrorSurface>(); // NORMAL MIRRORS
        currentLockedMirrorHits = new HashSet<LockedMirrorSurface>(); // LOCKED MIRRORS

        // List of points used to set positions in the LineRenderer and TubeRenderer
        List<Vector3> points = new List<Vector3>();

        // Initialize start position
        Vector3 currentOrigin = rayBase.position;
        Vector3 currentDirection = rayBase.forward.normalized;

        points.Add(currentOrigin);

        // Raycast loop to generate Vector3 points with maxBounce limit to ensure it doesnt go ham if two mirrors are pointed at eachother
        for (int bounce = 0; bounce < maxBounces; bounce++)
        {
            // Initialize a Ray from current position start position
            Ray ray = new Ray(currentOrigin, currentDirection);

            // Start a Raycast and returns what we hit
            // layersToHit is set in inspector exluding the layer given to some invisible colliders throughout the game
            // QueryTriggerInteraction.Collide became obsolete as FogClearSurface now handles the removal of crazyfog
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layersToHit, QueryTriggerInteraction.Collide))
            {
                // Whatever we hit add that point
                points.Add(hit.point);

                Debug.DrawRay(currentOrigin, currentDirection * hit.distance, Color.red);

                // Check if the surface we hit contains any of these scripts
                MirrorSurface mirror = hit.collider.GetComponentInParent<MirrorSurface>();
                LockedMirrorSurface lockedMirror = hit.collider.GetComponentInParent<LockedMirrorSurface>();
                FogClearSurface fogClearSurface = hit.collider.GetComponentInParent<FogClearSurface>();
                CrossSurface crossSurface = hit.collider.GetComponentInParent<CrossSurface>();

                // If a cross was hit
                if (crossSurface != null)
                {
                    // Register the point for TubeRenderer
                    Vector3 endPoint = hit.point;
                    points.Add(endPoint);

                    // Start a new light beam or open church door depending on which type of cross
                    crossSurface.CrossAction();

                    // Add that cross to the list of currently hit crosses
                    currentCrossClearererer.Add(crossSurface);

                    // Exit the for loop as that was the final point
                    break;
                }

                if (fogClearSurface != null)
                {
                    // Disables particles
                    fogClearSurface.DisableParticles();

                    // Add that mirror to the list of currently hit fog remover mirrors
                    currentFogClearererer.Add(fogClearSurface);

                    // No continue because we still need to see what other type the mirror was
                }

                if (mirror != null)
                {
                    // If a normal mirror was hit set the direction using Vector3.Reflect calculating an accurate direction depending on angle
                    currentDirection = Vector3.Reflect(currentDirection, hit.normal).normalized;
                    currentOrigin = hit.point + currentDirection * surfaceOffset;

                    // Add that mirror to the list of currently hit mirrors
                    currentMirrorHits.Add(mirror);
                    continue;
                }
                
                if (lockedMirror != null)
                {
                    // If locked position mirror was hit set the new start point and direction to that mirrors beamEmitter
                    Transform beamEmitter = lockedMirror.GetEmitterTransform();
                    currentOrigin = beamEmitter.position;
                    currentDirection = beamEmitter.forward.normalized;

                    // Add that mirror to the list of currently hit locked mirrors
                    currentLockedMirrorHits.Add(lockedMirror);
                    continue;
                }
            }
            else
            {
                // If nothing was hit, set an endpoint far away in the direction you're pointing as a point is needed to render the beam.
                Vector3 endPoint = currentOrigin + currentDirection * maxDistance;
                points.Add(endPoint);

                Debug.DrawRay(currentOrigin, currentDirection * maxDistance, Color.red);

                //Exit the for loop if no mirrors were hit as it shouldnt bounce anymore.
                break;
            }
        }

        // Play sound effect on new mirrors that was hit
        PlaySFXOnNewMirrorHits();

        // New normal and locked mirror hits just played their sound so we update the list of pastHits with the current hits.
        pastMirrorHits = new HashSet<MirrorSurface>(currentMirrorHits);
        pastLockedMirrorHits = new HashSet<LockedMirrorSurface>(currentLockedMirrorHits);

        // If the list of currently hit fog removers doesnt contain the one from past hits its no longer on it
        // in that case re-enable the crazy fog particles
        foreach (FogClearSurface fogScript in pastFogClearererer)
        {
            if (!currentFogClearererer.Contains(fogScript))
            {
                fogScript.EnableParticles();
            }
        }
        // Update past fog hits to the ones hit this iteration
        pastFogClearererer = new HashSet<FogClearSurface>(currentFogClearererer);

        // Same logic as previous, trigger CrossAction on old CrossScript again to reverse initial action
        foreach (CrossSurface crossScript in pastCrossClearererer)
        {
            if (!currentCrossClearererer.Contains(crossScript))
            {
                crossScript.CrossAction();
            }
        }
        // Update list
        pastCrossClearererer = new HashSet<CrossSurface>(currentCrossClearererer);

        // At last set the gathered Vector3 positions from which the tubeRenderer (MeshRender) generates the beam from and to.
        if (tubeRenderer != null)
        {
          tubeRenderer.SetPositions(points.ToArray()); 
        }
    }

    // Run through all current mirror hits and if there's a new one, not found the old list, play a sound from that mirror
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