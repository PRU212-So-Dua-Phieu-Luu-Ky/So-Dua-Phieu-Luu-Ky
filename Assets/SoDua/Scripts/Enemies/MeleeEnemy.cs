using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class MeleeEnemy : Enemy
{
    [Header(" Attack ")]
    [SerializeField] protected int damage;

    [SerializeField] protected int attackFrequency;
    protected float attackDelay;
    protected float attackTimer;

    protected override void Start()
    {
        base.Start();
        attackDelay = 1f / attackFrequency;
    }

    private void Update()
    {
        if (!CanAttack()) return;

        if (attackTimer >= attackDelay)
            TryAttack();
        else Wait();

        movement.FollowPlayer();
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
        Debug.Log("Player taking damage");
    }

    private void Wait()
    {
        attackTimer += Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }
}