using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform target;

    [Header("Settings")]
    [SerializeField] private Vector2 minMaxXY;
    
    void Start()
    {
        
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("No target has been specifided");

            return;
        }
        var cameraPosition = target.position;

        cameraPosition.z = -10;

        cameraPosition.x = Mathf.Clamp(cameraPosition.x, -minMaxXY.x, minMaxXY.x);
        cameraPosition.y = Mathf.Clamp(cameraPosition.y, -minMaxXY.y, minMaxXY.y);

        transform.position = cameraPosition;
    }
}
