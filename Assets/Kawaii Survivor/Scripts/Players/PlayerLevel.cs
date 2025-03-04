using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevel : MonoBehaviour
{
    [Header(" Settings ")]
    private int requiredXp;
    private int currentXp;
    private int level;
    private int levelsEarnedThisWave; // track how many level earned at the current wave

    [Header(" Visuals ")]
    [SerializeField] private Slider xpBar;
    [SerializeField] private TextMeshProUGUI levelText;

    [Header(" DEBUG ")]
    [SerializeField] private bool DEBUG;

    private void Awake()
    {
        Candy.onCollected += CandyCollectedCallback;
    }

    private void CandyCollectedCallback(Candy candy)
    {
        currentXp++;
        Debug.Log(currentXp);
        if (currentXp >= requiredXp)
        {
            levelUp();
        }
        UpdateVisuals();
    }

    private void levelUp()
    {
        level++;
        levelsEarnedThisWave++;
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

    /// <summary>
    /// Tracking how many time to choose the upgradable item
    /// true: keep continuing choosing
    /// false: not choosing
    /// </summary>
    /// <returns></returns>
    public bool HasLeveledUp()
    {
        if (DEBUG)
            return true;

        if (levelsEarnedThisWave > 0)
        {
            levelsEarnedThisWave--;
            return true;
        }
        return false;
    }
}
