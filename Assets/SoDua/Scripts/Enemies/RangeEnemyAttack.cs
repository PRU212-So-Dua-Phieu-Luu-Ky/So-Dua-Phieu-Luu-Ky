using UnityEngine;
using UnityEngine.Pool;

public class RangeEnemyAttack : MonoBehaviour
{
    [Header(" Elements ")]
    private Player player;

    [SerializeField] private Transform shootingPoint;
    [SerializeField] private EnemyBullet bulletPrefab;

    [Header(" Settings ")]
    [SerializeField] private int damage;

    [SerializeField] private float attackFrequency;
    private float attackDelay;
    private float attackTimer;

    [Header(" Pooling ")]
    private ObjectPool<EnemyBullet> bulletPool;

    private void Start()
    {
        attackDelay += 1f / attackFrequency;
        attackTimer = attackDelay;

        bulletPool = new ObjectPool<EnemyBullet>(Create, ActionOnGet, ActionOnRelease, ActionOnDestroy, defaultCapacity: 4);
    }

    private void ActionOnDestroy(EnemyBullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    private EnemyBullet Create()
    {
        var bullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity, shootingPoint);
        bullet.Configure(this);
        return bullet;
    }

    private void ActionOnRelease(EnemyBullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void ActionOnGet(EnemyBullet bullet)
    {
        bullet.Reload();
        bullet.transform.position = shootingPoint.position;

        bullet.gameObject.SetActive(true);
    }

    public void AutoAim()
    {
        ManageShooting();
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
        Vector2 direction = (player.GetCenter() - (Vector2)shootingPoint.position).normalized;

        var bullet = bulletPool.Get();
        bullet.Shoot(damage, direction);
    }

    public void StorePlayer(Player player)
    {
        this.player = player;
    }

    public void ReleaseBullet(EnemyBullet bullet)
    {
        bulletPool.Release(bullet);
    }
}