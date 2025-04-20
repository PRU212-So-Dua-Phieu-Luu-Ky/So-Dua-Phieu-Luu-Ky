using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameStateListener
{
    [Header("Player Components")]
    [SerializeField] private PlayerObjects playerObjects;
    [SerializeField] private PlayerWeapons playerWeapons;

    [Header("Inventory Elements")]
    [SerializeField] private Transform inventoryItemsParent;
    [SerializeField] private GameObject inventoryItemContainer;
    [SerializeField] private ShopManagerUI shopManagerUI;
    [SerializeField] private InventoryItemInfo inventoryItemInfo;

    void Awake()
    {
        ShopManager.onItemPurchased += ItemPurchasedCallback;
        WeaponMerger.onMerge += WeaponMergedCallback;
    }


    private void OnDestroy()
    {
        ShopManager.onItemPurchased -= ItemPurchasedCallback;
        WeaponMerger.onMerge -= WeaponMergedCallback;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameStateChangedCallBack(GameState gameState)
    {
        if (gameState == GameState.SHOP)
        {
            Configure();
        }
    }

    private void Configure()
    {
        inventoryItemsParent.Clear();

        Weapon[] weapons = playerWeapons.GetWeapons();

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] == null) continue;

            InventoryItemContainer container = Instantiate(inventoryItemContainer, inventoryItemsParent).GetComponent<InventoryItemContainer>();

            container.Configure(weapons[i], i, () => ShowItemInfo(container));
        }

        ObjectDataSO[] objectDatas = playerObjects.Objects.ToArray();

        for (int i = 0; i < objectDatas.Length; i++)
        {
            InventoryItemContainer container = Instantiate(inventoryItemContainer, inventoryItemsParent).GetComponent<InventoryItemContainer>();

            container.Configure(objectDatas[i], () => ShowItemInfo(container));
        }
    }

    private void ShowItemInfo(InventoryItemContainer container)
    {
        if (container.Weapon != null)
        {
            ShowWeaponInfo(container.Weapon, container.Index);
        }
        else
        {
            ShowObjectInfo(container.ObjectData);
        }
    }

    private void ShowWeaponInfo(Weapon weapon, int index)
    {
        inventoryItemInfo.Configure(weapon);

        inventoryItemInfo.RecycleButton.onClick.RemoveAllListeners();
        inventoryItemInfo.RecycleButton.onClick.AddListener(() => RecycleWeapon(index));

        shopManagerUI.ShowItemInfo();
    }


    private void ShowObjectInfo(ObjectDataSO objectData)
    {
        inventoryItemInfo.Configure(objectData);

        inventoryItemInfo.RecycleButton.onClick.RemoveAllListeners();
        inventoryItemInfo.RecycleButton.onClick.AddListener(() => RecycleObjects(objectData));
        shopManagerUI.ShowItemInfo();
    }

    private void RecycleObjects(ObjectDataSO objectData)
    {
        // Remove from player objects script
        playerObjects.RecycleObject(objectData);

        // Destroy the inventory item container
        Configure();

        // Close the item info 
        shopManagerUI.HideItemInfo();
    }
    private void RecycleWeapon(int index)
    {
        playerWeapons.RecycleWeapon(index);

        Configure();

        shopManagerUI.HideItemInfo();
    }
    private void ItemPurchasedCallback()
    {
        Configure();
    }

    private void WeaponMergedCallback(Weapon mergedWeapon)
    {
        Configure();
        inventoryItemInfo.Configure(mergedWeapon);
    }
}
