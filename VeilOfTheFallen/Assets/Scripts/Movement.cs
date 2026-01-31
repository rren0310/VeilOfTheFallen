using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private PlayerAbilities abilities;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Climbing Settings")]
    [SerializeField] private float climbSpeed = 4f;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform wallCheck;

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    // --- PRIVATE VARIABLES ---
    private float defaultGravity;
    private bool isClimbing;

    // !!! THIS IS THE MISSING VARIABLE !!!
    private float wallCheckDistance;

    void Start()
    {
        defaultGravity = rb.gravityScale;

        // Calculate the initial distance of the wallCheck from the center of the player
        if (wallCheck != null)
        {
            wallCheckDistance = Mathf.Abs(wallCheck.localPosition.x);
        }
    }

    void FixedUpdate()
    {
        float inputX = inputManager.moveInput.x;

        // --- NEW: FLIP THE WALL CHECK ---
        if (wallCheck != null)
        {
            // If pressing Right, put check on the Right (+)
            if (inputX > 0.1f)
            {
                Vector3 newPos = wallCheck.localPosition;
                newPos.x = wallCheckDistance;
                wallCheck.localPosition = newPos;
            }
            // If pressing Left, put check on the Left (-)
            else if (inputX < -0.1f)
            {
                Vector3 newPos = wallCheck.localPosition;
                newPos.x = -wallCheckDistance;
                wallCheck.localPosition = newPos;
            }
        }
        // --------------------------------

        // 1. Standard Horizontal Movement
        float xVelocity = inputX * moveSpeed;

        // 2. Climbing Logic
        if (CanClimb())
        {
            isClimbing = true;
            rb.gravityScale = 0f;

            float yVelocity = inputManager.moveInput.y * climbSpeed;
            rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        }
        else
        {
            isClimbing = false;
            rb.gravityScale = defaultGravity;

            rb.linearVelocity = new Vector2(xVelocity, rb.linearVelocity.y);
        }
    }

    private bool CanClimb()
    {
        // Check 1: Are we touching a wall?
        bool touchingWall = Physics2D.OverlapCircle(wallCheck.position, groundCheckRadius, wallLayer);

        // Check 2: Do we have the Blue Mask?
        bool hasPower = abilities != null && abilities.hasBlueMask;

        // Check 3: Is the player actually pressing Up/Down?
        bool distinctInput = Mathf.Abs(inputManager.moveInput.y) > 0.1f;

        return touchingWall && hasPower && distinctInput;
    }

    public void OnJump()
    {
        if (IsGrounded() || isClimbing)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
        if (wallCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(wallCheck.position, groundCheckRadius);
        }
    }
}
