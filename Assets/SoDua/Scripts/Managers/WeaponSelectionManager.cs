using System.Collections.Generic;
using UnityEngine;

public class WeaponSelectionManager : MonoBehaviour, IGameStateListener
{
    // ==============================
    // === Fields & Props
    // ==============================

    [Header("Elements")]
    [SerializeField] private Transform weaponSelectionContainersParent;
    [SerializeField] private WeaponSelectionContainer weaponSelectionContainerPrefab;
    [SerializeField] private PlayerWeapons playerWeapons;

    [Header(" Data ")]
    [SerializeField] private WeaponDataSO[] weaponDatas;

    //private HashSet<Data> existingWeapons = new HashSet<Data>();
    private HashSet<WeaponDataSO> existingWeapons = new();
    private WeaponDataSO selectedWeapon;
    private int initialWeaponLevel;

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
            case GameState.GAME:

                // dont add twice in the game
                if (selectedWeapon == null) return;
                playerWeapons.TryAddWeapon(selectedWeapon, initialWeaponLevel);
                selectedWeapon = null;
                initialWeaponLevel = 0;

                break;

            case GameState.WEAPON_SELECTION:
                Configure();
                break;
        }
    }

    [NaughtyAttributes.Button]
    private void Configure()
    {
        // Clear our parent, no children
        weaponSelectionContainersParent.Clear();

        // Generate weapon containers
        for (int i = 0; i < 3 && existingWeapons.Count < 3; i++)
        {
            GenerateSelectionWeaponContainer();
        }
    }

    private void GenerateSelectionWeaponContainer()
    {
        var weaponLength = weaponDatas.Length;
        if (existingWeapons.Count == weaponLength)
        {
            existingWeapons.Clear();
        }
        WeaponSelectionContainer weaponSelectionContainerInstance
            = Instantiate(weaponSelectionContainerPrefab, weaponSelectionContainersParent);


        // Get random weapon data injected from the inspector
        WeaponDataSO weaponData;
        bool isAdded = false;
        int level = 0;
        do
        {
            //weaponData = weaponDatas[UnityEngine.Random.Range(0, weaponDatas.Length)];
            //level = Random.Range(0, 4);
            //var data = new Data(weaponData, weaponLength);
            //isAdded = existingWeapons.Add(data);
            weaponData = weaponDatas[UnityEngine.Random.Range(0, weaponDatas.Length)];
            isAdded = existingWeapons.Add(weaponData);
        } while (!isAdded);

        if (!isAdded)
        {
            return;
        }
        // Create instance based on weapon data so
        weaponSelectionContainerInstance.Configure(weaponData, level);

        // Remove listeners and add the listener
        weaponSelectionContainerInstance.Button.onClick.RemoveAllListeners();
        weaponSelectionContainerInstance.Button.onClick.AddListener(() => WeaponSelectedCallback(weaponSelectionContainerInstance, weaponData, level));
    }

    private void WeaponSelectedCallback(WeaponSelectionContainer weaponSelectionContainerInstance, WeaponDataSO weaponData, int level)
    {
        // Choose the selected weapon, generate a random level
        selectedWeapon = weaponData;
        initialWeaponLevel = level;

        //If current selection instance is matching with the children of the container
        foreach (WeaponSelectionContainer container in weaponSelectionContainersParent.GetComponentsInChildren<WeaponSelectionContainer>())
        {
            if (container == weaponSelectionContainerInstance)
            {
                container.Select();
            }
            else
            {
                container.Deselect();
            }
        }
    }

    struct Data
    {
        WeaponDataSO weaponDataSO;
        int level;

        public Data(WeaponDataSO weaponDataSO, int level)
        {
            this.weaponDataSO = weaponDataSO;
            this.level = level;
        }
    }
}
