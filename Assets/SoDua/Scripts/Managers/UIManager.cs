using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour, IGameStateListener
{
    // ==============================
    // === Fields & Props
    // ==============================

    [Header(" Panels ")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject waveTransitionPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject stageCompletePanel;
    [SerializeField] private GameObject weaponSelectionPanel;
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
            weaponSelectionPanel
        });
    }

    // ==============================
    // === Constructors
    // ==============================

    private void Start()
    {
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
}