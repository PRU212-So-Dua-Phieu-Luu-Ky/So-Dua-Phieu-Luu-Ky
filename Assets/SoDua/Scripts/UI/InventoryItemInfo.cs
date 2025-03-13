using System.Collections.Generic;
using Assets.Kawaii_Survivor.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemInfo : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI recyclePriceText;

    [Header("Colors")]
    [SerializeField] private Image container;

    [Header("Stats")]
    [SerializeField] private Transform statsParent;

    [Header("Buttons")]
    [field: SerializeField] public Button MergeButton { get; private set; }
    [field: SerializeField] public Button RecycleButton { get; private set; }

    public void Configure(Weapon weapon)
    {
        Configure(
            weapon.WeaponData.Sprite,
            weapon.WeaponData.Name + " (Lv." + (weapon.Level + 1) + ")",
            ColorHolder.GetColor(weapon.Level),
            WeaponStatsCalculator.GetRecyclePrice(weapon.WeaponData, weapon.Level),
            WeaponStatsCalculator.GetStats(weapon.WeaponData, weapon.Level)
        );

        MergeButton.gameObject.SetActive(true);
        MergeButton.interactable = WeaponMerger.instance.CanMerge(weapon);

        MergeButton.onClick.RemoveAllListeners();
        MergeButton.onClick.AddListener(() => WeaponMerger.instance.Merge());

        RecycleButton.gameObject.SetActive(true);
    }

    public void Configure(ObjectDataSO objectData)
    {
        Configure(
            objectData.Icon,
            objectData.Name,
            ColorHolder.GetColor(objectData.Rarity),
            objectData.RecyclePrice,
            objectData.BaseStats
        );
        MergeButton.gameObject.SetActive(false);
        RecycleButton.gameObject.SetActive(true);
    }
    private void Configure(Sprite itemIcon, string name, Color containerColor, int recyclePrice, Dictionary<Stat, float> stats)
    {
        icon.sprite = itemIcon;
        itemNameText.text = name;
        // container.color = containerColor;

        recyclePriceText.text = recyclePrice.ToString();

        StatContainerManager.GenerateStatContainers(stats, statsParent);
    }
}
