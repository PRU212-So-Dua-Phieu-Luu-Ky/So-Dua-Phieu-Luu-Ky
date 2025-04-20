using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IPlayerStatDependency
{
    [field: SerializeField] public WeaponDataSO WeaponData { get; private set; }
    [Header("Animation")]
    [SerializeField] protected float aimLerp;

    [Header("Settings")]
    protected float weaponRange;
    [SerializeField] protected LayerMask enemyMask;

    [Header("Attack")]
    protected int damage;

    [Header("Critical")]
    protected int criticalChance;
    protected float criticalPercent;

    protected float attackDelay;
    protected float attackTimer;

    [Header("Level")]
    public int Level { get; private set; }

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
        Dictionary<Stat, float> calculatedStats = WeaponStatsCalculator.GetStats(WeaponData, Level);

        // Scale attack time and attack delay based on level
        damage = Mathf.RoundToInt(calculatedStats[Stat.Attack]);
        attackDelay = 1f / calculatedStats[Stat.AttackSpeed];

        // Scale crit damage and chance based on level
        criticalChance = Mathf.RoundToInt(calculatedStats[Stat.CriticalChance]);
        criticalPercent = calculatedStats[Stat.CriticalPercent];
        weaponRange = calculatedStats[Stat.Range];
    }

    public void UpgradeTo(int targetLevel)
    {
        Level = targetLevel;

        ConfigureStats();
    }

    internal int GetRecyclePrice()
    {
        return WeaponStatsCalculator.GetRecyclePrice(WeaponData, Level);
    }

    internal void Upgrade()
    {
        UpgradeTo(Level + 1);
    }
}
