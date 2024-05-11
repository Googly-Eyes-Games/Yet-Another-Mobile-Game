
using System;
using DefaultNamespace;
using UnityEngine;

public enum ColorTileType
{
    Water,
    Land,
    Oil
}
public class ColorPuzzleTile : PuzzleTile
{
    [SerializeField]
    private ColorTileType colorTileType;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public override bool ValidatePath(PuzzleNode[] checkedNodes)
    {
        PuzzleTile[] neighbourTiles = boardLogic.GetNeighbourTiles(GridIndex);

        // Check is all neighbours have same color or are separated by node segment
        foreach (PuzzleTile neighbour in neighbourTiles)
        {
            if (neighbour is not ColorPuzzleTile colorNeighbour)
                continue;

            if (colorNeighbour.colorTileType == ColorTileType.Land
                || colorNeighbour.colorTileType == colorTileType)
                continue;

            if (!boardLogic.IsTilesSeparated(checkedNodes, this, neighbour))
            {
                return false;
            }
        }
        
        return true;
    }

    private void ApplyColor()
    {
        switch (colorTileType)
        {
            case ColorTileType.Water:
                spriteRenderer.color = ColorPalette.Get().WaterTile;
                break;
            case ColorTileType.Land:
                spriteRenderer.color = ColorPalette.Get().LandTile;
                break;
            case ColorTileType.Oil:
                spriteRenderer.color = ColorPalette.Get().OilTile;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Awake()
    {
        ApplyColor();
    }

    private void OnValidate()
    {
        ApplyColor();
    }
}