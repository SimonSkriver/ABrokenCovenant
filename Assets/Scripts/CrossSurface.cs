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
    [SerializeField] GameObject churchDoor; //ONLY IF FINAL CHURCH CROSS

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
                //Activate cinemachine(maybe), or atleast door animation to open church doors. 
                isBeingHit = true;
            }
            else
            {
                isBeingHit = false;
            }
        }
    }
}

