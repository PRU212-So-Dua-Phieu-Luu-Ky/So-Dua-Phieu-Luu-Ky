using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private CharacterDataSO playerData;

    [Header("Settings")]
    private Dictionary<Stat, float> addends = new Dictionary<Stat, float>(); 
    private Dictionary<Stat, float> playerStat = new Dictionary<Stat, float>();

    private void Awake()
    {
        playerStat = playerData.BaseStats;
        foreach (KeyValuePair<Stat, float> kvp  in playerStat)
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

    public void AddPlayerStat(Stat stat, float value)
    {
        if (addends.ContainsKey(stat))
        {
            addends[stat] += value;
        } else
        {
            Debug.LogError($"The key {stat} has not been found");
        }

        UpdatePlayerStats();
    }

    public float GetStatValue(Stat stat)
    {
        float value = playerStat[stat] + addends[stat]; 
        return value;
    }

    private void UpdatePlayerStats()
    {
        IEnumerable<IPlayerStatDependency> playerStatsDependencies = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IPlayerStatDependency>();

        foreach (IPlayerStatDependency dependency in playerStatsDependencies)
        {
            dependency.UpdateStats(this);
        }
    }
}

