using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WaveManagerUI))]
public class WaveManager : MonoBehaviour, IGameStateListener
{
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

    private void Awake()
    {
        waveManagerUI = GetComponent<WaveManagerUI>();
    }

    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isTimerOn)
        {
            return;
        }
        if (timer < waveDuration)
        {
            string timerString = ((int)(waveDuration - timer)).ToString();
            waveManagerUI.UpdateTimerText(timerString);
            ManageCurrentWave();
        }
        else
        {
            StartWaveTransition();
        }
    }

    private void StartWaveTransition()
    {
        isTimerOn = false;

        DefeatAllEnemies();

        currentWaveIndex++;

        if (currentWaveIndex >= Waves.Length)
        {
            GameManagerController.Instance.SetGameState(GameState.STAGE_COMPLETE);
        } else
        {
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

    private void ManageCurrentWave()
    {
        Wave currentWave = Waves[currentWaveIndex];

        for (int i = 0; i < currentWave.segments.Count; i++)
        {
            WaveSegment segment = currentWave.segments[i];

            //calculating the start and end time of the segment
            float tStart = segment.tStartEnd.x / 100 * waveDuration;
            float tEnd = segment.tStartEnd.y / 100 * waveDuration;

            if (timer < tStart || timer > tEnd)
                continue;

            float timeSinceSegmentStart = timer - tStart;

            float spawnDelay = 1f / segment.spawnFrequency;

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
        Vector2 direction = UnityEngine.Random.onUnitSphere;
        Vector2 offset = direction.normalized * UnityEngine.Random.Range(6, 10);
        Vector2 targetPosition = (Vector2)player.transform.position + offset;

        targetPosition.x = Mathf.Clamp(targetPosition.x, -27, 27);
        targetPosition.x = Mathf.Clamp(targetPosition.x, -18, 18);

        return targetPosition;
    }

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
    [MinMaxSlider(0, 100)] public Vector2 tStartEnd;

    public float spawnFrequency;
    public GameObject prefab;
}