using UnityEngine;

public class ColorHolder : MonoBehaviour
{
    // ==============================
    // === Fields & Props
    // ==============================

    [Header(" Elements ")]
    [SerializeField] private PaletteSO palette;

    public static ColorHolder instance { get; private set; }

    // ==============================
    // === Constructors
    // ==============================

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

    void Start()
    {

    }

    void Update()
    {

    }

    // ==============================
    // === Methods
    // ==============================

    public static Color GetColor(int level)
    {
        // Get color in range of 0 and length of color palette
        level = Mathf.Clamp(level, 0, instance.palette.LevelColors.Length - 1);
        return instance.palette.LevelColors[level];
    }


    public static Color GetOutlineColor(int level)
    {
        // Get color in range of 0 and length of color palette
        level = Mathf.Clamp(level, 0, instance.palette.LevelOutlineColors.Length - 1);
        return instance.palette.LevelOutlineColors[level];
    }
}
