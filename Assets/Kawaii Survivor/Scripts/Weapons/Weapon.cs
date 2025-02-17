using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private enum State
    {
        Idle,
        Attack
    }

    private State state;

    [Header("Elements")]
    [SerializeField] private Transform hitDetectionTransform;

    [SerializeField] private float hitDetectionRadius;
    [SerializeField] private BoxCollider2D hitCollider;

    [Header("Animation")]
    [SerializeField] private float aimLerp;

    [SerializeField] private Animator animator;

    [Header("Settings")]
    [SerializeField] private float weaponRange;

    [SerializeField] private LayerMask enemyMask;

    [Header("Attack")]
    [SerializeField] private int damage;

    [SerializeField] private float attackDelay;
    private float attackTimer;

    private List<Enemy> damagedEnemies = new();

    private void Start()
    {
        state = State.Idle;
    }

    // Update is called once per frame
    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                AimAtClosestEnemy();
                break;

            case State.Attack:
                Attacking();
                break;
        }
    }

    [NaughtyAttributes.Button]
    private void StartAttack()
    {
        animator.Play("Attack");
        state = State.Attack;
        animator.speed = 1f / attackDelay;
        damagedEnemies.Clear();
    }

    private void Attacking()
    {
        Attack();
    }

    private void StopAttack()
    {
        state = State.Idle;
        damagedEnemies.Clear();
    }

    private void Attack()
    {
        var enemies = Physics2D.OverlapBoxAll(
            hitDetectionTransform.position, 
            hitCollider.bounds.size, 
            hitDetectionTransform.localEulerAngles.z, 
            enemyMask);
        for (int i = 0; i < enemies.Length; i++)
        {
            var enemy = enemies[i].GetComponent<Enemy>();
            if (!damagedEnemies.Contains(enemy))
            {
                damagedEnemies.Add(enemy);
                enemy.TakeDamage(damage);
            }
        }
    }

    private void AimAtClosestEnemy()
    {
        Enemy closestEnemy = null;
        Vector2 targetUpVector = Vector3.up;

        var enemies = Physics2D.OverlapCircleAll(transform.position, weaponRange, enemyMask);

        if (enemies.Length <= 0)
        {
            ResetAim();
            return;
        }

        closestEnemy = FindClosestEnemy(enemies);

        if (closestEnemy != null)
        {
            targetUpVector = (closestEnemy.transform.position - transform.position).normalized;
            transform.up = targetUpVector;
            ManageAttackTimer();
        }

        SmoothAim(targetUpVector);
        IncrementAttackTimer();
    }

    private void ManageAttackTimer()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackDelay)
        {
            attackTimer = 0;
            StartAttack();
        }
    }

    private void IncrementAttackTimer()
    {
        attackTimer += Time.deltaTime;
    }

    private void ResetAim()
    {
        transform.up = Vector3.up;
    }

    private Enemy FindClosestEnemy(Collider2D[] enemies)
    {
        Enemy closestEnemy = null;
        float minDistance = weaponRange;

        foreach (var enemyCollider in enemies)
        {
            var enemy = enemyCollider.GetComponent<Enemy>();
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < minDistance)
            {
                closestEnemy = enemy;
                minDistance = distanceToEnemy;
            }
        }

        return closestEnemy;
    }

    private void SmoothAim(Vector2 targetUpVector)
    {
        transform.up = Vector3.Lerp(transform.up, targetUpVector, Time.deltaTime * aimLerp);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, weaponRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(hitDetectionTransform.position, hitDetectionRadius);
    }
}