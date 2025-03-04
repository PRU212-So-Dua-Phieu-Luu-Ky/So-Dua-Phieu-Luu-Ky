using UnityEngine;

namespace Assets.Kawaii_Survivor.Scripts.Managers
{
    class ResourcesManager
    {
        // ==============================
        // === Fields & Props
        // ==============================

        const string statIconsDataPath = "Data/StatIconDataSO";
        const string objectDatasPath = "Data/Objects/";
        private static StatIcon[] statIcons;

        private static ObjectDataSO[] objectDatas;
        public static ObjectDataSO[] Objects
        {
            get
            {
                if (objectDatas == null)
                    return Resources.LoadAll<ObjectDataSO>(objectDatasPath);

                return objectDatas;
            }
            private set { }
        }

        // ==============================
        // === Methods
        // ==============================

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
