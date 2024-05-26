using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RopeHandler : MonoBehaviour
{
    public event Action<Vector3[] /** ClosedLoopPoints */> OnCloseLoopDetected;
    
    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    private float distanceBetweenMarks = 1f;
    
    [SerializeField]
    private float maxMarksCount = 10f;

    public List<Mark> Marks { get; private set; } = new List<Mark>();
    
    private void Awake()
    {
        PlaceMark();
    }

    private void Update()
    {
        Vector2 lastMarkPosition = Marks.Last().Position;
        float distanceToLastMark = Vector2.Distance(transform.position, lastMarkPosition);
        if (distanceToLastMark >= distanceBetweenMarks)
        {
            PlaceMark();
        }
    }

    public void PlaceMark()
    {
        Mark newMark = new Mark(transform.position);
        Marks.Add(newMark);

        if (Marks.Count > maxMarksCount)
        {
            Marks.RemoveAt(0);
        }

        Vector3[] closedLoop = GetClosedLoop();
        if (closedLoop != null)
        {
            Debug.Log($"Closed loop detected at {closedLoop[^1]}");
            OnCloseLoopDetected?.Invoke(closedLoop);
            
            Marks.Clear();
            PlaceMark();
        }
        
        UpdateMarksVisuals();
    }

    public Vector3[] GetClosedLoop()
    {
        if (Marks.Count < 4)
            return null;
        
        Vector3 lastSegmentStart = Marks[^2].Position;
        Vector3 lastSegmentEnd = Marks[^1].Position;
        
        for (int markID = 0; markID < Marks.Count - 2; markID++)
        {
            // Ugly as hell
            if (MathUtils.SegmentsIntersection(
                    lastSegmentStart, lastSegmentEnd,
                    Marks[markID].Position, Marks[markID + 1].Position,
                    out Vector2 intersection, 0.001f
                    ))
            {
                List<Vector3> result = new ();
                for (int loopMarkId = markID + 1; loopMarkId < Marks.Count - 1; loopMarkId++)
                {
                    result.Add(Marks[loopMarkId].Position);
                }
                
                result.Add(intersection);
                return result.ToArray();
            }
        }

        return null;
    }

    private void UpdateMarksVisuals()
    {
        float depth = lineRenderer.transform.position.z;
        Vector3[] marksPositions = Marks.Select(x => new Vector3(x.Position.x, x.Position.y, depth)).ToArray();
        lineRenderer.positionCount = marksPositions.Length;
        lineRenderer.SetPositions(marksPositions);
    }
}

public class Mark
{
    public Vector2 Position { get; private set; }

    public Mark(Vector2 position)
    {
        this.Position = position;
    }
}