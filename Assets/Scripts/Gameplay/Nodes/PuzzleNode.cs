using System;
using System.Linq;
using UnityEngine;

public class PuzzleNode : MonoBehaviour
{
    // Definitely we need tool to set up boards
    
    [field: SerializeField]
    public Vector2Int GridIndex { get; private set; }

    [HideInInspector]
    public BoardLogic boardLogic;

    public event Action OnSelection;

    public virtual void Select()
    {
        OnSelection?.Invoke();
    }
    public virtual void UnSelect() { }

    public virtual bool ValidateConnection(PuzzleNode previousNode)
    {
        PuzzleNode[] neighbours = boardLogic.GetNeighbourNodes(GridIndex);
        bool result = neighbours.Contains(previousNode);

        return result;
    }
}