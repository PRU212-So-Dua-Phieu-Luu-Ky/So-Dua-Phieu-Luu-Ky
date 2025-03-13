using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatDescriptionContainer : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image statIcon;
    [SerializeField] private TextMeshProUGUI statNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    /// <summary>
    /// Configure the container with stat information
    /// </summary>
    /// <param name="icon">The stat's icon</param>
    /// <param name="statName">The stat's name</param>
    /// <param name="description">The stat's description</param>
    public void Configure(Sprite icon, string statName, string description)
    {
        // Set the icon
        if (statIcon != null)
        {
            statIcon.sprite = icon;
            statIcon.gameObject.SetActive(icon != null);
        }

        // Set the stat name
        if (statNameText != null)
        {
            statNameText.text = statName;
        }

        // Set the description
        if (descriptionText != null)
        {
            descriptionText.text = description;
        }
    }
}