using System;
using UnityEngine;
using UnityEngine.Pool;

public class DropManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Candy candyPrefab;

    [SerializeField] private Cash cashPrefab;
    [SerializeField] private Chest chestPrefab;

    [Header("Settings")]
    [SerializeField][Range(0, 100)] private int cashDropChance;

    [SerializeField][Range(0, 100)] private int chestDropChance;

    [Header("Pooling")]
    private ObjectPool<Candy> candyPool;

    private ObjectPool<Cash> cashPool;

    private void Awake()
    {
        Enemy.onPassedAway += EnemyPassedAwayCallback;
        Candy.onCollected += ReleaseCandy;
        Cash.onCollected += ReleaseCash;
    }

    private void OnDestroy()
    {
        Enemy.onPassedAway -= EnemyPassedAwayCallback;
        Candy.onCollected -= ReleaseCandy;
        Cash.onCollected -= ReleaseCash;
    }

    private void Start()
    {
        candyPool = new ObjectPool<Candy>(CandyCreate, CandyActionOnGet, CandyActionOnRelease, CandyActionOnDestroy, defaultCapacity: 4);
        cashPool = new ObjectPool<Cash>(CashCreate, CashActionOnGet, CashActionOnRelease, CashActionOnDestroy, defaultCapacity: 4);
    }

    private Cash CashCreate()
    {
        var cash = Instantiate(cashPrefab, transform);
        //bullet.Configure(this);
        return cash;
    }

    private void CashActionOnGet(Cash cash) => cash.gameObject.SetActive(true);

    private void CashActionOnRelease(Cash cash) => cash.gameObject.SetActive(false);

    private void CashActionOnDestroy(Cash cash) => Destroy(cash.gameObject);

    private Candy CandyCreate()
    {
        var candy = Instantiate(candyPrefab, transform);
        //bullet.Configure(this);
        return candy;
    }

    private void CandyActionOnGet(Candy candy) => candy.gameObject.SetActive(true);

    private void CandyActionOnRelease(Candy candy) => candy.gameObject.SetActive(false);

    private void CandyActionOnDestroy(Candy candy) => Destroy(candy.gameObject);

    // Update is called once per frame
    private void Update()
    {
    }

    private void EnemyPassedAwayCallback(Vector2 enemyPosition)
    {
        bool shouldSpawnCash = UnityEngine.Random.Range(0, 101) <= cashDropChance;

        GameObject cashDrop;
        if (shouldSpawnCash)
        {
            cashDrop = cashPool.Get().gameObject;
            cashDrop.transform.position = enemyPosition;
        }
        var candyDrop = candyPool.Get().gameObject;

        candyDrop.transform.position = enemyPosition;

        TryDropChest(enemyPosition);
    }

    private void TryDropChest(Vector2 spawnPosition)
    {
        bool shouldSpawnChest = UnityEngine.Random.Range(0, 101) <= chestDropChance;

        if (!shouldSpawnChest) return;

        Instantiate(chestPrefab, spawnPosition, Quaternion.identity, transform);
    }

    private void ReleaseCandy(Candy candy) => candyPool.Release(candy);

    private void ReleaseCash(Cash cash) => cashPool.Release(cash);
}