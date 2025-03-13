using System;
using System.Collections.Generic;
using Assets.Kawaii_Survivor.Scripts.Managers;
using UnityEngine;

[RequireComponent(typeof(PlayerStatsManager))]
public class PlayerObjects : MonoBehaviour
{
    // ==============================
    // === Fields & Props
    // ==============================

    [field: SerializeField] public List<ObjectDataSO> Objects { get; private set; }
    private PlayerStatsManager playerStatsManager;


    private void Awake()
    {
        playerStatsManager = GetComponent<PlayerStatsManager>();
    }

    void Start()
    {
        foreach (ObjectDataSO objectData in Objects)
        {
            playerStatsManager.AddObject(objectData.BaseStats);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddObject(ObjectDataSO objectToTake)
    {
        Objects.Add(objectToTake);
        playerStatsManager.AddObject(objectToTake.BaseStats);
    }

    public void RecycleObject(ObjectDataSO objectToRecycle)
    {
        //Remove object from object list
        Objects.Remove(objectToRecycle);

        //Get $ back from manager 
        CurrencyManager.instance.AddCurrency(objectToRecycle.RecyclePrice);

        //Remove object stats from player stats manager
        playerStatsManager.RemoveObjectStats(objectToRecycle.BaseStats);
    }
}
