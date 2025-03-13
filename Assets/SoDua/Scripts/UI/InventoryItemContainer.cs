using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image container;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Button button;

    public int Index { get; private set; }
    public Weapon Weapon { get; private set; }
    public ObjectDataSO ObjectData { get; private set; }


    public void Configure(Color containerColor, Sprite itemIcon)
    {
        container.color = containerColor;
        this.itemIcon.sprite = itemIcon;
    }

    public void Configure(Weapon weapon, int index, Action clickedCallback)
    {
        Weapon = weapon;
        Index = index;

        container.color = ColorHolder.GetColor(weapon.Level);
        itemIcon.sprite = weapon.WeaponData.Sprite;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => clickedCallback?.Invoke());
    }

    public void Configure(ObjectDataSO objectData, Action clickedCallback)
    {
        ObjectData = objectData;
        container.color = ColorHolder.GetColor(objectData.Rarity);
        itemIcon.sprite = objectData.Icon;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => clickedCallback?.Invoke());
    }

}
