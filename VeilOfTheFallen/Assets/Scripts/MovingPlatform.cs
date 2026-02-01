using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Waypoints")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float waitTime = 0.5f;

    private int currentPointIndex = 0;
    private float waitCounter = 0f;
    private bool isWaiting = false;

    private void Update()
    {
        // 1. Safety Check: If no waypoints, do nothing
        if (waypoints == null || waypoints.Length < 2) return;

        // 2. If we are at the target waypoint, wait
        if (isWaiting)
        {
            waitCounter -= Time.deltaTime;
            if (waitCounter <= 0)
            {
                isWaiting = false;
                currentPointIndex = (currentPointIndex + 1) % waypoints.Length;
            }
            return;
        }

        // 3. Move towards the current target
        Transform target = waypoints[currentPointIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // 4. Check if we reached the target
        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            isWaiting = true;
            waitCounter = waitTime;
        }
    }

    // Helps you see the path in the Editor
    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length < 2) return;
        //Gizmos.color = Color.green;
        for (int i = 0; i < waypoints.Length; i++)
        {
            Transform p1 = waypoints[i];
            Transform p2 = waypoints[(i + 1) % waypoints.Length];
            if (p1 != null && p2 != null) Gizmos.DrawLine(p1.position, p2.position);
        }
    }
}