using System;
using System.Collections.Generic;
using System.Linq;
using Clipper2Lib;
using erulathra;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class OilCleaner : MonoBehaviour
{
    public event Action<float> OnCleanSpill;
    public event Action OnPartlyClean;
    
    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    private PolygonCollider2D polygonCollider2D;

    [SerializeField]
    private float maxAreaToDestroySpill = 3f;

    [SerializeField]
    private IntSOEvent onScoreChanged;
    
    [SerializeField]
    private SOEvent onClean;
    
    [SerializeField]
    private SOEvent onWrongClean;

    [SerializeField]
    private GameObject CleanedSpillPrefab;
    
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
            if (!collider.CompareTag("Oil"))
                continue;
            
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

                polygonSpill.points = MathUtils.OptimizePolygon(newSpillPoints, 0.3f);
                OilSpillMeshGenerator meshGenerator = polygonSpill.GetComponent<OilSpillMeshGenerator>();
                meshGenerator.GenerateFromCollider();
                meshGenerator.EnableWarning = true;
                
                PathsD cleanedSpill = Clipper.Intersect(spillPolygon, cleanerPolygon, FillRule.NonZero);
                SpawnCleanedSpill(MathUtils.OptimizePolygon(cleanedSpill[0].ToVectorArray(), 0.3f));

                OnPartlyClean?.Invoke();
                onWrongClean?.Invoke();
                
                return;
            }
        }

        ScoreSubsystem scoreSubsystem = SceneSubsystemManager.GetSubsystem<ScoreSubsystem>();
        if (wasProperClean)
        {
            float selectedArea = MathUtils.CalculateAreaOfPolygon(closedRopeLoop);
            scoreSubsystem.AddWholeClearedSpillScore(selectedArea, spillArea);

            OnCleanSpill?.Invoke(spillArea / selectedArea);
        }
        else
        {
            scoreSubsystem.AddBaseScore();
            OnCleanSpill?.Invoke(-1f);
        }
        
        onClean?.Invoke();
        
        oilSpill.FullClean();
        
        SpriteShapeController spriteShapeController = polygonSpill.GetComponent<SpriteShapeController>();
        SpawnCleanedSpill(spriteShapeController);
        
        Destroy(spillCollider.GameObject());
    }

    private void SpawnCleanedSpill(Vector2[] cleanedSpill)
    {
        GameObject cleanedSpillGameObject = Instantiate(CleanedSpillPrefab);
        
        cleanedSpillGameObject.transform.position += Vector3.forward * 5f;
        PolygonCollider2D cleanedSpillPolygon = cleanedSpillGameObject.GetComponent<PolygonCollider2D>();
        cleanedSpillPolygon.points = cleanedSpill;
        OilSpillMeshGenerator cleanedSpillMeshGenerator = cleanedSpillGameObject.GetComponent<OilSpillMeshGenerator>();
        cleanedSpillMeshGenerator.GenerateFromCollider();
        
    }
    
    private void SpawnCleanedSpill(SpriteShapeController otherShape)
    {
        GameObject cleanedSpillGameObject = Instantiate(CleanedSpillPrefab);
        cleanedSpillGameObject.transform.position = otherShape.transform.position;
        cleanedSpillGameObject.transform.rotation = otherShape.transform.rotation;
        
        cleanedSpillGameObject.transform.position += Vector3.forward * 5f;
        OilSpillMeshGenerator cleanedSpillMeshGenerator = cleanedSpillGameObject.GetComponent<OilSpillMeshGenerator>();
        cleanedSpillMeshGenerator.GenerateFromShape(otherShape);
        
    }
}
