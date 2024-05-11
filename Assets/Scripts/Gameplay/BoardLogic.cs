using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardLogic : MonoBehaviour
{
    [field: SerializeField]
    private LineLogic lineLogic;

    [field: SerializeField]
    private Transform nodesParentTransform;

    [field: SerializeField]
    private Transform tilesParentTransform;

    private readonly Dictionary<Vector2Int, PuzzleNode> nodes = new();
    private readonly Dictionary<Vector2Int, PuzzleTile> tiles = new();

    /** Relative neighbour coordinates to tile */
    public static readonly Vector2Int[] NeighboursCoords = new[]
    {
        new Vector2Int(-1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
    };
    

    /** Relative node coordinates to tile, indexed by direction */
    public static readonly Dictionary<Vector2Int, NodeSegment> TileDirectionToNodeCoords = new()
    {
        [new Vector2Int(-1, 0)] = new NodeSegment(new Vector2Int(0, 0), new Vector2Int(0, 1)),
        [new Vector2Int(0, 1)] = new NodeSegment(new Vector2Int(0, 1), new Vector2Int(1, 1)),
        [new Vector2Int(1, 0)] = new NodeSegment(new Vector2Int(1, 0), new Vector2Int(1, 1)),
        [new Vector2Int(0, -1)] = new NodeSegment(new Vector2Int(0, 0), new Vector2Int(1, 0)),
    };

    public event Action OnPathCompleted;
    public event Action<bool> OnGameFinish;

    public PuzzleNode[] GetNeighbourNodes(Vector2Int gridIndex)
    {
        return GetNeighbours(nodes, gridIndex);
    }

    public PuzzleTile[] GetNeighbourTiles(Vector2Int gridIndex)
    {
        return GetNeighbours(tiles, gridIndex);
    }

    private T[] GetNeighbours<T>(Dictionary<Vector2Int, T> gridCollection, Vector2Int gridIndex)
    {
        List<T> neighbours = new();
        foreach (Vector2Int neighbour in NeighboursCoords)
        {
            Vector2Int neighbourIndex = gridIndex + neighbour;

            if (gridCollection.ContainsKey(neighbourIndex))
            {
                neighbours.Add(gridCollection[neighbourIndex]);
            }
        }

        return neighbours.ToArray();
    }

    /** Works only for neighbours tiles */
    public bool IsTilesSeparated(PuzzleNode[] checkedNodes, PuzzleTile tileOne, PuzzleTile tileTwo)
    {
#if UNITY_EDITOR
        if (!GetNeighbourTiles(tileOne.GridIndex).Contains(tileTwo))
        {
            throw new NotImplementedException();
        }
#endif

        Vector2Int direction = tileTwo.GridIndex - tileOne.GridIndex;
        NodeSegment nodeSegmentRelative = TileDirectionToNodeCoords[direction];
        NodeSegment nodeSegment = new NodeSegment(
            nodeSegmentRelative.First + tileOne.GridIndex,
            nodeSegmentRelative.Second + tileOne.GridIndex
        );

        return BoardUtils.IsPathContainSegment(checkedNodes, nodeSegment);
    }

    private void Awake()
    {
        FindAndInitializeNodes();
        FindAndInitializeTiles();
    }

    private void FindAndInitializeNodes()
    {
        PuzzleNode[] foundNodes = nodesParentTransform.GetComponentsInChildren<PuzzleNode>();
        foreach (PuzzleNode node in foundNodes)
        {
            nodes.Add(node.GridIndex, node);
            node.boardLogic = this;

            if (node is EndPuzzleNode)
            {
                node.OnSelection += OnEndNodeSelected;
            }
        }
    }
    
    private void FindAndInitializeTiles()
    {
        PuzzleTile[] foundTiles = tilesParentTransform.GetComponentsInChildren<PuzzleTile>();
        foreach (PuzzleTile tile in foundTiles)
        {
            tiles.Add(tile.GridIndex, tile);
            tile.boardLogic = this;
        }
    }

    private void OnEndNodeSelected()
    {
        OnPathCompleted?.Invoke();

        bool solutionValid = lineLogic.IsPathValid();

        PuzzleNode[] path = lineLogic.GetCheckedNodes();
        
        foreach (PuzzleTile tile in tiles.Values)
        {
            if (!tile.ValidatePath(path))
            {
                solutionValid = false;
            }
        }
        
        OnGameFinish?.Invoke(solutionValid);
    }
}