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
        int level = Random.Range(0, 4);
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
}
