using UnityEngine;
using System.Collections;

public class ChangeablePlatform : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private float activeDuration = 3.0f;
    [SerializeField] private Color frozenColor = Color.yellow; // Visual feedback when stopped

    [Header("References")]
    [SerializeField] private MovingPlatform moveScript; // Drag your MovingPlatform script here
    [SerializeField] private SpriteRenderer myRenderer;

    private Color originalColor;
    private bool isFrozen = false;
    private Coroutine revertCoroutine;

    private void Start()
    {
        if (myRenderer != null) originalColor = myRenderer.color;
        
        // Ensure the platform starts moving
        if (moveScript != null) moveScript.enabled = true;
    }

    // Called by the Yellow Mask Raycast
    public void ActivateChange()
    {
        // If already frozen, just reset the timer so it stays frozen longer
        if (isFrozen)
        {
            if (revertCoroutine != null) StopCoroutine(revertCoroutine);
            revertCoroutine = StartCoroutine(RevertRoutine());
            return;
        }

        // Freeze the platform
        isFrozen = true;
        
        // Visual Feedback
        if (myRenderer != null) myRenderer.color = frozenColor;
        
        // Logic: Turn OFF the movement script
        if (moveScript != null) moveScript.enabled = false;

        // Start countdown to unfreeze
        revertCoroutine = StartCoroutine(RevertRoutine());
    }

    private IEnumerator RevertRoutine()
    {
        yield return new WaitForSeconds(activeDuration);
        Unfreeze();
    }

    private void Unfreeze()
    {
        isFrozen = false;
        
        // Restore Color
        if (myRenderer != null) myRenderer.color = originalColor;
        
        // Restore Movement
        if (moveScript != null) moveScript.enabled = true;
    }
}