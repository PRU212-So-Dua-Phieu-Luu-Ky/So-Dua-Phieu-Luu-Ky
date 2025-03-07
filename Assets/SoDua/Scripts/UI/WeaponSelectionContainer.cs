using Assets.Kawaii_Survivor.Scripts.Managers;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    // Configure the upgradable container
    public void Configure(WeaponDataSO weaponData, int level)
    {
        // Configure icon and name
        weaponIcon.sprite = weaponData.Sprite;
        nameText.text = weaponData.Name + $"(Level {level + 1})";

        //Configure image. color based on level

        //Color imageColor = ColorHolder.GetColor(level);
        //nameText.color = Color.white;
        //foreach (Image image in levelDependentImages)
        //{
        //    image.color = imageColor;
        //}

        Dictionary<Stat, float> calculatedStats = WeaponStatsCalculator.GetStats(weaponData, level);
        ConfigureStatContainers(calculatedStats);
    }

    // Configure the weapon's stat container
    private void ConfigureStatContainers(Dictionary<Stat, float> calculatedStats)
    {
        StatContainerManager.GenerateStatContainers(calculatedStats, statContainersParent);
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
