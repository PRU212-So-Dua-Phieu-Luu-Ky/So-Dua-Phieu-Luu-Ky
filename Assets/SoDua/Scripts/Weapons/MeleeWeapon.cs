using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeleeWeapon : Weapon
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
    [SerializeField] private Animator animator;

    private List<Enemy> damagedEnemies = new();

    [Header("SFX")]
    [SerializeField] private AudioSource audioSource;
    private bool IsPlayed;

    private void Start()
    {
        state = State.Idle;
        IsPlayed = false;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                IsPlayed = false;
                AimAtClosestEnemy();
                break;

            case State.Attack:
                Attacking();
                break;
        }
    }

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
        PlaySound();
    }

    private void PlaySound()
    {
        if (!IsPlayed)
        {
            audioSource.Play();
            IsPlayed = true;
        }
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
                int damage = GetDamage(out bool isCriticalHit);

                damagedEnemies.Add(enemy);
                enemy.TakeDamage(damage, isCriticalHit);
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

        if (enemies != null && enemies.ElementAt(0) != null)
        {
            closestEnemy = FindClosestEnemy(enemies);
        }

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
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, weaponRange);

        if (hitDetectionTransform == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(hitDetectionTransform.position, hitDetectionRadius);
    }

    public override void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        // Base stats based on level
        ConfigureStats();

        damage = (int)(damage * (1 + playerStatsManager.GetStatValue(Stat.Attack) / 100));
        attackDelay /= 1 + (playerStatsManager.GetStatValue(Stat.AttackSpeed) / 100);

        criticalChance = Mathf.RoundToInt(criticalChance * (1 + playerStatsManager.GetStatValue(Stat.CriticalChance) / 100));
        criticalPercent += playerStatsManager.GetStatValue(Stat.CriticalPercent);
    }
}