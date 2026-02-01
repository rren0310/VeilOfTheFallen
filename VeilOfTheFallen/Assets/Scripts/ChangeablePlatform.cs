using UnityEngine;
using System.Collections;

public class ChangeablePlatform : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("How long the platform stops moving when shot.")]
    [SerializeField] private float stopDuration = 3.0f;
    
    [Tooltip("Color to change to when stopped (Visual Feedback).")]
    [SerializeField] private Color frozenColor = Color.yellow; 

    [Header("References")]
    [SerializeField] private MovingPlatform moveScript;
    [SerializeField] private SpriteRenderer myRenderer;

    private Color originalColor;
    private Coroutine stopCoroutine;
    private bool isStopped = false;

    private void Start()
    {
        // Save the original color so we can switch back later
        if (myRenderer != null) originalColor = myRenderer.color;

        // Auto-find the moving script if you forgot to drag it in
        if (moveScript == null) moveScript = GetComponent<MovingPlatform>();
    }

    // This function is called by your Player's "Yellow Mask" Raycast
    public void ActivateChange()
    {
        // If already stopped, just reset the timer (keep it stopped longer)
        if (isStopped)
        {
            if (stopCoroutine != null) StopCoroutine(stopCoroutine);
            stopCoroutine = StartCoroutine(WaitAndResume());
            return;
        }

        // 1. FREEZE!
        isStopped = true;
        
        // Turn OFF the movement script
        if (moveScript != null) moveScript.enabled = false;

        // Change Color
        if (myRenderer != null) myRenderer.color = frozenColor;

        // Start the timer to unfreeze
        stopCoroutine = StartCoroutine(WaitAndResume());
    }

    private IEnumerator WaitAndResume()
    {
        yield return new WaitForSeconds(stopDuration);

        // 2. UNFREEZE!
        isStopped = false;

        // Restore Color
        if (myRenderer != null) myRenderer.color = originalColor;

        // Turn ON the movement script
        if (moveScript != null) moveScript.enabled = true;
    }
}