using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerHealth : MonoBehaviour, IPlayerStatDependency
{
    // ==============================
    // === Fields & Props
    // ==============================

    [Header("Settings")]
    [SerializeField] private int baseMaxHealth;

    private float maxHealth;

    private float health;
    private float armor;
    private float lifesteal;
    private float dodge;

    private float healthRecoverySpeed;
    private float healthRecoveryTimer;
    private float healthRecoveryDuration;

    [Header("Elements")]
    [SerializeField] private Slider healthSlider;

    [SerializeField]
    private TextMeshProUGUI healthText;

    [Header("Actions")]
    public static Action<Vector2> onAttackedDodged;

    // ==============================
    // === Lifecycles
    // ==============================

    private void Awake()
    {
        Enemy.onDamageTaken += EnemyTookDamageCallback;
    }

    private void OnDestroy()
    {
        Enemy.onDamageTaken -= EnemyTookDamageCallback;
    }

    // Update is called once per frame
    private void Update()
    {
        if (health < maxHealth)
            RecoverHealth();
    }
    private void Start()
    {
        //health = maxHealth;

        //healthSlider.value = 1;

        //UpdateUI();
    }

    // ==============================
    // === Methods
    // ==============================

    private void EnemyTookDamageCallback(int damage, Vector2 enemyPos, bool isCriticalHit)
    {
        if (health >= baseMaxHealth) return;
        float lifeStealValue = damage * lifesteal;
        float healthToAdd = Mathf.Min(lifeStealValue, maxHealth - health);

        health += healthToAdd;
        UpdateUI();
    }

    /// <summary>
    /// 
    /// </summary>
    private void RecoverHealth()
    {
        healthRecoveryTimer += Time.deltaTime;
        if (healthRecoveryTimer >= healthRecoveryDuration)
        {
            healthRecoveryTimer = 0;
            float healthToAdd = Mathf.Min(.1f, maxHealth - health);
            health += healthToAdd;

            UpdateUI();
        }
    }

    /// <summary>
    /// Taking Damage + dodge, if health <=0 then GAME_OVER
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        if (ShouldDodge())
        {
            onAttackedDodged?.Invoke(transform.position);
            return;
        }
        float realDamage = damage * Mathf.Clamp(1 - (armor / 1000), 0, 10000);
        realDamage = Mathf.Min(realDamage, health);
        health -= realDamage;

        UpdateUI();

        if (health <= 0)
        {
            PassAway();
        }
    }

    private bool ShouldDodge()
    {
        return Random.Range(0f, 100f) < Mathf.Clamp(dodge, 0, 99.99f);
    }

    private void UpdateUI()
    {
        healthSlider.value = health / maxHealth;
        healthText.text = (int)health + " / " + maxHealth;
    }

    /// <summary>
    /// Showing UI game over when player health = 0
    /// </summary>
    private void PassAway()
    {
        GameManagerController.Instance.SetGameState(GameState.GAME_OVER);
    }

    /// <summary>
    /// Update stats triggered by the PlayerStatsManager
    /// </summary>
    /// <param name="playerStatsManager"></param>
    public void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        float addedHealth = playerStatsManager.GetStatValue(Stat.MaxHealth);
        maxHealth = baseMaxHealth + (int)addedHealth;
        maxHealth = Mathf.Max(maxHealth, 1);

        health = maxHealth;
        UpdateUI();

        armor = playerStatsManager.GetStatValue(Stat.Armor);
        lifesteal = playerStatsManager.GetStatValue(Stat.LifeSteal) / 100;
        dodge = playerStatsManager.GetStatValue(Stat.Dodge);

        healthRecoverySpeed = Mathf.Max(.001f, playerStatsManager.GetStatValue(Stat.HealthRecoverySpeed));
        healthRecoveryDuration = 1f / healthRecoverySpeed;
    }
}