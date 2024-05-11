using DefaultNamespace;
using UnityEngine;

public class EndPuzzleNode : PuzzleNode
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public override void Select()
    {
        base.Select();
        spriteRenderer.color = ColorPalette.Get().ForeGroundSelected;
    }

    public override void UnSelect()
    {
        base.UnSelect();
        spriteRenderer.color = ColorPalette.Get().ForeGroundUnselected;
    }
}