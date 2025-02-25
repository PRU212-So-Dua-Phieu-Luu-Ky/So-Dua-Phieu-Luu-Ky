using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerController : MonoBehaviour
{
    public static GameManagerController Instance { get; private set; }
    [SerializeField] private int targetFrameRate = 60;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        Application.targetFrameRate = targetFrameRate;
        SetGameState(GameState.MENU);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void WaveCompletedCallback()
    {
        if (Player.Instance.HasLeveledUp())
        {
            SetGameState(GameState.WAVE_TRANSITION);
        } else
        {
            SetGameState(GameState.SHOP);
        }
    }

    public void SetGameState(GameState gameState)
    {
        IEnumerable<IGameStateListener> gameStateListeners = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IGameStateListener>(); 

        foreach (IGameStateListener gameStateListener in gameStateListeners)
        {
            gameStateListener.GameStateChangedCallBack(gameState);
        }
    }

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