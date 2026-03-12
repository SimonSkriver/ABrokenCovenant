using UnityEngine;
using System.Collections.Generic;

public class MultiBeamScript : MonoBehaviour
{
[Header("Beam Setup")]
    [SerializeField] private Transform rayBase;
    [SerializeField] private LineRenderer lineRenderer;

    [Header("Beam Settings")]
    [SerializeField] private float maxDistance = 25f;
    [SerializeField] private int maxBounces = 10;
    [SerializeField] private LayerMask layersToHit;
    [SerializeField] private float surfaceOffset = 0.02f;

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

                if (mirror != null)
                {
                    currentDirection = Vector3.Reflect(currentDirection, hit.normal).normalized;
                    currentOrigin = hit.point + currentDirection * surfaceOffset;
                    continue;
                }

                break;
            }
            else
            {
                Vector3 endPoint = currentOrigin + currentDirection * maxDistance;
                points.Add(endPoint);

                Debug.DrawRay(currentOrigin, currentDirection * maxDistance, Color.red);
                break;
            }
        }

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }
}
