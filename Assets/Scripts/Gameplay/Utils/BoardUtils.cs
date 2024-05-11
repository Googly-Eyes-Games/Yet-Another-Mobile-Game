using UnityEngine;

public struct NodeSegment
{
    public Vector2Int First;
    public Vector2Int Second;

    public NodeSegment(Vector2Int first, Vector2Int second)
    {
        First = first;
        Second = second;
    }
}

public static class BoardUtils
{
    public static bool IsPathContainSegment(PuzzleNode[] path, NodeSegment nodeSegment)
    {
        if (path.Length < 2)
        {
            return false;
        }
        
        for (int nodeID = 0; nodeID < path.Length - 1; nodeID++)
        {
            PuzzleNode actualNode = path[nodeID];
            PuzzleNode nextNode = path[nodeID + 1];

            if (nodeSegment.First == actualNode.GridIndex && nodeSegment.Second == nextNode.GridIndex
                || nodeSegment.First == nextNode.GridIndex && nodeSegment.Second == actualNode.GridIndex)
            {
                return true;
            }
        }

        return false;
    }
}