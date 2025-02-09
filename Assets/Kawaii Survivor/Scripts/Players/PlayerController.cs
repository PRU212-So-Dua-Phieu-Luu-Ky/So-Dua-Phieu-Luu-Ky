using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class NewMonoBehaviourScript : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private MobileJoystick mobileJoystick;

    private Rigidbody2D rigidbody2D;

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 1f;

    private Vector3 moveVector;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void FixedUpdate()
    {
        moveVector = mobileJoystick.GetMoveVector() * moveSpeed * Time.fixedDeltaTime;
        rigidbody2D.linearVelocity = moveVector;
    }
}