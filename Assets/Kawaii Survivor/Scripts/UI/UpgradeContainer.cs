using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeContainer : MonoBehaviour
{
    [Header(" Elements ")]
    [field: SerializeField] public Button Button { get; private set; }
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI upgradeNameText;
    [SerializeField] private TextMeshProUGUI upgradeValueText;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Configure(Sprite icon, string upgradeName, string  upgradeValue)
    {
        image.sprite = icon;
        upgradeNameText.text = upgradeName;
        upgradeValueText.text = upgradeValue;
    }
}
