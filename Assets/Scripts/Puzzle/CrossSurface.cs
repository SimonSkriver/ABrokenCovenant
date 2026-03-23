using UnityEngine;
public enum CrossKind
{
   NewLightCross,
   FinalChurchCross
}

public class CrossSurface : MonoBehaviour
{
    [SerializeField] CrossKind crossKind; 
    [SerializeField] bool isBeingHit = false;
    [SerializeField] GameObject newLightBeam; //ONLY IF NEW LIGHT ACTIVATOR CROSS
    [SerializeField] Animator churchAnimator;

    public void CrossAction()
    {
        if (crossKind == CrossKind.NewLightCross)
        {
            if (!isBeingHit) 
            {
            newLightBeam.SetActive(true);
            isBeingHit = true;
            }
            else
            {
                newLightBeam.SetActive(false);
                isBeingHit = false;
            }
        }

        if (crossKind == CrossKind.FinalChurchCross)
        {
            if (!isBeingHit)
            {
                churchAnimator.SetBool("CrossHit", true);
                isBeingHit = true;
                
            }
            else
            {
                //this results in a flip/flop. So hitting the cross the first time opens the door. Hitting it again closes it. Thus I've left it out, so it only opens once.
                //churchAnimator.SetBool("CrossHit", false);
                isBeingHit = false;
            }
        }
    }
}