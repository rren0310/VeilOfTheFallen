using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float waitTime = 1f; // Pause at each end
    
    [Header("Path Points")]
    [Tooltip("Drag empty GameObjects here to define the path.")]
    [SerializeField] private Transform[] waypoints;

    private int currentPointIndex = 0;
    private float waitTimer;
    private bool isWaiting;

    private void Start()
    {
        // If no waypoints are set, don't do anything to avoid errors
        if (waypoints == null || waypoints.Length < 2)
        {
            Debug.LogWarning("Moving Platform needs at least 2 waypoints!");
            enabled = false;
            return;
        }

        // Snap platform to the first point immediately
        transform.position = waypoints[0].position;
    }

    private void FixedUpdate()
    {
        if (isWaiting)
        {
            waitTimer -= Time.fixedDeltaTime;
            if (waitTimer <= 0)
            {
                isWaiting = false;
                NextWaypoint();
            }
            return;
        }

        MovePlatform();
    }

    private void MovePlatform()
    {
        // Get the target position
        Transform target = waypoints[currentPointIndex];
        
        // Move towards the target
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);

        // Check if we reached the target (very close distance)
        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            // Start waiting
            isWaiting = true;
            waitTimer = waitTime;
        }
    }

    private void NextWaypoint()
    {
        currentPointIndex++;
        // If we reached the end of the list, loop back to 0
        if (currentPointIndex >= waypoints.Length)
        {
            currentPointIndex = 0;
        }
    }
    
    // VISUALIZATION: Draws lines in the editor so you can see the path
    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        Gizmos.color = Color.green;
        for (int i = 0; i < waypoints.Length; i++)
        {
            Transform p1 = waypoints[i];
            Transform p2 = waypoints[(i + 1) % waypoints.Length]; // Loop back to start

            if (p1 != null && p2 != null)
            {
                Gizmos.DrawLine(p1.position, p2.position);
                Gizmos.DrawWireSphere(p1.position, 0.2f);
            }
        }
    }
}