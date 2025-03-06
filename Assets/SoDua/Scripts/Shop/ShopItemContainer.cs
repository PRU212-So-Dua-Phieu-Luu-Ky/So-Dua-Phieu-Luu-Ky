using Assets.Kawaii_Survivor.Scripts.Managers;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemContainer : MonoBehaviour
{
    // ==============================
    // === Fields & Props
    // ==============================


    [Header("Elements")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;

    [Header("Stats")]
    [SerializeField] public Button purchaseButton;
    [SerializeField] private Transform statContainersParent;
    //private WeaponDataSO weaponData;

    [Header("Colors")]
    [SerializeField] private Image[] levelDependentImages;

    [Header("Lock elements")]
    [SerializeField] private Image lockImage;
    [SerializeField] private Sprite lockedSprite, unlockedSprite;
    public bool IsLocked { get; private set; }

    [Header("Purchasing")]
    public WeaponDataSO WeaponData { get; private set; }
    public ObjectDataSO ObjectData { get; private set; }
    private int weaponLevel;

    [Header("Actions")]
    public static Action<ShopItemContainer, int> onPurchased;
    // ==============================
    // === Lifecycles
    // ==============================

    private void Awake()
    {
        CurrencyManager.onUpdated += CurrencyUpdatedCallback;
    }

    private void OnDestroy()
    {
        CurrencyManager.onUpdated -= CurrencyUpdatedCallback;
    }

    private void CurrencyUpdatedCallback()
    {
        int itemPrice;
        if (WeaponData != null)
        {
            itemPrice = WeaponStatsCalculator.GetPurchasePrice(WeaponData, weaponLevel);
        }
        else
        {
            itemPrice = ObjectData.Price;
        }

        purchaseButton.interactable = CurrencyManager.instance.HasEnoughCurrency(itemPrice);
    }

    // ==============================
    // === Methods
    // ==============================

    // Configure the upgradable container
    public void Configure(WeaponDataSO weaponData, int level)
    {
        weaponLevel = level;
        WeaponData = weaponData;
        // Configure icon and name
        icon.sprite = weaponData.Sprite;
        nameText.text = weaponData.Name + $"(Level {level + 1})";

        int weaponPrice = WeaponStatsCalculator.GetPurchasePrice(weaponData, level);

        priceText.text = weaponPrice.ToString();

        //Configure image. color based on level
        Color imageColor = ColorHolder.GetColor(level);
        nameText.color = Color.white;
        foreach (Image image in levelDependentImages)
        {
            image.color = imageColor;
        }

        Dictionary<Stat, float> calculatedStats = WeaponStatsCalculator.GetStats(weaponData, level);
        ConfigureStatContainers(calculatedStats);

        purchaseButton.onClick.AddListener(Purchase);
        purchaseButton.interactable = CurrencyManager.instance.HasEnoughCurrency(weaponPrice);
    }

    public void Configure(ObjectDataSO objectData)
    {
        ObjectData = objectData;
        // Configure icon and name
        icon.sprite = objectData.Icon;
        nameText.text = objectData.Name;
        priceText.text = objectData.Price.ToString();

        //Configure image. color based on level
        Color imageColor = ColorHolder.GetColor(objectData.Rarity);
        nameText.color = Color.white;
        foreach (Image image in levelDependentImages)
        {
            image.color = imageColor;
        }

        ConfigureStatContainers(objectData.BaseStats);

        purchaseButton.onClick.AddListener(Purchase);
        purchaseButton.interactable = CurrencyManager.instance.HasEnoughCurrency(objectData.Price);
    }

    private void ConfigureStatContainers(Dictionary<Stat, float> stats)
    {
        statContainersParent.Clear();
        StatContainerManager.GenerateStatContainers(stats, statContainersParent);
    }

    public void LockButtonCallback()
    {
        IsLocked = !IsLocked;
        UpdateLogVisuals();
    }

    private void UpdateLogVisuals()
    {
        lockImage.sprite = IsLocked ? lockedSprite : unlockedSprite;
    }

    private void Purchase()
    {
        onPurchased?.Invoke(this, weaponLevel);
    }

}
