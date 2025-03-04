using Assets.Kawaii_Survivor.Scripts.Managers;
using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WaveTransitionManager : MonoBehaviour, IGameStateListener
{
    public static WaveTransitionManager instance { get; private set; }

    [Header("Player")]
    [SerializeField] private PlayerObjects playerObjects;

    [Header(" Elements ")]
    [SerializeField] private UpgradeContainer[] upgradeContainers;
    [SerializeField] private GameObject upgradeContainersParent;
    [SerializeField] private PlayerStatsManager playerStatsManager;

    [Header(" Chest Management ")]
    [SerializeField] private ChestObjectContainer chestContainerPrefab;
    [SerializeField] private Transform chestContainerParent;
    //private ChestObjectContainer containerInstance;
    private int chestCollected;

    // ==============================
    // === Lifecycles
    // ==============================

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Chest.onCollected += ChestCollectedCallback;
    }

    private void OnDestroy()
    {
        Chest.onCollected -= ChestCollectedCallback;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // ==============================
    // === Methods
    // ==============================

    /// <summary>
    /// Ch
    /// </summary>
    private void ChestCollectedCallback()
    {
        chestCollected++;
    }

    public void GameStateChangedCallBack(GameState gameState)
    {
        // State trnasition to the next wave
        switch (gameState)
        {
            case GameState.WAVE_TRANSITION:
                TryOpenChest();
                break;

        }
    }

    private void TryOpenChest()
    {
        // clear the open chest
        chestContainerParent.Clear();

        if (chestCollected > 0)
        {
            ShowObject();
        }
        else
        {
            ConfigureUpgradeContainers();
        }
    }

    private void ShowObject()
    {
        // Count how many chest do we get
        chestCollected--;

        upgradeContainersParent.SetActive(false);

        // Get a list of object datas from the Resource Manager
        ObjectDataSO[] objectDatas = ResourcesManager.Objects;
        ObjectDataSO objectData = objectDatas[Random.Range(0, objectDatas.Length)];

        var containerInstance = Instantiate(chestContainerPrefab, chestContainerParent);
        containerInstance.Configure(objectData);
        containerInstance.TakeButton.onClick.AddListener(() => TakeButtonCallback(objectData));
        containerInstance.RecycleButton.onClick.AddListener(() => RecycleButtonCallback(objectData));
    }

    private void TakeButtonCallback(ObjectDataSO objectToTake)
    {
        Debug.Log("CLCDCWD");
        playerObjects.AddObject(objectToTake);  // Add stats of chest intothe current player
        TryOpenChest();                         // Open the next chest
    }

    private void RecycleButtonCallback(ObjectDataSO objectToRecycle)
    {
        CurrencyManager.instance.AddCurrency(objectToRecycle.RecyclePrice);
        TryOpenChest();
    }


    /// <summary>
    /// Set random stats for upgrable buttons
    /// </summary>
    [Button]
    private void ConfigureUpgradeContainers()
    {
        upgradeContainersParent.SetActive(true);

        for (int i = 0; i < upgradeContainers.Length; i++)
        {
            int randomIndex = Random.Range(0, Enum.GetValues(typeof(Stat)).Length);  // get random index
            var stat = (Stat)Enum.GetValues(typeof(Stat)).GetValue(randomIndex);     // get random stats on list
            string randomStatString = Enums.FormatStatName(stat);                    // format the stat name

            string buttonString;
            Action action = GenerateActionToPerform(stat, out buttonString);              // generate the value 

            upgradeContainers[i].Configure(null, randomStatString, buttonString);
            upgradeContainers[i].Button.onClick.RemoveAllListeners();
            upgradeContainers[i].Button.onClick.AddListener(() => action?.Invoke());
            upgradeContainers[i].Button.onClick.AddListener(() => GameManagerController.Instance.WaveCompletedCallback());
        }
    }


    public bool HasCollectedChest()
    {
        return chestCollected > 0;
    }

    /// <summary>
    /// Assign the random value and string for stats
    /// </summary>
    /// <param name="stat"></param>
    /// <param name="buttonString"></param>
    /// <returns></returns>
    private Action GenerateActionToPerform(Stat stat, out string buttonString)
    {
        buttonString = string.Empty;
        float value;

        switch (stat)
        {
            case Stat.Attack:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;
            case Stat.AttackSpeed:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;
            case Stat.Range:
                value = Random.Range(1f, 5f);
                buttonString = "+" + value.ToString("F2") + "%"; // 2 decimal
                break;
            case Stat.MoveSpeed:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;
            case Stat.CriticalChance:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;
            case Stat.CriticalPercent:
                value = Random.Range(1f, 2f);
                buttonString = "+" + value.ToString("F2") + "%"; // 2 decimal
                break;
            case Stat.MaxHealth:
                value = Random.Range(1, 5);
                buttonString = "+" + value.ToString();
                break;
            case Stat.HealthRecoverySpeed:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;
            case Stat.Armor:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;
            case Stat.Luck:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;
            case Stat.Dodge:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;
            case Stat.LifeSteal:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;
            default:
                return () => Debug.Log("Invalid stat");

        }

        // Create the action that add stat's value into the player
        return () => playerStatsManager.AddPlayerStat(stat, value);
    }
}
