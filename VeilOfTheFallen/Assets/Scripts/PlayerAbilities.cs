using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    [Header("Unlocked Masks")]
    public bool hasRedMask = false;
    public bool hasBlueMask = false; // For future use
    public bool hasYellowMask = false; // For future use
    [Header("Yellow Mask Settings")]
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private KeyCode interactKey = KeyCode.F;
    [SerializeField] private LayerMask whatIsPlatform; // <--- NEW: Tells us what to hit

    // This function will be called by the Pickup script
    public void UnlockMask(MaskPickup.MaskType type)
    {
        switch (type)
        {
            case MaskPickup.MaskType.Red:
                hasRedMask = true;
                Debug.Log("Red Mask Abilities Unlocked: Shooting Enabled!");
                break;
                
            case MaskPickup.MaskType.Blue:
                hasBlueMask = true;
                break;

            case MaskPickup.MaskType.Yellow:
                hasYellowMask = true;
                break;
        }
    }

    private void Update()
    {
        // Only run if we have the Yellow Mask
        // (Assuming you have a hasYellowMask bool defined)
        if (Input.GetKeyDown(interactKey) && hasYellowMask) 
        {
            PerformYellowInteract();
        }
    }

    private void PerformYellowInteract()
    {
        // FORCE the direction based on which way the sprite faces
        // If your sprite flips using "Flip X" in SpriteRenderer:
        float direction = GetComponent<SpriteRenderer>().flipX ? -1 : 1; 
        
        // If your sprite flips using Transform Scale (e.g. -1 scale):
        // float direction = transform.localScale.x > 0 ? 1 : -1;

        Vector2 origin = transform.position;
        // Lift the ray up slightly so it shoots from the Chest, not the Feet
        origin.y += 0.5f; 

        // Raycast
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.right * direction, interactRange, whatIsPlatform);

        // Visual Debug
        Color color = (hit.collider != null) ? Color.green : Color.red;
        //Debug.DrawRay(origin, Vector2.right * direction * interactRange, color, 2f);

        if (hit.collider != null)
        {
            ChangeablePlatform platform = hit.collider.GetComponentInParent<ChangeablePlatform>();
            if (platform != null)
            {
                platform.ActivateChange(); // The ONLY place this should be called
            }
        }
    }
}