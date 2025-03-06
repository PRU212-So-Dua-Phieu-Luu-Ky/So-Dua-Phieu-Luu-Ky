using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform targetPlayer;

    [Header("Settings")]
    [SerializeField] private Vector2 minMaxXY;

    private void Start()
    {
        //minMaxXY.x =
    }

    private void LateUpdate()
    {
        if (targetPlayer == null)
        {
            Debug.LogWarning("No Player has been specified");

            return;
        }
        var cameraPosition = targetPlayer.position;

        cameraPosition.z = -10;

        cameraPosition.x = Mathf.Clamp(cameraPosition.x, -minMaxXY.x, minMaxXY.x);
        cameraPosition.y = Mathf.Clamp(cameraPosition.y, -minMaxXY.y, minMaxXY.y);

        transform.position = cameraPosition;
    }
}