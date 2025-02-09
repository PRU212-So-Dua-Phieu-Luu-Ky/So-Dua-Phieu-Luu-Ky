using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class Enemy : MonoBehaviour
{
    [Header(" Components ")]
    private EnemyMovement movement;

    [Header(" Elements ")]
    private Player player;

    [Header(" Spawn sequence related ")]
    [SerializeField] private SpriteRenderer enemyRenderer;

    [SerializeField] private SpriteRenderer spawnIndicator;
    private bool hasSpawned = false;

    [Header(" Attack ")]
    [SerializeField] private int damage;

    [SerializeField] private int attackFrequency;
    [SerializeField] private float playerDetectionRadius = 0.5f;
    private float attackDelay;
    private float attackTimer;

    [Header(" Effects ")]
    [SerializeField] private ParticleSystem particleSystem;

    [Header(" Debug ")]
    [SerializeField] private bool showGizmos;

    private void Start()
    {
        movement = GetComponent<EnemyMovement>();
        player = FindFirstObjectByType<Player>();

        if (player == null)
        {
            Debug.LogWarning("No player found");
            Destroy(gameObject);
        }
        StartSpawnSequence();
        attackDelay = 1f / attackFrequency;
    }

    private void Update()
    {
        if (attackTimer >= attackDelay)
            TryAttack();
        else Wait();
    }

    private void SetRenderersVisibility(bool visibility)
    {
        enemyRenderer.enabled = visibility;
        spawnIndicator.enabled = !visibility;
    }

    private void TryAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= playerDetectionRadius)
            Attack();
    }

    private void Attack()
    {
        attackTimer = 0f;
        player.TakeDamage(damage);
    }

    private void Wait()
    {
        attackTimer += Time.deltaTime;
    }

    private void EnemyPassAway()
    {
        particleSystem.transform.SetParent(null);
        particleSystem.Play();

        Destroy(gameObject);
    }

    private void StartSpawnSequence()
    {
        SetRenderersVisibility(false);

        Vector3 targetScale = spawnIndicator.transform.localScale * 1.2f;
        LeanTween.scale(spawnIndicator.gameObject, targetScale, .3f).setLoopPingPong(4).setOnComplete(OnSpawnSequenceCompleted);
    }

    private void OnSpawnSequenceCompleted()
    {
        movement.StorePlayer(player);
        SetRenderersVisibility(true);

        hasSpawned = true;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }
}