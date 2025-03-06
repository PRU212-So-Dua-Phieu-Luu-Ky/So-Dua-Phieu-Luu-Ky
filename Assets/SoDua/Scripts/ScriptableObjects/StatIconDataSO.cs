using UnityEngine;

[CreateAssetMenu(fileName = "StatIconDataSO", menuName = "Scriptable Objects/StatIconDataSO")]
public class StatIconDataSO : ScriptableObject
{
    [field: SerializeField] public StatIcon[] StatIcons { get; private set; }
}

[System.Serializable]
public struct StatIcon
{
    public Stat stat;
    public Sprite icon;
}
