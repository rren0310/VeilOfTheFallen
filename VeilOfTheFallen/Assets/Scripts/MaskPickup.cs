using UnityEngine;

public class MaskPickup : MonoBehaviour
{
    // Define the types of masks available (good for organization later)
    public enum MaskType { Red, Blue, Yellow }

    [Header("Settings")]
    public MaskType maskColor; // Select 'Red' in the inspector for this one
    [SerializeField] private float floatSpeed = 2f;
    [SerializeField] private float floatHeight = 0.25f;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private Dialogue dialogueSystem;

    private Vector3 startPos;

    private void Start()
    {
        // Remember where the mask started so we can bob up and down relative to this spot
        startPos = transform.position;
    }

    private void Update()
    {
        // Smooth floating effect using Math Sine wave
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object colliding is the Player
        if (other.CompareTag("Player"))
        {
            PickUp(other.gameObject);
            dialogueBox.SetActive(true);
            dialogueSystem.StartDialogue();
        }
    }

    private void PickUp(GameObject player)
    {
        // 1. Try to find the PlayerAbilities script on the object we collided with
        PlayerAbilities abilities = player.GetComponent<PlayerAbilities>();

        // 2. If the script exists, unlock the mask
        if (abilities != null)
        {
            abilities.UnlockMask(maskColor);
            
            // Visual feedback (Optional)
            Debug.Log($"Picked up the {maskColor} Mask!");

            // 3. Destroy the object so it disappears
            Destroy(gameObject);
        }
    }
}