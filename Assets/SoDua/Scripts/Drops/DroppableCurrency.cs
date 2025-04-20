using System.Collections;
using UnityEngine;

public abstract class DroppableCurrency : MonoBehaviour, ICollectables
{
    private bool isCollected;

    private void OnEnable()
    {
        isCollected = false;
    }

    public void Collect(Player player)
    {
        if (isCollected) return;

        isCollected = true;

        StartCoroutine(MoveTowardsPlayer(player));
    }

    private IEnumerator MoveTowardsPlayer(Player player)
    {
        float timer = 0f;
        Vector2 initialPosition = transform.position;

        while (timer < 1f)
        {
            Vector2 targetPosition = player.GetCenter();
            transform.position = Vector2.Lerp(initialPosition, targetPosition, timer);
            timer += Time.deltaTime;

            yield return null;
        }

        Collected();
    }

    protected virtual void Collected()
    {
        gameObject.SetActive(false);
    }
}
