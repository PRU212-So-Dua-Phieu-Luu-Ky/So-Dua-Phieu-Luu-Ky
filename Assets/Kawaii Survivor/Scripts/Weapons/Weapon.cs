using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IPlayerStatDependency
{
    [field: SerializeField] public WeaponDataSO WeaponData { get; private set; }
    [Header("Animation")]
    [SerializeField] protected float aimLerp;

    [Header("Settings")]
    [SerializeField] protected float weaponRange;
    [SerializeField] protected LayerMask enemyMask;

    [Header("Attack")]
    [SerializeField] protected int damage;

    [Header("Critical")]
    protected int criticalChance;
    protected float criticalPercent;

    [SerializeField] protected float attackDelay;
    protected float attackTimer;

    [Header("Level")]
    [field: SerializeField] public int Level { get; private set; }

    private void Start()
    {
    }

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
        if (Random.Range(0, 101) <= criticalChance)
        {
            isCriticalHit = true;
            Debug.Log("crit percent " + criticalPercent);
            return Mathf.RoundToInt(damage * criticalPercent);
        }

        isCriticalHit = false;
        return damage;
    }

    public abstract void UpdateStats(PlayerStatsManager playerStatsManager);

    /// <summary>
    /// Calculate a multiplier based on the weapon level
    /// </summary>
    protected void ConfigureStats()
    {
        // lv3 -> 2x stats, level 6 -> 3x stats
        float multiplier = 1 + (float)Level / 3;

        // Scale attack time and attack delay based on level
        damage = Mathf.RoundToInt(WeaponData.GetStatValue(Stat.Attack) * multiplier);
        attackDelay = 1f / (WeaponData.GetStatValue(Stat.AttackSpeed) * multiplier);

        // Scale crit damage and chance based on level
        criticalChance = Mathf.RoundToInt(WeaponData.GetStatValue(Stat.CriticalChance) * multiplier);
        criticalPercent = WeaponData.GetStatValue(Stat.CriticalPercent) * multiplier;

        // Scale range for RangeWeapon only
        if (WeaponData.prefab.GetType() == typeof(RangeWeapon))
        {
            weaponRange = WeaponData.GetStatValue(Stat.Range) * multiplier;
        }

    }
}
