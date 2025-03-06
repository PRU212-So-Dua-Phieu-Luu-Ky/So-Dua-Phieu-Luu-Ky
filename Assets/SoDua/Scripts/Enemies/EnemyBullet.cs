using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class EnemyBullet : MonoBehaviour
{
    [Header("Elements")]
    private Rigidbody2D rigidbody;

    private Collider2D collider;

    private RangeEnemyAttack rangeEnemyAttack;

    [Header("Settings")]
    private int damage;

    [SerializeField]
    private float moveSpeed;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();

        LeanTween.delayedCall(gameObject, 5, () => rangeEnemyAttack.ReleaseBullet(this));
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out Player player))
        {
            LeanTween.cancel(gameObject);
            player.TakeDamage(damage);
            this.collider.enabled = false;

            rangeEnemyAttack.ReleaseBullet(this);
        }
    }

    public void Shoot(int damage, Vector2 direction)
    {
        this.damage = damage;

        transform.right = direction;
        rigidbody.linearVelocity = direction * moveSpeed;
    }

    public void Configure(RangeEnemyAttack rangeEnemyAttack)
    {
        this.rangeEnemyAttack = rangeEnemyAttack;
    }

    public void Reload()
    {
        rigidbody.linearVelocity = Vector2.zero;
        collider.enabled = true;
    }
}