using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Kawaii_Survivor.Scripts.Managers
{
    class ResourcesManager
    {
        const string statIconsDataPath = "Data/StatIconDataSO";

        private static StatIcon[] statIcons;

        public static Sprite GetStatIcon(Stat stat)
        {
            if (statIcons == null)
            {
                StatIconDataSO statIconDataSO = Resources.Load<StatIconDataSO>(statIconsDataPath);
                statIcons = statIconDataSO.StatIcons;
            }

            foreach (StatIcon statIcon in statIcons)
            {
                if (stat == statIcon.stat)
                {
                    return statIcon.icon;
                }
            }

            Debug.LogError("No icon found for stat : " + stat);
            return null;
        }
    }
}
