using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private InputManager inputManager;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    // FixedUpdate is for physics calculations
    void FixedUpdate()
    {
        // 1. Calculate X velocity based on Input
        // We only use .x from the input so Up/Down on the stick doesn't affect movement
        float xVelocity = inputManager.moveInput.x * moveSpeed;

        // 2. Apply Velocity
        // IMPORTANT: We use 'rb.linearVelocity.y' for the Y axis to preserve 
        // the effect of gravity (falling) calculated by the physics engine.
        rb.linearVelocity = new Vector2(xVelocity, rb.linearVelocity.y);
    }

    // Call this method from your InputManager script when the button is pressed
    public void OnJump()
    {
        if (IsGrounded())
        {
            // Reset Y velocity before jumping to ensure consistent jump height
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            
            // Apply instant upward force
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private bool IsGrounded()
    {
        // Creates a small invisible circle at the feet to check if we are touching 'Ground'
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    // Visualizes the ground check in the Editor so you can position it correctly
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}