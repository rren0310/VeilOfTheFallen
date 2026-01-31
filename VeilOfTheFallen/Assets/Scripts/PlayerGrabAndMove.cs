using UnityEngine;

public class PlayerGrabAndMove : MonoBehaviour
{
    [Header("Grab Settings")]
    [SerializeField] private KeyCode grabKey = KeyCode.E;
    [SerializeField] private LayerMask movableLayer;
    [SerializeField] private Transform grabCheckPoint;
    [SerializeField] private Vector2 grabCheckSize = new Vector2(0.6f, 0.6f);

    [Header("Movement Settings")]
    [SerializeField] private Movement normalMovementScript;

    private MovableObject2D_Grab grabbedObject;
    private FixedJoint2D grabJoint; // The physics "Glue"
    private bool isGrabbing;

    private void Update()
    {
        if (Input.GetKeyDown(grabKey))
        {
            if (isGrabbing) Release();
            else TryGrab();
        }
    }

    private void TryGrab()
    {
        Collider2D hit = Physics2D.OverlapBox(grabCheckPoint.position, grabCheckSize, 0f, movableLayer);

        if (hit != null)
        {
            MovableObject2D_Grab obj = hit.GetComponent<MovableObject2D_Grab>();
            if (obj != null && obj.CanGrab)
            {
                grabbedObject = obj;
                
                // 1. Perform the Snap BEFORE connecting the joint
                SnapToBox(obj);

                // 2. Create the Joint (The Glue)
                ConnectJoint(obj);

                // 3. Notify the box
                grabbedObject.Grab(transform);
                isGrabbing = true;
            }
        }
    }

    private void SnapToBox(MovableObject2D_Grab obj)
    {
        // Get the colliders to measure widths
        Collider2D playerCol = GetComponent<Collider2D>();
        Collider2D boxCol = obj.GetComponent<Collider2D>();

        if (playerCol != null && boxCol != null)
        {
            // Determine side based on which side of the box the player is currently on
            float xDiff = transform.position.x - obj.transform.position.x;
            float directionFromBox = Mathf.Sign(xDiff); // +1 if Player is Right, -1 if Player is Left

            // Calculate the perfect offset: (Half Player Width) + (Half Box Width)
            float offset = playerCol.bounds.extents.x + boxCol.bounds.extents.x + 0.02f;

            // Apply position
            Vector3 newPos = transform.position;
            newPos.x = obj.transform.position.x + (directionFromBox * offset);
            transform.position = newPos;
        }
    }

    private void ConnectJoint(MovableObject2D_Grab obj)
    {
        // Add a FixedJoint2D to the player dynamically
        grabJoint = gameObject.AddComponent<FixedJoint2D>();
        
        // Connect it to the Box's Rigidbody
        grabJoint.connectedBody = obj.GetComponent<Rigidbody2D>();
        
        // Ensure the joint doesn't try to auto-calculate crazy distances
        grabJoint.autoConfigureConnectedAnchor = false;
        
        // IMPORTANT: Allow the joint to handle collisions correctly
        grabJoint.enableCollision = false; 
    }

    private void Release()
    {
        if (grabJoint != null)
        {
            Destroy(grabJoint); // Remove the glue
        }

        if (grabbedObject != null)
        {
            grabbedObject.Release();
            grabbedObject = null;
        }

        isGrabbing = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (grabCheckPoint == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(grabCheckPoint.position, grabCheckSize);
    }
}