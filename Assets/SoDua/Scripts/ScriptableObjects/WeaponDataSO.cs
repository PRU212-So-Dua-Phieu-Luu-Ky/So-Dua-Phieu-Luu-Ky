using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Weapon Data", menuName = "Scriptable Objects/New Weapon Data", order = 0)]
public class WeaponDataSO : ScriptableObject
{
    // ==============================
    // === Fields & Propss
    // ==============================

    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public int PurchasePrice { get; private set; }
    [field: SerializeField] public int RecyclePrice { get; private set; }

    [field: SerializeField] public Weapon Prefab { get; private set; }

    [SerializeField] private float attack;
    [SerializeField] private float attackSpeed;

    [SerializeField] private float criticalChance;
    [SerializeField] private float criticalPercent;

    [SerializeField] private float range;

    public Dictionary<Stat, float> BaseStats
    {
        get
        {
            return new Dictionary<Stat, float>()
            {
                {Stat.Attack, attack },
                {Stat.AttackSpeed, attackSpeed },
                {Stat.CriticalChance, criticalChance },
                {Stat.CriticalPercent, criticalPercent },
                {Stat.Range, range },
            };
        }
    }

    // ==============================
    // === Methods
    // ==============================

    public float GetStatValue(Stat stat)
    {
        foreach (KeyValuePair<Stat, float> pair in BaseStats)
        {
            if (pair.Key == stat) return pair.Value;
        }
        return 0;
    }
}
