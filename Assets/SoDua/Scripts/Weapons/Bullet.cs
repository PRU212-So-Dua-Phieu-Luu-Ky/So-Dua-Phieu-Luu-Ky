using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    [Header("Elements")]
    private Rigidbody2D rigidbody;

    private Collider2D collider;
    private RangeWeapon rangeWeapon;

    [Header("Settings")]
    private int damage;

    [SerializeField] private float moveSpeed;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private LayerMask playerMask;
    private Enemy target;
    private bool isCriticalHit;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        //LeanTween.delayedCall(gameObject, 5, () => rangeEnemyAttack.ReleaseBullet(this));
    }
    // Update is called once per frame
    private void Update()
    {
    }

    public void Shoot(int damage, Vector2 direction, bool isCriticalHit)
    {
        Invoke(nameof(Release), 1);
        this.damage = damage;
        this.isCriticalHit = isCriticalHit;

        //transform.right = direction;
        rigidbody.linearVelocity = direction * moveSpeed;
    }

    private void Release()
    {
        if (!gameObject.activeSelf) return;
        rangeWeapon.ReleaseBullet(this);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // if target is already found 
        if (target != null) return;

        if (IsInLayerMask(collider.gameObject.layer, enemyMask))
        {
            target = collider.GetComponent<Enemy>();
            CancelInvoke();
            Attack(target);
            //this.collider.enabled = false;
            Release();
        } else if (!IsInLayerMask(collider.gameObject.layer, playerMask))
        {
            CancelInvoke();
            Release();
        } 
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
        
    //}

    private void Attack(Enemy enemy)
    {
        enemy.TakeDamage(damage, isCriticalHit);
    }

    private bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        return (layerMask.value & (1 << layer)) != 0;
    }

    public void Reload()
    {
        target = null;
        rigidbody.linearVelocity = Vector2.zero;
        collider.enabled = true;
    }

    public void Configure(RangeWeapon rangeWeapon)
    {
        this.rangeWeapon = rangeWeapon;
    }
}