using UnityEngine;

public class StickyPlatform : MonoBehaviour
{
    private Transform playerTransform;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerTransform = collision.transform;
            playerTransform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // FIX: If the platform is turning off (Deactivating), stop here!
        // This prevents the "Cannot set parent" error.
        if (!gameObject.activeInHierarchy) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
            playerTransform = null;
        }
    }

    // This acts as the backup "Un-sticker" when the object vanishes
    private void OnDisable()
    {
        if (playerTransform != null)
        {
            playerTransform.SetParent(null);
            playerTransform = null;
        }
    }
}