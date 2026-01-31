using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    [Header("Unlocked Masks")]
    public bool hasRedMask = false;
    public bool hasBlueMask = false; // For future use
    public bool hasYellowMask = false; // For future use

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
}