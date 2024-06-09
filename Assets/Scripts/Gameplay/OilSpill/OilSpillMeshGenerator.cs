using System.Linq;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(SpriteShapeController))]
public class OilSpillMeshGenerator : MonoBehaviour
{
    public Bounds Bounds => spriteShapeRenderer.bounds;

    private bool enableWarning;
    public bool EnableWarning
    {
        get => enableWarning;
        set
        {
            enableWarning = value;
            spriteShapeRenderer.material.SetFloat(ShaderLookUp.Warning, value ? 1f : 0f);
        }
    }
    
    [Foldout("Components")]
    [SerializeField]
    private PolygonCollider2D polygonCollider;
    
    [Foldout("Components")]
    [SerializeField]
    private SpriteShapeController spriteShapeController;
    
    [Foldout("Components")]
    [SerializeField]
    private SpriteShapeRenderer spriteShapeRenderer;

    private void UpdateMesh(Vector2[] polygonPoints)
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
        UpdateMesh(polygonCollider.points);
    }

    private static class ShaderLookUp
    {
        public static int Warning = Shader.PropertyToID("_Warning");
    }
}