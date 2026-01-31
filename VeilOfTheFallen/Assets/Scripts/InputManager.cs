using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] public Vector2 moveInput;
    
    [SerializeField] private float Horizontal;
    [SerializeField] private float Vertical;
    
    void Update()
    {
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");
        moveInput = new Vector2(Horizontal, Vertical);
    }
}
