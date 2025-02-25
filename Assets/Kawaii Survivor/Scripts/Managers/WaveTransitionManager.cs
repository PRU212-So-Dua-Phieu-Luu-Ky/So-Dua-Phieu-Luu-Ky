using NaughtyAttributes;
using NUnit.Framework.Internal;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WaveTransitionManager : MonoBehaviour, IGameStateListener
{
    [Header(" Elemenents ")]
    [SerializeField] private Button[] upgradeContainers;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStateChangedCallBack(GameState gameState)
    {
        switch(gameState)
        {
            case GameState.WAVE_TRANSITION:
                ConfigureUpgradeContainers();
                break;

        }

    }

    [Button]
    private void ConfigureUpgradeContainers()
    {
        for (int i = 0; i < upgradeContainers.Length; i++)
        {

            int randomIndex = Random.Range(0, Enum.GetValues(typeof(Stat)).Length);
            var stat = (Stat) Enum.GetValues(typeof(Stat)).GetValue(randomIndex);

            string randomStatString = Enums.FormatStatName(stat);

            upgradeContainers[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = randomStatString;

            upgradeContainers[i].onClick.RemoveAllListeners();
        }
    }
}
