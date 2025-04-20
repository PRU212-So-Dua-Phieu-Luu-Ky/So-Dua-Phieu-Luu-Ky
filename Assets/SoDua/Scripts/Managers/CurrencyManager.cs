using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Kawaii_Survivor.Scripts.Managers
{
    public class CurrencyManager : MonoBehaviour
    {
        // ==============================
        // === Fields & Props
        // ==============================

        public static CurrencyManager instance;
        [field: SerializeField] public int Currency { get; private set; }
        [field: SerializeField] public int ChestCount { get; private set; }
        [SerializeField] private AudioSource audioSource;

        [Header("Actions")]
        public static Action onUpdated;

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
            Cash.onCollected += OnCoinCollectedCallback;
            Chest.onCollected += OnChestCollectedCallback;
            WaveTransitionManager.onChestDecrease += OnChestDecreaseCallback; 
        }

        private void OnDestroy()
        {
            Cash.onCollected -= OnCoinCollectedCallback;
            Chest.onCollected -= OnChestCollectedCallback;
            WaveTransitionManager.onChestDecrease -= OnChestDecreaseCallback; 
        }


        private void Start()
        {
            Currency = 0;
            ChestCount = 0;
            UpdateCurrencyText();
            UpdateChestCountText();
        }

        // ==============================
        // === Methods
        // ==============================

        [NaughtyAttributes.Button]
        public void Add500Currency()
        {
            AddCurrency(500);
        }

        public void AddCurrency(int amount)
        {
            Currency += amount;
            UpdateCurrencyText();

            onUpdated?.Invoke();
        }

        private void UpdateCurrencyText()
        {
            CurrencyText[] currencyTexts = FindObjectsByType<CurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach(var text in currencyTexts)
            {
                text.UpdateText(Currency.ToString());
            }
        }

        private void UpdateChestCountText()
        {
            ChestCountText[] currencyTexts = FindObjectsByType<ChestCountText>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach(var text in currencyTexts)
            {
                text.UpdateText(ChestCount.ToString());
            }

        }

        public bool HasEnoughCurrency(int rerollPrice)
        {
            return Currency >= rerollPrice;
        }

        public void UseCurrency(int rerollPrice)
        {
            AddCurrency(-rerollPrice);
        }

        private void OnCoinCollectedCallback(Cash cash)
        {
            Currency++;
            UpdateCurrencyText();
            audioSource.Play();
        }

        private void OnChestCollectedCallback()
        {
            ChestCount++;
            UpdateChestCountText();
        }

        private void OnChestDecreaseCallback()
        {
            ChestCount--;
            UpdateChestCountText();
        }
    }
}
