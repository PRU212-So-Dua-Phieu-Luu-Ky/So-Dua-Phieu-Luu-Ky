using System.Collections.Generic;
using UnityEngine;

namespace Assets.Kawaii_Survivor.Scripts.Managers
{
    public class StatContainerManager : MonoBehaviour
    {

        public static StatContainerManager instance;

        [Header("Elements")]
        [SerializeField] private StatContainer statContainer;


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
        }

        private void GenerateContainers(Dictionary<Stat, float> statDictionary, Transform parent)
        {
            List<StatContainer> statContainers = new List<StatContainer>();

            foreach (KeyValuePair<Stat, float> kvp in statDictionary)
            {
                StatContainer containerInstance = Instantiate(statContainer, parent);
                statContainers.Add(containerInstance);

                Sprite statIcon = ResourcesManager.GetStatIcon(kvp.Key);
                string statName = Enums.FormatStatName(kvp.Key);
                float statValue = kvp.Value;

                containerInstance.Configure(statIcon, statName, statValue);
            }

            // Resize the text of stat'scontainers
            // Since we are resizing the UI element, we have to wait a bit 
            LeanTween.delayedCall(Time.deltaTime * 2, () => ResizeTexts(statContainers));
        }

        // Ensure the text are
        private void ResizeTexts(List<StatContainer> statContainers)
        {
            float minFontSize = 500;
            for (int i = 0; i < statContainers.Count; i++)
            {
                StatContainer statContainer = statContainers[i];
                float fontSize = statContainer.GetFontSize();

                if (fontSize < minFontSize)
                {
                    minFontSize = fontSize;
                }
            }

            // Set font size for all containers
            for (int i = 0; i < statContainers.Count; i++)
            {
                statContainers[i].SetFontSize(minFontSize);
            }
        }

        public static void GenerateStatContainers(Dictionary<Stat, float> statDictionary, Transform parent)
        {
            instance.GenerateContainers(statDictionary, parent);
        }
    }
}
