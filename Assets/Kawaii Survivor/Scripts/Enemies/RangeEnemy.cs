using System;
using TMPro;
using UnityEngine;

public class RangeEnemy : MonoBehaviour
{
    [Header(" Components ")]
    private EnemyMovement movement;

    [Header(" Elements ")]
    private Player player;

    [Header(" Spawn sequence related ")]
    [SerializeField] private SpriteRenderer enemyRenderer;

    [SerializeField] private SpriteRenderer spawnIndicator;
    [SerializeField] private Collider2D collider;
    private bool hasSpawned = false;

    [Header(" Attack ")]
    [SerializeField] private int damage;

    [SerializeField] private int attackFrequency;
    [SerializeField] private float playerDetectionRadius = 0.5f;
    private float attackDelay;
    private float attackTimer;

    [Header("Health")]
    [SerializeField] private int maxHealth;

    private int health;

    [Header(" Effects ")]
    [SerializeField] private ParticleSystem particleSystem;

    [Header(" Actions ")]
    public static Action<int, Vector2> onDamageTaken;


    [Header(" Debug ")]
    [SerializeField] private bool showGizmos;
    private void Start()
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
        attackDelay = 1f / attackFrequency;
    }

    private void StartSpawnSequence()
    {
        SetRenderersVisibility(false);
        collider.enabled = false;
        Vector3 targetScale = spawnIndicator.transform.localScale * 1.2f;
        LeanTween.scale(spawnIndicator.gameObject, targetScale, .3f).setLoopPingPong(4).setOnComplete(OnSpawnSequenceCompleted);
    }

    private void OnSpawnSequenceCompleted()
    {
        movement.StorePlayer(player);
        SetRenderersVisibility(true);

        collider.enabled = true;

        hasSpawned = true;
    }

    private void TryAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= playerDetectionRadius)
            Attack();
        else movement.FollowPlayer();
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

    private void Attack()
    {
        attackTimer = 0f;
        Debug.Log("Player taking damage");
    }

    private void Wait()
    {
        attackTimer += Time.deltaTime;
    }

    private void PassAway()
    {
        particleSystem.transform.SetParent(null);
        particleSystem.Play();

        Destroy(gameObject);
    }

    private void SetRenderersVisibility(bool visibility)
    {
        enemyRenderer.enabled = visibility;
        spawnIndicator.enabled = !visibility;
    }

    private void Update()
    {
        if (attackTimer >= attackDelay)
            TryAttack();
        else Wait();
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }
}