using UnityEngine;
public enum CrossKind
{
    // Ended up not needed as we removed the light beam split
   NewLightCross,
   FinalChurchCross
}

public class CrossSurface : MonoBehaviour
{
    [SerializeField] CrossKind crossKind; 
    [SerializeField] bool isBeingHit = false;
    [SerializeField] GameObject newLightBeam; // ONLY NEEDED IF NEW LIGHT ACTIVATOR CROSS
    [SerializeField] Animator churchAnimator; // ONLY NEEDED FOR FINAL CROSS

    public void CrossAction()
    {
        // UNUSED but easy to add to game if wanted
        if (crossKind == CrossKind.NewLightCross) 
        {
            // Check if currently being hit by a light beam
            if (!isBeingHit)
            {
            // Active the new light beam GameObject
            newLightBeam.SetActive(true);
            isBeingHit = true;
            }

            // If action is called called again while isBeingHit is true, its because you moved the beam away from the cross
            else
            {
                // Disable light beam
                newLightBeam.SetActive(false);
                isBeingHit = false;
            }
        }

        if (crossKind == CrossKind.FinalChurchCross)
        {
            // If not currently being hit by light beam
            if (!isBeingHit)
            {
                // Open church door
                churchAnimator.SetBool("CrossHit", true); 
                isBeingHit = true;
            }
            else
            {
                // Had some jankiness so removed close animation, once you've hit the final cross, once, the door opens.
                // churchAnimator.SetBool("CrossHit", false);
                isBeingHit = false;
            }
        }
    }
}