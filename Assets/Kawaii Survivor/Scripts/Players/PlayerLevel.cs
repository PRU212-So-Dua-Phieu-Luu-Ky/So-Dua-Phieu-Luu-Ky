using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevel : MonoBehaviour
{
    [Header(" Settings ")]
    private int requiredXp;
    private int currentXp;
    private int level;

    [Header(" Visuals ")]
    [SerializeField] private Slider xpBar;
    [SerializeField] private TextMeshProUGUI levelText;

    private void Awake()
    {
        Candy.onCollected += CandyCollectedCallback; 
    }

    private void CandyCollectedCallback(Candy candy)
    {
        currentXp++;
        if (currentXp >= requiredXp)
        {
            levelUp();
        }
        UpdateVisuals();
    }

    private void levelUp()
    {
        level++;
        currentXp = 0;
        UpdateRequiredXp();
    }

    void Start()
    {
        UpdateRequiredXp(); 
        UpdateVisuals(); 
    }

    private void UpdateVisuals()
    {
        xpBar.value = (float)currentXp / requiredXp;
        levelText.text = "lvl " + (level + 1);
    }

    private void UpdateRequiredXp()
    {
        requiredXp = (level + 1) * 5;
    }
}
