using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour, IGameStateListener
{
    // ==============================
    // === Fields & Props
    // ==============================

    public static UIManager Instance { get; private set; }

    [Header(" Panels ")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject waveTransitionPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject stageCompletePanel;
    [SerializeField] private GameObject weaponSelectionPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject tutorialPanel;
    private List<GameObject> panels = new List<GameObject>();

    private void Awake()
    {
        panels.AddRange(new GameObject[]
        {
            menuPanel,
            gamePanel,
            waveTransitionPanel,
            shopPanel,
            stageCompletePanel,
            gameOverPanel,
            weaponSelectionPanel,
            tutorialPanel
        });
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // ==============================
    // === Constructors
    // ==============================

    private void Start()
    {
        HidePausePanel();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    // ==============================
    // === Methods
    // ==============================

    public void GameStateChangedCallBack(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.MENU:
                ShowPanel(menuPanel);
                break;

            case GameState.STAGE_COMPLETE:
                ShowPanel(stageCompletePanel);
                break;

            case GameState.WEAPON_SELECTION:
                ShowPanel(weaponSelectionPanel);
                break;

            case GameState.GAME_OVER:
                ShowPanel(gameOverPanel);
                break;

            case GameState.GAME:
                ShowPanel(gamePanel);
                break;

            case GameState.WAVE_TRANSITION:
                ShowPanel(waveTransitionPanel);
                break;

            case GameState.SHOP:
                ShowPanel(shopPanel);
                break;
            case GameState.TUTORIAL:
                ShowPanel(tutorialPanel);
                break;
        }
    }

    /// <summary>
    /// Enable/Disable all game objects of the panels
    /// </summary>
    /// <param name="panel"></param>
    private void ShowPanel(GameObject panel)
    {
        foreach (GameObject gameObject in panels)
        {
            gameObject.SetActive(gameObject == panel);
        }
    }

    public void ShowPausePanel()
    {
        pausePanel.SetActive(true);
    }

    public void HidePausePanel()
    {
        pausePanel.SetActive(false);
    }

}