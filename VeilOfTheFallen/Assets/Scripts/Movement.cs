using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private PlayerAbilities abilities; // Reference to check for Blue Mask

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Climbing Settings")]
    [SerializeField] private float climbSpeed = 4f;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform wallCheck; // Assign a new empty object here

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private float defaultGravity;
    private bool isClimbing;

    void Start()
    {
        // Store normal gravity so we can restore it after climbing
        defaultGravity = rb.gravityScale;
    }

    void FixedUpdate()
    {
        // 1. Standard Horizontal Movement
        float xVelocity = inputManager.moveInput.x * moveSpeed;
        
        // 2. Climbing Logic
        if (CanClimb())
        {
            isClimbing = true;
            rb.gravityScale = 0f; // Turn off gravity so we don't slide down
            
            // Move Up/Down based on Vertical Input
            float yVelocity = inputManager.moveInput.y * climbSpeed;
            rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        }
        else
        {
            isClimbing = false;
            rb.gravityScale = defaultGravity; // Restore gravity
            
            // Normal Gravity Movement
            rb.linearVelocity = new Vector2(xVelocity, rb.linearVelocity.y);
        }
    }

    private bool CanClimb()
    {
        // Check 1: Are we actually touching a "Climbable" wall?
        bool touchingWall = Physics2D.OverlapCircle(wallCheck.position, groundCheckRadius, wallLayer);
        
        // Check 2: Do we have the Blue Mask?
        bool hasPower = abilities != null && abilities.hasBlueMask;

        // Check 3: Is the player actually pressing Up/Down? (Optional, prevents sticking by accident)
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
        // Visualizer for the Wall Check
        if (wallCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(wallCheck.position, groundCheckRadius);
        }
    }
}