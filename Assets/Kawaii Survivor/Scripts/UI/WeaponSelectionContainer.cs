using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine;
using System.Collections.Generic;

public class WeaponSelectionContainer : MonoBehaviour
{
    // ==============================
    // === Fields & Props
    // ==============================

    [Header("Elements")]
    [SerializeField] private Image weaponIcon;
    [SerializeField] private TextMeshProUGUI nameText;
    [field: SerializeField] public Button Button { get; set; }

    [Header("Stats")]
    [SerializeField] private Transform statContainersParent;
    [SerializeField] private StatContainer statContainersPrefab;
    [SerializeField] private Sprite statIcon;
    private WeaponDataSO weaponData;

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

    public void Configure(Sprite spirte, string name, int level, WeaponDataSO weaponData)
    {
        // Configure icon and name
        weaponIcon.sprite = spirte;
        nameText.text = name + $"(Level {level + 1})";

        //Configure image. color based on level
        Color imageColor = ColorHolder.GetColor(level);
        foreach (Image image in levelDependentImages)
        {
            image.color = imageColor;
        }

        ConfigureStatContainers(weaponData);
    }

    private void ConfigureStatContainers(WeaponDataSO weaponData)
    {
        foreach (KeyValuePair<Stat, float> kvp in weaponData.BaseStats)
        {
            StatContainer containerInstance = Instantiate(statContainersPrefab, statContainersParent);
            containerInstance.Configure(statIcon, Enums.FormatStatName(kvp.Key), kvp.Value.ToString());
        }
    }

    // Scale down the current game object
    public void Deselect()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one, .3f);
    }

    // Scale up the current game object
    public void Select()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one * 1.075f, .3f).setEase(LeanTweenType.easeInOutBounce);
    }
}
