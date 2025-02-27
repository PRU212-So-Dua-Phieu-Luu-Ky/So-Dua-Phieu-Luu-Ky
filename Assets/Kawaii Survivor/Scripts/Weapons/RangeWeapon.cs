using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class RangeWeapon : Weapon
{
    [Header(" Elements ")]
    [SerializeField] private Bullet bulletPrefab;

    [SerializeField] private Transform shootingPoint;

    [Header(" Pooling ")]
    private ObjectPool<Bullet> bulletPool;

    private void Start()
    {
        bulletPool = new ObjectPool<Bullet>(Create, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }

    private Bullet Create()
    {
        var bullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity, shootingPoint);
        bullet.Configure(this);
        return bullet;
    }

    private void ActionOnGet(Bullet Bullet)
    {
        Bullet.Reload();
        Bullet.transform.position = shootingPoint.position;

        Bullet.gameObject.SetActive(true);
    }

    private void ActionOnRelease(Bullet Bullet)
    {
        Bullet.gameObject.SetActive(false);
    }

    private void ActionOnDestroy(Bullet Bullet)
    {
        Destroy(Bullet.gameObject);
    }

    private void Update()
    {
        AimAtClosestEnemy();
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
            targetUpVector = (closestEnemy.GetCenter() - (Vector2) transform.position).normalized;
            transform.up = targetUpVector;
            ManageShooting();
            return;
        }

        SmoothAim(targetUpVector);
    }

    private void ManageShooting()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackDelay)
        {
            attackTimer = 0;
            Shoot();
        }
    }

    private void Shoot()
    {
        int damage = GetDamage(out bool isCriticalHit);

        var bullet = bulletPool.Get();
        bullet.Shoot(damage, transform.up, isCriticalHit);
    }

    public void ReleaseBullet(Bullet bullet)
    {
        bulletPool.Release(bullet);
    }

    public override void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        ConfigureStats();

        damage =(int)(damage * (1 + playerStatsManager.GetStatValue(Stat.Attack) / 100));
        attackDelay /= 1 + (playerStatsManager.GetStatValue(Stat.AttackSpeed) / 100);

        criticalChance = Mathf.RoundToInt(criticalChance * (1 + playerStatsManager.GetStatValue(Stat.CriticalChance) / 100));
        criticalPercent += playerStatsManager.GetStatValue(Stat.CriticalPercent);

        weaponRange += playerStatsManager.GetStatValue(Stat.Range) / 10;


    }
}