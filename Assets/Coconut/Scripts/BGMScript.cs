using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMScript : MonoBehaviour
{
    public static BGMScript instance;
    [SerializeField] private string[] gameScenes;
    
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

    private void Update()
    {
        StopMusic();
    }

    public void StopMusic()
    {
        if (gameScenes.Contains(SceneManager.GetActiveScene().name))
        {
            Destroy(gameObject);
        }
    }
}
