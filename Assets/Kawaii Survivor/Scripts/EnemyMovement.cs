using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header(" Elements ")]
    private Player player;

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float playerDetectionRadius = 0.5f;

    [Header(" Debug ")]
    [SerializeField] private bool showGizmos;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();

        if (player == null)
        {
            Debug.LogWarning("No player found");
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        FollowPlayer();

        TryAttack();
    }

    private void FollowPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;

        Vector2 targetPosition = (Vector2)transform.position + direction * moveSpeed * Time.deltaTime;

        transform.position = targetPosition;
    }

    private void TryAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= playerDetectionRadius)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }
}