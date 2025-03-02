using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    // ==============================
    // === Fields & Props
    // ==============================

    [Header("Data")]
    [SerializeField] private CharacterDataSO playerData;

    [Header("Settings")]
    private Dictionary<Stat, float> addends = new Dictionary<Stat, float>();     // extra stat modifications
    private Dictionary<Stat, float> playerStat = new Dictionary<Stat, float>();  // current player stats

    // ==============================
    // === Lifecycles
    // ==============================

    private void Awake()
    {
        playerStat = playerData.BaseStats;
        foreach (KeyValuePair<Stat, float> kvp in playerStat)
        {
            addends[kvp.Key] = 0;
        }
    }

    void Start()
    {
        UpdatePlayerStats();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Add stats into the addends
    /// </summary>
    /// <param name="stat"></param>
    /// <param name="value"></param>
    public void AddPlayerStat(Stat stat, float value)
    {
        // Add extra stats into addends
        if (addends.ContainsKey(stat))
        {
            addends[stat] += value;
        }
        else
        {
            Debug.LogError($"The key {stat} has not been found");
        }

        // Update the playe stats after adding extra
        UpdatePlayerStats();
    }

    public float GetStatValue(Stat stat)
    {
        float value = playerStat[stat] + addends[stat];
        return value;
    }

    /// <summary>
    /// Find all scripts that implement IPlayerStatDependency
    /// </summary>
    private void UpdatePlayerStats()
    {
        IEnumerable<IPlayerStatDependency> playerStatDependencies
            = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IPlayerStatDependency>();

        foreach (IPlayerStatDependency dependency in playerStatDependencies)
        {
            dependency.UpdateStats(this);
        }
    }
}

