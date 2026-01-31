using UnityEngine;
using UnityEngine.SceneManagement; // Required for reloading

public class DeadlyHazard : MonoBehaviour
{
    // Works if the hazard is a Trigger (like Lava or transparent Spikes)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            RestartLevel();
        }
    }

    // Works if the hazard is Solid (like a physical Spike wall)
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            RestartLevel();
        }
    }

    private void RestartLevel()
    {
        // Get the name of the current scene and load it again
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}