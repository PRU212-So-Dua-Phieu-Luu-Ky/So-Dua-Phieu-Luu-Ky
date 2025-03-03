using System;
using UnityEngine;

public class WeaponSelectionManager : MonoBehaviour, IGameStateListener
{
    // ==============================
    // === Fields & Props
    // ==============================

    [Header("Elements")]
    [SerializeField] private Transform weaponSelectionContainersParent;
    [SerializeField] private WeaponSelectionContainer weaponSelectionContainerPrefab;

    // ==============================
    // === Lifecycles
    // ==============================

    void Start()
    {

    }

    void Update()
    {

    }

    // ==============================
    // === Methods
    // ==============================

    public void GameStateChangedCallBack(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.WEAPON_SELECTION:
                Configure();
                break;
        }
    }

    private void Configure()
    {
        // Clear our parent, no children
        weaponSelectionContainersParent.Clear();

        // Generate weapon containers
        for (int i = 0; i < 3; i++)
        {
            GenerateSelectionWeaponContainer();
        }
    }

    private void GenerateSelectionWeaponContainer()
    {
        WeaponSelectionContainer containerInstance
            = Instantiate(weaponSelectionContainerPrefab, weaponSelectionContainersParent);
    }
}
