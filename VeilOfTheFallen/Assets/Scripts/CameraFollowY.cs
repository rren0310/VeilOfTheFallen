using UnityEngine;

public class CameraFollowY : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform player;

    [Header("Settings")]
    [Tooltip("How far above/below the player the camera should sit.")]
    [SerializeField] private float yOffset = 2f;
    
    [Tooltip("If checked, the camera will NEVER move down, even if the player falls.")]
    [SerializeField] private bool oneWayOnly = true;
    
    [Tooltip("How smooth the camera movement is (0 = instant, 0.1 = smooth).")]
    [SerializeField] private float smoothSpeed = 0.125f;

    private void LateUpdate()
    {
        if (player == null) return;

        // 1. Calculate the goal position
        // We keep the Camera's original X and Z, only changing Y
        float targetY = player.position.y + yOffset;
        
        // 2. Check "One Way" Logic
        if (oneWayOnly)
        {
            // Only update targetY if it is HIGHER than where we are now
            if (targetY < transform.position.y)
            {
                targetY = transform.position.y;
            }
        }

        // 3. Smoothly move to the new Y
        Vector3 desiredPosition = new Vector3(transform.position.x, targetY, transform.position.z);
        
        // Use Lerp for smooth movement
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        
        // 4. Apply the position
        transform.position = smoothedPosition;
    }
}