using UnityEngine;

public class StickyPlatform : MonoBehaviour
{
    private Transform playerTransform;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Save a reference to the player
            playerTransform = collision.transform;
            playerTransform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Only try to un-parent if the object is actually active.
        // If it's turning off, OnDisable will handle it safely.
        if (gameObject.activeInHierarchy && collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
            playerTransform = null;
        }
    }

    // SAFEGUARD: This runs immediately when the platform is turned off (or destroyed)
    private void OnDisable()
    {
        // If we are currently holding the player, let them go immediately
        if (playerTransform != null && playerTransform.parent == transform)
        {
            playerTransform.SetParent(null);
            playerTransform = null;
        }
    }
}