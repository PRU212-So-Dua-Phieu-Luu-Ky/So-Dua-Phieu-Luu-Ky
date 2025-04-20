using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Change the game state
/// </summary>
public class GameManagerController : MonoBehaviour
{
    // ==============================
    // === Fields & Props
    // ==============================

    public static GameManagerController Instance { get; private set; }
    [SerializeField] private int targetFrameRate = 60;

    private GameState previousState;

    // ==============================
    // === Lifecycle
    // ==============================

    private void Awake()
    {
        // Make sure only 1 instance of game manager for the entire game lifecycle
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        Application.targetFrameRate = targetFrameRate;
        SetGameState(GameState.TUTORIAL);
    }

    // ==============================
    // === Methods
    // ==============================

    /// <summary>
    /// Clicking this function to show SHOP or MENU of Upgradable Stats
    /// </summary>
    public void WaveCompletedCallback()
    {
        if (Player.Instance.HasLeveledUp() || WaveTransitionManager.instance.HasCollectedChest())
        {
            SetGameState(GameState.WAVE_TRANSITION);
        }
        else
        {
            SetGameState(GameState.SHOP);
        }
    }

    /// <summary>
    /// Any game objec that implement this interface will react to game state changes
    /// </summary>
    /// <param name="gameState"></param>
    public void SetGameState(GameState gameState)
    {
        // Store the previous state if we're not pausing or unpausing
        if (gameState != GameState.PAUSE)
        {
            previousState = gameState;
        }

        // Finding all class implementing the interface IGameStateListener
        IEnumerable<IGameStateListener> gameStateListeners = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IGameStateListener>();

        foreach (IGameStateListener gameStateListener in gameStateListeners)
        {
            gameStateListener.GameStateChangedCallBack(gameState);
        }
    }


    /// <summary>
    /// Pause the game, showing pause UI and stopping time
    /// </summary>
    public void PauseGame()
    {

        Time.timeScale = 0f; // Stop time
        SetGameState(GameState.PAUSE);

        // Show pause panel through UIManager
        UIManager.Instance.ShowPausePanel();
    }

    /// <summary>
    /// Resume the game from a paused state
    /// </summary>
    public void ResumeGame()
    {
        Time.timeScale = 1f; // Restore normal time flow

        // Return to previous game state
        SetGameState(previousState);

        // Hide pause panel through UIManager
        UIManager.Instance.HidePausePanel();
    }

    /// <summary>
    /// Load the scene from scratch when click the button
    /// </summary>
    public void ManageGameOver()
    {
        SceneManager.LoadScene(0);
    }

    public void StartGame() => SetGameState(GameState.GAME);
    public void StartShop() => SetGameState(GameState.SHOP);
    public void StartWeaponSelection() => SetGameState(GameState.WEAPON_SELECTION);
}

public interface IGameStateListener
{
    void GameStateChangedCallBack(GameState gameState);
}