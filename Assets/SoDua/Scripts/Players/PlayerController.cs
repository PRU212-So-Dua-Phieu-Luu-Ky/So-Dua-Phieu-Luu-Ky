using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, IGameStateListener, IPlayerStatDependency
{
    [Header("Elements")]
    [SerializeField] private MobileJoystick mobileJoystick;

    private Rigidbody2D rigidbody2D;

    [SerializeField] private SpriteRenderer playerSprite;

    [Header("Settings")]
    private float moveSpeed = 1f;
    [SerializeField] private float baseMoveSpeed = 1f;

    private PlayerInputSystem inputActions;

    private Vector3 moveVector;
    private Vector2 moveDirection;
    private float moveDistance;
    private bool canMove;

    // Boundary settings
    [SerializeField] private float minX = -30.4f;
    [SerializeField] private float maxX = 21f;
    [SerializeField] private float minY = -11f;
    [SerializeField] private float maxY = 17.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        inputActions = new PlayerInputSystem();
        inputActions.Player.Enable();
        rigidbody2D = GetComponent<Rigidbody2D>();

        // If playerSprite is not set in the inspector, try to find it
        if (playerSprite == null)
        {
            playerSprite = GetComponentInChildren<SpriteRenderer>();
            if (playerSprite == null)
            {
                Debug.LogWarning("Player sprite renderer not found! The flip functionality will not work.");
            }
        }
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }


    private void OnDestroy()
    {
        if (inputActions == null) return;
        inputActions.Disable();
        inputActions.Dispose();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    /// <summary>
    /// Only alow First State as GAME to move character, other is false
    /// </summary>
    /// <param name="gameState"></param>
    public void GameStateChangedCallBack(GameState gameState)
    {
        if (gameState != GameState.GAME) canMove = false;
        else canMove = true;
    }

    private void HandleMovement()
    {
        if (canMove)
        {
            var inputVector = GetMovementVectorNormalized();
            moveDirection = new Vector2(inputVector.x, inputVector.y);
            moveDistance = moveSpeed * Time.deltaTime;

            // Check boundaries and adjust movement direction
            Vector2 newVelocity = moveDirection * moveDistance;

            // Check X boundaries
            if ((transform.position.x <= minX && newVelocity.x < 0) ||
                (transform.position.x >= maxX && newVelocity.x > 0))
            {
                newVelocity.x = 0;
            }

            // Check Y boundaries
            if ((transform.position.y <= minY && newVelocity.y < 0) ||
                (transform.position.y >= maxY && newVelocity.y > 0))
            {
                newVelocity.y = 0;
            }

            rigidbody2D.linearVelocity = newVelocity;

            // Flip the sprite based on movement direction
            if (playerSprite != null)
            {
                // Only flip vertically if moving up/down
                if (moveDirection.x != 0)
                {
                    playerSprite.flipX = moveDirection.x > 0;
                }
            }
        }
        else
        {
            rigidbody2D.linearVelocity = Vector2.zero;
        }
    }

    public void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        float moveSpeedPercent = playerStatsManager.GetStatValue(Stat.MoveSpeed) / 100;
        moveSpeed = baseMoveSpeed * (1 + moveSpeedPercent);
    }
}