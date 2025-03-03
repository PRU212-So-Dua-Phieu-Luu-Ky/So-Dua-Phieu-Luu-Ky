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
}
