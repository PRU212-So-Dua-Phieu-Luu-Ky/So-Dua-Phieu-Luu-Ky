using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WaveManagerUI))]
public class WaveManager : MonoBehaviour, IGameStateListener
{
    // ==============================
    // === Fields & Props
    // ==============================

    [Header(" Elements ")]
    [SerializeField] private Player player;
    private WaveManagerUI waveManagerUI;

    [Header(" Settings ")]
    [SerializeField] private float waveDuration;

    private float timer;
    private bool isTimerOn;
    private int currentWaveIndex = 0;

    [Header(" Waves ")]
    [SerializeField] private Wave[] Waves;
    private List<float> localCounters = new List<float>();

    // ==============================
    // === Lifecycles
    // ==============================
    private void Awake()
    {
        waveManagerUI = GetComponent<WaveManagerUI>();
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (!isTimerOn)
        {
            return;
        }
        if (timer < waveDuration)
        {
            // Execute the current wave
            string timerString = ((int)(waveDuration - timer)).ToString();
            waveManagerUI.UpdateTimerText(timerString);
            ManageCurrentWave();
        }
        else
        {
            // Start transition to the next wave
            StartWaveTransition();
        }
    }

    // ==============================
    // === Methods
    // ==============================

    /// <summary>
    /// Function calling the next wave
    /// </summary>
    private void StartWaveTransition()
    {
        isTimerOn = false;

        // Timeout then clear all enemies
        DefeatAllEnemies();

        currentWaveIndex++;

        if (currentWaveIndex >= Waves.Length)
        {
            // Enable the game state to complete
            GameManagerController.Instance.SetGameState(GameState.STAGE_COMPLETE);
        }
        else
        {
            // If wave complete, move level and open shop
            GameManagerController.Instance.WaveCompletedCallback();
        }
    }

    private void DefeatAllEnemies()
    {
        transform.Clear();
    }

    private void StartWave(int waveIndex)
    {
        waveManagerUI.UpdateWaveText("Wave " + (currentWaveIndex + 1) + " / " + Waves.Length);

        localCounters.Clear();
        foreach (WaveSegment segment in Waves[waveIndex].segments)
        {
            localCounters.Add(1);
        }
        isTimerOn = true;
        timer = 0;
    }

    /// <summary>
    /// manage each segment in the current wave
    /// </summary>
    private void ManageCurrentWave()
    {
        Wave currentWave = Waves[currentWaveIndex];

        for (int i = 0; i < currentWave.segments.Count; i++)
        {
            WaveSegment segment = currentWave.segments[i];

            //calculating the start and end time of the segment
            float tStart = segment.tStartEnd.x / 100 * waveDuration;
            float tEnd = segment.tStartEnd.y / 100 * waveDuration;

            // If timer goes beyong the start and then, moving to the next segments
            if (timer < tStart || timer > tEnd)
                continue;

            // Convert frequency into delay
            // if frequency = 2, => 2 enemies pers second
            // then delay is 1 / 2 => 1 enemies per 0.5 second
            float spawnDelay = 1f / segment.spawnFrequency;

            float timeSinceSegmentStart = timer - tStart;
            if (localCounters[i] < (timeSinceSegmentStart / spawnDelay))
            {
                Instantiate(segment.prefab, GetSpawnPosition(), Quaternion.identity, transform);
                localCounters[i]++;
            }
        }

        timer += Time.deltaTime;
    }

    private Vector2 GetSpawnPosition()
    {
        // Spawn enemy at the sphere of the player
        Vector2 direction = UnityEngine.Random.onUnitSphere;
        Vector2 offset = direction.normalized * UnityEngine.Random.Range(6f, 10f);
        Vector2 targetPosition = (Vector2)player.transform.position + offset;

        targetPosition.x = Mathf.Clamp(targetPosition.x, -27, 27);
        targetPosition.y = Mathf.Clamp(targetPosition.x, -18, 18);

        return targetPosition;
    }

    /// <summary>
    /// Wave Manager react to the game state
    /// - Game: StartNextWave()
    /// - Wave Transition: do nothing
    /// - Game over: turn off timer and delete all enemies game objects
    /// </summary>
    /// <param name="gameState"></param>
    public void GameStateChangedCallBack(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GAME:
                StartNextWave();
                break;
            case GameState.WAVE_TRANSITION:
                break;
            case GameState.GAME_OVER:
                isTimerOn = false;
                DefeatAllEnemies();
                break;
        }
    }

    private void StartNextWave()
    {
        StartWave(currentWaveIndex);
    }
}

// ==============================
// === Wave structs
// ==============================

[System.Serializable]
public struct Wave
{
    public string name;
    public List<WaveSegment> segments;
}

[System.Serializable]
public struct WaveSegment
{
    // the duration of each segment is equal to a percentage of the wave duration
    // Using MinMax slider from Naughty Attribute
    [MinMaxSlider(0, 100)] public Vector2 tStartEnd;

    public float spawnFrequency;
    public GameObject prefab;
}