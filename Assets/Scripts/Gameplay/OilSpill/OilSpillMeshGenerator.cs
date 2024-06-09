using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(SpriteShapeController))]
public class OilSpillMeshGenerator : MonoBehaviour
{
    [field: SerializeField]
    public Bounds Bounds { get; private set; }
    
    private PolygonCollider2D polygonCollider;
    private SpriteShapeController spriteShapeController;

    public void UpdateMesh(Vector2[] polygonPoints)
    {
        Vector3[] vertices = polygonPoints.Select(x => new Vector3(x.x, x.y)).ToArray();
        spriteShapeController.spline.Clear();

        for (int pointID = 0; pointID < vertices.Length; pointID++)
        {
            spriteShapeController.spline.InsertPointAt(pointID, vertices[pointID]);
            spriteShapeController.spline.SetTangentMode(pointID, ShapeTangentMode.Continuous);
        }

        UpdateBoundingBox();
        
        polygonCollider.SetPath(0, polygonPoints);
    }

    [Button]
    public void GenerateFromCollider()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
        spriteShapeController = GetComponent<SpriteShapeController>();
            
        UpdateMesh(polygonCollider.points);
    }

    [Button]
    public void UpdateBoundingBox()
    {
        if (!spriteShapeController)
            spriteShapeController = GetComponent<SpriteShapeController>();
        
        Vector3 boundsMin = new Vector3(float.MaxValue, float.MaxValue, 0);
        Vector3 boundsMax = new Vector3(float.MinValue, float.MinValue, 0);

        for (int pointID = 0; pointID < spriteShapeController.spline.GetPointCount(); pointID++)
        {
            Vector3 point = spriteShapeController.spline.GetPosition(pointID);
            boundsMin.x = Mathf.Min(boundsMin.x, point.x);
            boundsMin.y = Mathf.Min(boundsMin.y, point.y);
            
            boundsMax.x = Mathf.Max(boundsMax.x, point.x);
            boundsMax.y = Mathf.Max(boundsMax.y, point.y);
        }
        
        Vector3 boundsCenter = (boundsMin + boundsMax) * 0.5f;
        Vector3 boundsExtent = boundsMax - boundsCenter;
        Bounds = new Bounds(boundsCenter, boundsExtent);
    }

}