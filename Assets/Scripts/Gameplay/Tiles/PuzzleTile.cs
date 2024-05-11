using UnityEngine;

public class PuzzleTile : MonoBehaviour
{
    [field: SerializeField]
    public Vector2Int GridIndex { get; private set; }

    [HideInInspector]
    public BoardLogic boardLogic;

    public virtual bool ValidatePath(PuzzleNode[] checkedNodes)
    {
        return true;
    }
}