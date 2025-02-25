using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class NewMonoBehaviourScript : MonoBehaviour, IGameStateListener
{
    [Header("Elements")]
    [SerializeField] private MobileJoystick mobileJoystick;

    private Rigidbody2D rigidbody2D;

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 1f;

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

    // Update is called once per frame
    private void Update()
    {
        HandleMovement();
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
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

    private void OnDestroy()
    {
        inputActions.Disable();
        inputActions.Dispose();
    }

    private void FixedUpdate()
    {
        //moveVector = mobileJoystick.GetMoveVector() * moveSpeed * Time.fixedDeltaTime;
        //rigidbody2D.linearVelocity = moveVector;
        //HandleMovement();
    }

    public void GameStateChangedCallBack(GameState gameState)
    {
        if (gameState != GameState.GAME) canMove = false;
        else canMove = true;
    }
}