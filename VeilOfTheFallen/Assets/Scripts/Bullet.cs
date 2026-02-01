using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 1. If the bullet is a physical object (bounces off things)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Don't destroy if we hit the player who shot us
        if (collision.gameObject.CompareTag("Player")) return;

        Destroy(gameObject);
    }

    // 2. If the bullet is a "Trigger" (passes through things like ghosts)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Don't destroy if we hit the player or other triggers (like checkpoints)
        if (collision.CompareTag("Player")) return;
        
        // Optional: Ignore other bullets so they don't destroy each other
        if (collision.GetComponent<Bullet>() != null) return;

        Destroy(gameObject);
    }
}