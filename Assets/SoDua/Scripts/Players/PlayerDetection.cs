using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerDetection : MonoBehaviour
{
    [Header("Colliders")]
    [SerializeField] private Collider2D collectableCollider;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out ICollectables collectables))
        {
            if (!collider.IsTouching(collectableCollider)) return;

            collectables.Collect(GetComponent<Player>());
        }
    }
}
