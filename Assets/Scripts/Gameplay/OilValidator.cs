using UnityEngine;

public class OilValidator : MonoBehaviour
{
    [SerializeField]
    private SOEvent onGameOver;

    [SerializeField]
    private Collider2D trigger;

    public Vector2 Start => trigger.bounds.center.ToVector2() + (trigger.bounds.extents.x * Vector2.left);
    public Vector2 End => trigger.bounds.center.ToVector2() + (trigger.bounds.extents.x * Vector2.right);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isActiveAndEnabled && other.CompareTag("Oil"))
        {
            onGameOver?.Invoke();
        }
    }
}
