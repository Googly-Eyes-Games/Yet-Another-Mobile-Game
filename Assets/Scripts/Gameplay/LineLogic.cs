using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SFGPlayerInput))]
public class LineLogic : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;

    private SFGPlayerInput playerInput;

    private PuzzleNode lastHitNode;
    private PuzzleNode lastSelectedNode;
    private readonly List<PuzzleNode> checkedNodes = new();

    private List<PuzzleNode> GetCheckedNodes()
    {
        return new List<PuzzleNode>(checkedNodes);
    }

    private void Awake()
    {
        playerInput = GetComponent<SFGPlayerInput>();
    }

    private void OnEnable()
    {
        playerInput.OnUnPress += HandleUnPress;
    }

    private void OnDisable()
    {
        playerInput.OnUnPress -= HandleUnPress;
    }

    private void Update()
    {
        if (playerInput.IsPressed)
        {
            HandleLineLogic();
        }
    }

    private void HandleLineLogic()
    {
        Camera mainCam = Camera.main;
        if (mainCam)
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCam.ScreenToWorldPoint(playerInput.TouchPosition), Vector2.zero);
            if (hit)
            {
                PuzzleNode hitNode = hit.collider.GetComponent<PuzzleNode>();

                if (hitNode != lastHitNode)
                {
                    if (!checkedNodes.Contains(hitNode) && hitNode.ValidateConnection(lastSelectedNode))
                    {
                        // Select node
                        checkedNodes.Add(hitNode);
                        lastSelectedNode = hitNode;
                        hitNode.Select();
                        UpdateLineRenderer();
                    }
                    else if (checkedNodes.Count >= 2 && checkedNodes[^2] == hitNode)
                    {
                        // Unselect node
                        lastSelectedNode = hitNode;
                        checkedNodes[^1].UnSelect();
                        checkedNodes.RemoveAt(checkedNodes.Count - 1);
                        UpdateLineRenderer();
                    }
                    
                    lastHitNode = hitNode;
                }
            }
        }
    }

    private void UpdateLineRenderer()
    {
        lineRenderer.positionCount = checkedNodes.Count;

        for (int nodeID = 0; nodeID < checkedNodes.Count; nodeID++)
        {
            lineRenderer.SetPosition(nodeID, checkedNodes[nodeID].transform.position);
        }
    }

    private void HandleUnPress()
    {
        foreach (PuzzleNode node in checkedNodes)
        {
            node.UnSelect();
        }
        
        checkedNodes.Clear();
        lastHitNode = null;
        lastSelectedNode = null;
        
        UpdateLineRenderer();
    }
}
