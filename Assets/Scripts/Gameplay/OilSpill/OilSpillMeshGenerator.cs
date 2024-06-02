using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(SpriteShapeController))]
public class OilSpillMeshGenerator : MonoBehaviour
{
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

        polygonCollider.SetPath(0, polygonPoints);
    }

    [Button]
    public void GenerateFromCollider()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
        spriteShapeController = GetComponent<SpriteShapeController>();
            
        UpdateMesh(polygonCollider.points);
    }
}