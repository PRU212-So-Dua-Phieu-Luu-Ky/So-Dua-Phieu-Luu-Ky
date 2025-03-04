using Assets.Kawaii_Survivor.Scripts.Managers;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestObjectContainer : MonoBehaviour
{
    // ==============================
    // === Fields & Props
    // ==============================

    [Header("Stats")]
    [SerializeField] private Transform statContainersParent;

    [Header("Elements")]
    [SerializeField] private Image weaponIcon;
    [SerializeField] private TextMeshProUGUI nameText;
    [field: SerializeField] public Button TakeButton { get; set; }
    [field: SerializeField] public Button RecycleButton { get; set; }

    [Header("Colors")]
    [SerializeField] private Image[] levelDependentImages;

    // ==============================
    // === Methods
    // ==============================

    public void Configure(ObjectDataSO objectData)
    {
        // Configure icon and name
        weaponIcon.sprite = objectData.Icon;
        nameText.text = objectData.Name;

        //Configure image. color based on level
        Color imageColor = ColorHolder.GetColor(objectData.Rarity);
        nameText.color = imageColor;

        foreach (Image image in levelDependentImages)
        {
            image.color = imageColor;
        }

        // Configure stats for Object item
        ConfigureStatContainers(objectData.BaseStats);
    }

    private void ConfigureStatContainers(Dictionary<Stat, float> stats)
    {
        StatContainerManager.GenerateStatContainers(stats, statContainersParent);
    }
}
