using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class OilCleaner : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    private PolygonCollider2D polygonCollider2D;
    
    public void CleanOil(Vector3[] closedRopeLoop)
    {
        lineRenderer.positionCount = closedRopeLoop.Length;
        lineRenderer.SetPositions(closedRopeLoop);
        lineRenderer.loop = true;

        Vector2[] points2D = new Vector2[closedRopeLoop.Length];
        for (int pointID = 0; pointID < closedRopeLoop.Length; pointID++)
        {
            Vector3 localPoint3D = transform.InverseTransformPoint(closedRopeLoop[pointID]);
            points2D[pointID] = new Vector2(localPoint3D.x, localPoint3D.y);
        }
        
        polygonCollider2D.SetPath(0, points2D);

        ContactFilter2D contactFilter2D = new();
        contactFilter2D.layerMask = LayerMask.GetMask("Oil");

        List<Collider2D> overlappedColliders = new();
        polygonCollider2D.OverlapCollider(contactFilter2D, overlappedColliders);

        foreach (Collider2D collider in overlappedColliders)
        {
            OilSpill oilSpill = collider.GetComponent<OilSpill>();
            if (oilSpill)
            {
                oilSpill.FullClean();
            }
            
            Destroy(collider.GameObject());
        }
    }
}
