using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovableObject2D_Grab : MonoBehaviour
{
    [Header("Tuning")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float maxSpeed = 7f;
    [SerializeField] private float dragWhenGrabbed = 12f;

    private Rigidbody2D rb;
    private float defaultDrag;
    private bool isGrabbed;
    private Transform grabber;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultDrag = rb.linearDamping;
    }

    public bool CanGrab => !isGrabbed;

    public void Grab(Transform player)
    {
        isGrabbed = true;
        grabber = player;
        rb.linearDamping = dragWhenGrabbed;
    }

    public void Release()
    {
        isGrabbed = false;
        grabber = null;
        rb.linearDamping = defaultDrag;
    }

    public void Move(float direction)
    {
        if (!isGrabbed) return;

        // Only move horizontally (platformer style)
        Vector2 v = rb.linearVelocity;
        v.x = direction * moveSpeed;

        // Clamp
        if (Mathf.Abs(v.x) > maxSpeed)
            v.x = Mathf.Sign(v.x) * maxSpeed;

        rb.linearVelocity = v;
    }
}