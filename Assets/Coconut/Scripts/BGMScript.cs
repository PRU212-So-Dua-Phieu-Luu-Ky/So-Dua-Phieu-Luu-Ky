using UnityEngine;

public class BGMScript : MonoBehaviour
{
    private static BGMScript instance;
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
