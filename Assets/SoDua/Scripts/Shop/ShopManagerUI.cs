using UnityEngine;

public class ShopManagerUI : MonoBehaviour
{
    [Header("Player Stats Elements")]
    [SerializeField] private GameObject playerStatsPanel;
    [SerializeField] private GameObject playerStatsClosePanel;

    [Header("Inventory Elements")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject inventoryClosePanel;

    [Header("Item Info Elements")]
    [SerializeField] private GameObject itemInfoPanel;
    [SerializeField] private GameObject itemInfoClosePanel;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowPlayerStats()
    {
        playerStatsPanel.SetActive(true);
        playerStatsClosePanel.SetActive(true);
    }

    public void HidePlayerStats()
    {
        playerStatsPanel.SetActive(false);
        playerStatsClosePanel.SetActive(false);
    }

    [NaughtyAttributes.Button]
    public void ShowInventory()
    {
        inventoryPanel.SetActive(true);
        inventoryClosePanel.SetActive(true);
    }

    [NaughtyAttributes.Button]
    public void HideInventory()
    {
        inventoryPanel.SetActive(false);
        inventoryClosePanel.SetActive(false);
        HideItemInfo();
    }

    [NaughtyAttributes.Button]
    public void ShowItemInfo()
    {
        if (inventoryPanel.activeSelf)
        {
            itemInfoPanel.SetActive(true);
            itemInfoClosePanel.SetActive(true);
        }
    }

    [NaughtyAttributes.Button]
    public void HideItemInfo()
    {
        itemInfoPanel.SetActive(false);
        itemInfoClosePanel.SetActive(false);
    }
}
