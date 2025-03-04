using UnityEngine;

[RequireComponent(typeof(PlayerHealth), typeof(PlayerLevel))]
public class Player : MonoBehaviour
{
    // ==============================
    // === Props & Fields
    // ==============================

    public static Player Instance { get; private set; }

    [Header(" Components ")]
    private PlayerHealth playerHealth;
    private PlayerLevel playerLevel;
    [SerializeField] private CircleCollider2D collider;

    // ==============================
    // === Constructors
    // ==============================

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        playerHealth = GetComponent<PlayerHealth>();
        playerLevel = GetComponent<PlayerLevel>();
    }

    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    // ==============================
    // === Methods
    // ==============================

    public void TakeDamage(int damage)
    {
        playerHealth.TakeDamage(damage);
    }

    public Vector2 GetCenter()
    {
        return (Vector2)transform.position + collider.offset;
    }

    public bool HasLeveledUp()
    {
        return playerLevel.HasLeveledUp();
    }
}