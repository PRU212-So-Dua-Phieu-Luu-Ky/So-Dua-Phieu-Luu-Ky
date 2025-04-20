﻿using UnityEngine;
using UnityEngine.Pool;

public class DamageTextManager : MonoBehaviour
{
    // ==============================
    // === Fields & Props
    // ==============================

    [Header(" Elements ")]
    [SerializeField] private DamageText damageTextPrefab;

    [Header(" Pooling ")]
    private ObjectPool<DamageText> damageTextPool;

    // ==============================
    // === Lifecycles
    // ==============================

    private void Awake()
    {
        Enemy.onDamageTaken += EnemyHitCallback;
        PlayerHealth.onAttackedDodged += AttackDodgedCallback;
    }

    private void OnDestroy()
    {
        Enemy.onDamageTaken -= EnemyHitCallback;
        PlayerHealth.onAttackedDodged -= AttackDodgedCallback;
    }

    private void Start()
    {
        damageTextPool = new ObjectPool<DamageText>(Create, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }

    // ==============================
    // === Methods
    // ==============================
    private DamageText Create()
    {
        return Instantiate(damageTextPrefab, transform);
    }

    private void ActionOnGet(DamageText damageText)
    {
        damageText.gameObject.SetActive(true);
    }

    private void ActionOnRelease(DamageText damageText)
    {
        damageText.gameObject.SetActive(false);
    }

    private void ActionOnDestroy(DamageText damageText)
    {
        Destroy(damageText.gameObject);
    }

    private void EnemyHitCallback(int damage, Vector2 enemyPosition, bool isCriticalHit)
    {
        var damageTextInstance = damageTextPool.Get();

        Vector3 spawnPosition = enemyPosition + Vector2.up * 1.5f;
        damageTextInstance.transform.position = spawnPosition;

        damageTextInstance.Animate(damage.ToString(), isCriticalHit);

        if (damageTextInstance != null)
        {
            LeanTween.delayedCall(1, () => damageTextPool.Release(damageTextInstance));
        }
    }

    private void AttackDodgedCallback(Vector2 playerPositioon)
    {
        var damageTextInstance = damageTextPool.Get();

        Vector3 spawnPosition = playerPositioon + Vector2.up * 1.5f;
        damageTextInstance.transform.position = spawnPosition;

        damageTextInstance.Animate("Hụt", false);

        if (damageTextInstance != null)
        {
            LeanTween.delayedCall(1, () => damageTextPool.Release(damageTextInstance));
        }

    }
}