using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private PlayerAbilities abilities;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 12f;

    [Header("Climbing & Wall Jump")]
    [SerializeField] private float climbSpeed = 4f;
    [SerializeField] private Vector2 wallJumpForce = new Vector2(8f, 12f); // X is kick away, Y is jump up
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform wallCheck;

    [Header("Ground & Platform Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer; 

    [Header("Shooting Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float fireRate = 0.2f;

    // --- PRIVATE VARIABLES ---
    private float defaultGravity;
    private bool isClimbing;
    private float wallCheckDistance;
    private bool isShooting = false;
    
    // Wall Jump Control
    private bool isWallJumping;
    private float wallJumpDirection;

    void Start()
    {
        defaultGravity = rb.gravityScale;

        if (wallCheck != null)
        {
            wallCheckDistance = Mathf.Abs(wallCheck.localPosition.x);
        }
    }

    void Update()
    {
        if (Mouse.current.leftButton.isPressed && !isShooting)
        {
            StartCoroutine(Attack());
        }
    }

    void FixedUpdate()
    {
        // 1. INPUT HANDLING
        float inputX = inputManager.moveInput.x;

        // If we are currently being "kicked" off a wall, ignore player input for a split second
        if (isWallJumping) return;

        // --- FLIP WALL CHECK ---
        if (wallCheck != null)
        {
            if (inputX > 0.1f)
            {
                Vector3 newPos = wallCheck.localPosition;
                newPos.x = wallCheckDistance; 
                wallCheck.localPosition = newPos;
            }
            else if (inputX < -0.1f)
            {
                Vector3 newPos = wallCheck.localPosition;
                newPos.x = -wallCheckDistance;
                wallCheck.localPosition = newPos;
            }
        }

        // 2. HORIZONTAL MOVEMENT
        float xVelocity = inputX * moveSpeed;
        
        // 3. CLIMBING LOGIC
        // We only climb if touching a wall AND holding Up/Down
        // (Or if we are just hanging still)
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
            
            // Standard physics movement
            rb.linearVelocity = new Vector2(xVelocity, rb.linearVelocity.y);
        }
    }

    private bool CanClimb()
    {
        bool touchingWall = Physics2D.OverlapCircle(wallCheck.position, groundCheckRadius, wallLayer);
        bool hasPower = abilities != null && abilities.hasBlueMask;
        
        // Only actually "Climb" (stop gravity) if we are outputting vertical input
        // This prevents getting stuck to walls when just jumping past them
        bool distinctInput = Mathf.Abs(inputManager.moveInput.y) > 0.1f;

        return touchingWall && hasPower && distinctInput;
    }

    public void OnJump()
    {
        // Check states
        bool isGrounded = IsGrounded();
        bool touchingWall = Physics2D.OverlapCircle(wallCheck.position, groundCheckRadius, wallLayer);
        bool hasBlueMask = abilities != null && abilities.hasBlueMask;

        // 1. GROUND JUMP (Priority)
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        // 2. WALL JUMP (Hollow Knight Style)
        else if (touchingWall && hasBlueMask)
        {
            StartCoroutine(WallJumpRoutine());
        }
    }

    private IEnumerator WallJumpRoutine()
    {
        isWallJumping = true;
        isClimbing = false; // Stop climbing physics immediately

        // Determine direction: If wall is to our Right, we jump Left (-1)
        // We check where the wallCheck object currently is
        float jumpDir = wallCheck.localPosition.x > 0 ? -1f : 1f;

        // Apply force: Kick away (X) + Jump Up (Y)
        rb.linearVelocity = Vector2.zero; // Reset current momentum for snappy feel
        rb.AddForce(new Vector2(jumpDir * wallJumpForce.x, wallJumpForce.y), ForceMode2D.Impulse);

        // Lock movement controls for 0.2 seconds so player doesn't steer back into wall instantly
        yield return new WaitForSeconds(0.2f);

        isWallJumping = false;
    }

    private bool IsGrounded()
    {
        // Remembers to check BOTH "Ground" and "Movable" layers in Inspector!
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    IEnumerator Attack()
    {
        isShooting = true;

        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        Vector2 direction = (mouseWorldPosition - transform.position).normalized;
        Vector3 spawnPosition = transform.position + (Vector3)(direction * 1.0f);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (bulletPrefab != null)
        {
            GameObject newBullet = Instantiate(bulletPrefab, spawnPosition, rotation);
            Rigidbody2D bulletRb = newBullet.GetComponent<Rigidbody2D>();
            
            if (bulletRb != null)
            {
                bulletRb.linearVelocity = direction * bulletSpeed;
            }
            Destroy(newBullet, 2f);
        }
        yield return new WaitForSeconds(fireRate);
        isShooting = false;
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