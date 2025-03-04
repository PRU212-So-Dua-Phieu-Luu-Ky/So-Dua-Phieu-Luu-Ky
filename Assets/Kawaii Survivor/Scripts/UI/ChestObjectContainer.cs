using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestObjectContainer : MonoBehaviour
{
    // ==============================
    // === Fields & Props
    // ==============================

    [Header("Elements")]
    [SerializeField] private Image weaponIcon;
    [SerializeField] private TextMeshProUGUI nameText;
    [field: SerializeField] public Button TakeButton { get; set; }
    [field: SerializeField] public Button RecycleButton { get; set; }

    [Header("Colors")]
    [SerializeField] private Image[] levelDependentImages;

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

    public void Configure(ObjectDataSO objectData)
    {
        // Configure icon and name
        weaponIcon.sprite = objectData.Icon;
        nameText.text = name;

        //Configure image. color based on level
        Color imageColor = ColorHolder.GetColor(objectData.Rarity);

        foreach (Image image in levelDependentImages)
        {
            image.color = imageColor;
        }
    }
}
