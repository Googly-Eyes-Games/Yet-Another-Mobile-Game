using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Clipper2Lib;
using erulathra;
using Unity.VisualScripting;
using UnityEngine;

public class OilCleaner : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    private PolygonCollider2D polygonCollider2D;

    [SerializeField]
    private float maxAreaToDestroySpill = 3f;

    [SerializeField]
    private IntSOEvent onScoreChanged;
    
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
            Vector2[] closedRopeLoop2D = closedRopeLoop.Select(x => new Vector2(x.x, x.y)).ToArray();
            SplitSpill(collider, closedRopeLoop2D);
        }
    }

    private void SplitSpill(Collider2D spillCollider, Vector2[] closedRopeLoop)
    {
        OilSpill oilSpill = spillCollider.GetComponent<OilSpill>();
        PolygonCollider2D polygonSpill = spillCollider as PolygonCollider2D;
        
        Vector2[] spillPoints = polygonSpill.points;
        for (int pointID = 0; pointID < spillPoints.Length; pointID++)
        {
            spillPoints[pointID] = polygonSpill.transform.TransformPoint(spillPoints[pointID]);
        }

        PathsD spillPolygon = new PathsD();
        spillPolygon.Add(spillPoints.ToPathD());
        PathsD cleanerPolygon = new PathsD();
        cleanerPolygon.Add(closedRopeLoop.ToPathD());

        PathsD newSpill = Clipper.Difference(spillPolygon, cleanerPolygon, FillRule.NonZero);

        float spillArea = MathUtils.CalculateAreaOfPolygon(spillPoints);
        bool wasProperClean = true;
        
        if (newSpill.Count != 0 && newSpill[0].Count != 0)
        {
            Vector2[] newSpillPoints = newSpill[0].ToVectorArray();
            // No intersection or area of difference is small
            float areaOfNewSpill = MathUtils.CalculateAreaOfPolygon(newSpillPoints);

            wasProperClean = false;

            if (areaOfNewSpill > maxAreaToDestroySpill)
            {
                for (int pointID = 0; pointID < newSpillPoints.Length; pointID++)
                {
                    newSpillPoints[pointID] = polygonSpill.transform.InverseTransformPoint(newSpillPoints[pointID]);
                }

                polygonSpill.points = newSpillPoints;
                OilSpillMeshGenerator meshGenerator = polygonSpill.GetComponent<OilSpillMeshGenerator>();
                meshGenerator.GenerateFromCollider();
                
                return;
            }
        }

        ScoreSubsystem scoreSubsystem = SceneSubsystemManager.GetSubsystem<ScoreSubsystem>();
        if (wasProperClean)
        {
            float selectedArea = MathUtils.CalculateAreaOfPolygon(closedRopeLoop);
            scoreSubsystem.AddWholeClearedSpillScore(selectedArea, spillArea);
        }
        else
        {
            scoreSubsystem.AddBaseScore();
        }
        
        oilSpill.FullClean();
        Destroy(spillCollider.GameObject());
    }
}
