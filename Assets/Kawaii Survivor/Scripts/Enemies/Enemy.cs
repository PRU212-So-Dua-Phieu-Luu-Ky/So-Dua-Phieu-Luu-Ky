using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header(" Components ")]
    protected EnemyMovement movement;

    [Header(" Elements ")]
    protected Player player;

    [Header(" Spawn sequence related ")]
    [SerializeField] protected SpriteRenderer enemyRenderer;

    [SerializeField] protected SpriteRenderer spawnIndicator;
    [SerializeField] protected Collider2D collider;
    protected bool hasSpawned = false;

    [Header(" Attack ")]
    [SerializeField] protected float playerDetectionRadius = 0.5f;

    [Header("Health")]
    [SerializeField] protected int maxHealth;

    protected int health;

    [Header(" Effects ")]
    [SerializeField] protected ParticleSystem particleSystem;

    [Header(" Actions ")]
    public static Action<int, Vector2> onDamageTaken;

    [Header(" Debug ")]
    [SerializeField] protected bool showGizmos;

    protected virtual void Start()
    {
        health = maxHealth;
        movement = GetComponent<EnemyMovement>();
        player = FindFirstObjectByType<Player>();

        if (player == null)
        {
            Debug.LogWarning("No player found");
            Destroy(gameObject);
        }
        StartSpawnSequence();
    }

    protected bool CanAttack()
    {
        return enemyRenderer.enabled;
    }

    private void StartSpawnSequence()
    {
        SetRenderersVisibility(false);
        collider.enabled = false;
        Vector3 targetScale = spawnIndicator.transform.localScale * 1.2f;
        LeanTween.scale(spawnIndicator.gameObject, targetScale, .3f).setLoopPingPong(4).setOnComplete(OnSpawnSequenceCompleted);
    }

    private void SetRenderersVisibility(bool visibility)
    {
        enemyRenderer.enabled = visibility;
        spawnIndicator.enabled = !visibility;
    }

    private void OnSpawnSequenceCompleted()
    {
        movement.StorePlayer(player);
        SetRenderersVisibility(true);

        collider.enabled = true;

        hasSpawned = true;
    }

    public void TakeDamage(int damage)
    {
        int realDamage = Mathf.Min(damage, health);
        health -= damage;

        onDamageTaken?.Invoke(damage, transform.position);

        if (health <= 0)
        {
            PassAway();
        }
    }

    private void PassAway()
    {
        particleSystem.transform.SetParent(null);
        particleSystem.Play();

        Destroy(gameObject);
    }

}