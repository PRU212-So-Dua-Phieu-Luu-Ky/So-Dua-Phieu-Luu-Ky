using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, IGameStateListener, IPlayerStatDependency
{
    [Header("Elements")]
    [SerializeField] private MobileJoystick mobileJoystick;

    private Rigidbody2D rigidbody2D;

    [Header("Settings")]
    private float moveSpeed = 1f;
    [SerializeField] private float baseMoveSpeed = 1f;

    private PlayerInputSystem inputActions;

    private Vector3 moveVector;
    private Vector2 moveDirection;
    private float moveDistance;
    private bool canMove;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        inputActions = new PlayerInputSystem();
        inputActions.Player.Enable();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }


    private void OnDestroy()
    {
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
            rigidbody2D.linearVelocity = moveDirection * moveDistance;
        }
        else rigidbody2D.linearVelocity = Vector2.zero;
    }

    public void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        float moveSpeedPercent = playerStatsManager.GetStatValue(Stat.MoveSpeed) / 100;
        moveSpeed = baseMoveSpeed * (1 + moveSpeedPercent);
    }
}