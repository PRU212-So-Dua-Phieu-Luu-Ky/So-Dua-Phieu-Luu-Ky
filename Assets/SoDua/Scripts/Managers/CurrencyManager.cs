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

        [Header("Actions")]
        public static Action onUpdated;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            UpdateTexts();
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
            UpdateTexts();

            onUpdated?.Invoke();
        }

        private void UpdateTexts()
        {
            CurrencyText[] currencyTexts = FindObjectsByType<CurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach(var text in currencyTexts)
            {
                text.UpdateText(Currency.ToString());
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
    }
}
