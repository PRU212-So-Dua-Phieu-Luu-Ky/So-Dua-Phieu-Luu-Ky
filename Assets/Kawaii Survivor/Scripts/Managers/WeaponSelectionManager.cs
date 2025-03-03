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

    [Header(" Data ")]
    [SerializeField] private WeaponDataSO[] weaponDatas;

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
        WeaponSelectionContainer weaponSelectionContainerInstance
            = Instantiate(weaponSelectionContainerPrefab, weaponSelectionContainersParent);

        // Get random weapon data injected from the inspector
        WeaponDataSO weaponData = weaponDatas[UnityEngine.Random.Range(0, weaponDatas.Length)];

        // Create instance based on weapon data so
        weaponSelectionContainerInstance.Configure(weaponData.Sprite, weaponData.Name);

        // Remove listeners and add the listener
        weaponSelectionContainerInstance.Button.onClick.RemoveAllListeners();
        weaponSelectionContainerInstance.Button.onClick.AddListener(() => WeaponSelectedCallback(weaponSelectionContainerInstance, weaponData));
    }

    private void WeaponSelectedCallback(WeaponSelectionContainer weaponSelectionContainerInstance, WeaponDataSO weaponData)
    {
        Debug.Log("Hello World");
    }
}
