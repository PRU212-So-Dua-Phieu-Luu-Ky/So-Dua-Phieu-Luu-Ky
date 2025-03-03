using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectionContainer : MonoBehaviour
{
    // ==============================
    // === Fields & Props
    // ==============================

    [Header(" Element ")]
    [SerializeField] private Image weaponIcon;
    [SerializeField] private TextMeshProUGUI nameText;
    [field: SerializeField] public Button Button { get; set; }

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

    public void Configure(Sprite spirte, string name)
    {
        weaponIcon.sprite = spirte;
        nameText.text = name;
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
