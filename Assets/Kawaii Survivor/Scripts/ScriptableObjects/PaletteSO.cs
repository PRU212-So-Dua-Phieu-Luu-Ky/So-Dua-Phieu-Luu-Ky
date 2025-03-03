using UnityEngine;

[CreateAssetMenu(fileName = "PaletteSO", menuName = "Scriptable Objects/PaletteSO", order = 0)]
public class PaletteSO : ScriptableObject
{
    [field: SerializeField] public Color[] LevelColors { get; private set; }
    [field: SerializeField] public Color[] LevelOutlineColors { get; private set; }
}
