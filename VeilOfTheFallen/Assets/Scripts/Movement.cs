using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private float moveSpeed = 5f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //rb.AddForce(inputManager.moveInput * moveSpeed);
        Vector2 velocity = inputManager.moveInput * moveSpeed;
        rb.linearVelocity = velocity;
    }
}
