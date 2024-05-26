using System;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(LineRenderer))]
public class OilSpillMeshGenerator : MonoBehaviour
{
    private PolygonCollider2D polygonCollider;
    private MeshFilter meshFilter;
    private LineRenderer lineRenderer;

    private void Start()
    {
        UpdateDefaultPoints();
    }

    public void UpdateMesh(Vector2[] polygonPoints)
    {
        Vector3[] vertices = polygonPoints.Select(x => new Vector3(x.x, x.y)).ToArray();

        Triangulator triangulator = new Triangulator(polygonPoints);
        int[] indices = triangulator.Triangulate();

        Mesh newMesh = new Mesh()
        {
            vertices = vertices,
            triangles = indices
        };
        
        newMesh.RecalculateNormals();
        newMesh.RecalculateBounds();

        meshFilter.sharedMesh = newMesh;
        polygonCollider.SetPath(0, polygonPoints);
        lineRenderer.positionCount = vertices.Length;
        lineRenderer.SetPositions(vertices);
        lineRenderer.useWorldSpace = false;
    }

    [Button]
    private void UpdateDefaultPoints()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
        meshFilter = GetComponent<MeshFilter>();
        lineRenderer = GetComponent<LineRenderer>();
            
        UpdateMesh(polygonCollider.points);
    }
}