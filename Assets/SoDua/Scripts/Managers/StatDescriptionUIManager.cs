using System;
using System.Collections.Generic;
using Assets.Kawaii_Survivor.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatDescriptionUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button[] questionMarkButtons;
    [SerializeField] private GameObject statDescriptionPanel;
    [SerializeField] private Transform statContainersParent;
    [SerializeField] private StatDescriptionContainer statDescriptionPrefab;

    [Header("Panel Settings")]
    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] private LeanTweenType easeType = LeanTweenType.easeOutQuad;

    private List<StatDescriptionContainer> statContainers = new List<StatDescriptionContainer>();
    private bool isPanelVisible = false;
    private RectTransform panelRectTransform;

    private void Awake()
    {
        panelRectTransform = statDescriptionPanel.GetComponent<RectTransform>();

        statDescriptionPanel.SetActive(false);

        foreach (Button button in questionMarkButtons)
        {
            button.onClick.AddListener(ToggleStatDescriptionPanel);
        }

        GenerateStatDescriptions();
    }

    private void GenerateStatDescriptions()
    {
        foreach (Transform child in statContainersParent)
        {
            Destroy(child.gameObject);
        }
        statContainers.Clear();

        foreach (Stat stat in Enum.GetValues(typeof(Stat)))
        {
            StatDescriptionContainer container = Instantiate(statDescriptionPrefab, statContainersParent);
            statContainers.Add(container);

            Sprite statIcon = ResourcesManager.GetStatIcon(stat);

            string statName = Enums.FormatStatName(stat);

            // Configure the container
            container.Configure(statIcon, statName, GetStatDescription(stat));
        }
    }

    private string GetStatDescription(Stat stat)
    {
        // Return descriptions for each stat - you can customize these
        switch (stat)
        {
            case Stat.Attack:
                return "Tăng sát thương cơ bản của bạn.";
            case Stat.AttackSpeed:
                return "Giảm thời gian giữa các đòn tấn công.";
            case Stat.CriticalChance:
                return "Cơ hội gây sát thương chí mạng với đòn tấn công của bạn.";
            case Stat.CriticalPercent:
                return "Tăng hệ số nhân sát thương cho đòn đánh chí mạng.";
            case Stat.MoveSpeed:
                return "Tăng tốc độ di chuyển của bạn.";
            case Stat.MaxHealth:
                return "Tăng lượng máu tối đa của bạn.";
            case Stat.Range:
                return "Tăng tầm đánh của bạn.";
            case Stat.HealthRecoverySpeed:
                return "Tăng tốc độ hồi máu.";
            case Stat.Armor:
                return "Giảm sát thương nhận vào từ kẻ địch.";
            case Stat.Luck:
                return "Tăng cơ hội tìm thấy vật phẩm hiếm.";
            case Stat.Dodge:
                return "Cơ hội né tránh hoàn toàn đòn tấn công của kẻ địch.";
            case Stat.LifeSteal:
                return "Phần trăm sát thương gây ra được chuyển thành máu.";
            default:
                return "Không có mô tả.";
        }
    }

    private void ToggleStatDescriptionPanel()
    {
        isPanelVisible = !isPanelVisible;

        if (isPanelVisible)
        {
            ShowPanel();
        }
        else
        {
            HidePanel();
        }
    }

    private void ShowPanel()
    {
        // Enable the panel
        statDescriptionPanel.SetActive(true);

        // Animate the panel in
        LeanTween.cancel(statDescriptionPanel);

        // Scale animation from 0 to 1
        panelRectTransform.localScale = Vector3.zero;
        LeanTween.scale(panelRectTransform, Vector3.one, animationDuration)
            .setEase(easeType);

        // Fade in animation
        CanvasGroup canvasGroup = statDescriptionPanel.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            LeanTween.alphaCanvas(canvasGroup, 1f, animationDuration)
                .setEase(easeType);
        }
    }

    private void HidePanel()
    {
        // Animate the panel out
        LeanTween.cancel(statDescriptionPanel);

        // Scale animation from 1 to 0
        LeanTween.scale(panelRectTransform, Vector3.zero, animationDuration)
            .setEase(easeType)
            .setOnComplete(() =>
            {
                statDescriptionPanel.SetActive(false);
            });

        // Fade out animation
        CanvasGroup canvasGroup = statDescriptionPanel.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            LeanTween.alphaCanvas(canvasGroup, 0f, animationDuration)
                .setEase(easeType);
        }
    }

    // For use with UI events
    public void OnQuestionMarkButtonClicked()
    {
        ToggleStatDescriptionPanel();
    }
}