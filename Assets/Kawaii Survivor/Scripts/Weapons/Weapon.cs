using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] protected float aimLerp;


    [Header("Settings")]
    [SerializeField] protected float weaponRange;
    [SerializeField] protected LayerMask enemyMask;

    [Header("Attack")]
    [SerializeField] protected int damage;

    [SerializeField] protected float attackDelay;
    protected float attackTimer;


    protected void SmoothAim(Vector2 targetUpVector)
    {
        transform.up = Vector3.Lerp(transform.up, targetUpVector, Time.deltaTime * aimLerp);
    }

    protected void ResetAim()
    {
        transform.up = Vector3.up;
    }

    protected Enemy FindClosestEnemy(Collider2D[] enemies)
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

    protected int GetDamage(out bool isCriticalHit)
    {
        if (Random.Range(0, 101) <= 50)
        {
            isCriticalHit = true;
            return damage * 2;
        }

        isCriticalHit = false;
        return damage;
    }
}
