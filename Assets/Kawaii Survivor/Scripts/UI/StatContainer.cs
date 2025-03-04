using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatContainer : MonoBehaviour
{
    // ==============================
    // === Fields & Props
    // ==============================

    [Header("Elements")]
    [SerializeField] private Image statImage;
    [SerializeField] private TextMeshProUGUI statText;
    [SerializeField] private TextMeshProUGUI statValueText;

    // ==============================
    // === Methods
    // ==============================

    public void Configure(Sprite icon, string statName, string statValue)
    {
        statImage.sprite = icon;
        statText.text = statName;
        statValueText.text = statValue;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
