using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("References")]
    // Drag your Player object with the Movement script here in the Inspector
    [SerializeField] private Movement movementScript; 

    [Header("Output")]
    [SerializeField] public Vector2 moveInput;

    // These variables are for debugging view in Inspector
    [SerializeField] private float Horizontal;
    [SerializeField] private float Vertical;

    void Update()
    {
        // 1. Handle Movement Input (Continuous)
        Horizontal = Input.GetAxis("Horizontal"); // Maps to WASD & Left Stick
        Vertical = Input.GetAxis("Vertical");
        
        moveInput = new Vector2(Horizontal, Vertical);

        // 2. Handle Jump Input (Instant)
        // "Jump" Maps to Spacebar & Controller Button 0 (A/Cross)
        if (Input.GetButtonDown("Jump"))
        {
            // We call the jump method on the Movement script directly
            if (movementScript != null)
            {
                movementScript.OnJump();
            }
        }
    }
}