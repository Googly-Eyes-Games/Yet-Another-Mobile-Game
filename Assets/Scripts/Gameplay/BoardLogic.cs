using System.Collections.Generic;
using UnityEngine;

public class BoardLogic : MonoBehaviour
{
    [field: SerializeField]
    public Vector2Int BoardSize { get; private set; } = new Vector2Int(3, 3);

    [field: SerializeField]
    private Transform nodesParentTransform;

    private readonly Dictionary<Vector2Int, PuzzleNode> nodes = new();

    public PuzzleNode GetNode(Vector2Int gridIndex)
    {
        return nodes[gridIndex];
    }

    public PuzzleNode[] GetNeighbourNodes(Vector2Int gridIndex)
    {
        Vector2Int[] defaultNeighbours = new[]
        {
            new Vector2Int(-1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(1, 0),
            new Vector2Int(0, 1),
        };

        List<PuzzleNode> neighboursNodes = new();
        foreach (Vector2Int neighbour in defaultNeighbours)
        {
            Vector2Int neighbourIndex = gridIndex + neighbour;
            
            if (nodes.ContainsKey(neighbourIndex))
            {
                neighboursNodes.Add(nodes[neighbourIndex]);
            }
        }

        return neighboursNodes.ToArray();
    }
    
    private void Awake()
    {
        PuzzleNode[] foundNodes = nodesParentTransform.GetComponentsInChildren<PuzzleNode>();
        foreach (PuzzleNode node in foundNodes)
        {
            nodes.Add(node.GridIndex, node);
            node.boardLogic = this;
        }
    }
}