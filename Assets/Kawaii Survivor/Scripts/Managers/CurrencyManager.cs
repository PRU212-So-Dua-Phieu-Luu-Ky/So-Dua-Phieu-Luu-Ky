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

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
            {
                Destroy(gameObject);
            }
        }

        // ==============================
        // === Methods
        // ==============================

        public void AddCurrency(int amount)
        {
            Currency += amount;
        }
    }
}
