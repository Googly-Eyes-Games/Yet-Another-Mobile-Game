using System;
using UnityEngine;

public class LoopDebugger : MonoBehaviour
{
    [SerializeField]
    private RopeHandler ropeHandler;
    
    [SerializeField]
    private LineRenderer lineRenderer;

    private void Awake()
    {
        ropeHandler.OnCloseLoopDetected += points =>
        {
            lineRenderer.positionCount = points.Length;
            lineRenderer.SetPositions(points);
        };
    }
}
