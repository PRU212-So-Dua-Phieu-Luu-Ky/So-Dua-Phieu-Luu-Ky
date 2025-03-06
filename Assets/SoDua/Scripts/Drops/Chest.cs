using System;
using UnityEngine;

public class Chest : MonoBehaviour, ICollectables
{
    [Header("Actions")]
    public static Action onCollected;
    public void Collect(Player player)
    {
        onCollected?.Invoke();
        Destroy(gameObject);
    }
}
