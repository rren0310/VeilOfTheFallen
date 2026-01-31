using UnityEngine;
using UnityEngine.SceneManagement; // Required for changing scenes

public class LevelExit : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The exact name of the scene you want to load next.")]
    [SerializeField] private string nextSceneName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only the player should trigger the level exit
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player reached the exit! Loading: " + nextSceneName);
            
            // This loads the scene
            SceneManager.LoadScene(nextSceneName);
        }
    }
}