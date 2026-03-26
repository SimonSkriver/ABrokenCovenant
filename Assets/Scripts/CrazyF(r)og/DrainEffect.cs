using UnityEngine;

public class DrainEffect : MonoBehaviour
{
    public float counter;   
    public float maxTime;
    public bool playerIsHere;
    
    void Update()
    {
        HandleCounting();
    }
    //playerIsHere gets updated OnTriggerEnter and Exit
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsHere = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsHere = false;
        }
    }
    //When player is here, we start counting up, and count down when there player isn't here
    //This counter is used in crazy fog post process
    void HandleCounting()
    {
        if (playerIsHere)
        {
            counter += Time.deltaTime;
        }
        else
        {
            counter -= Time.deltaTime;
        }
        //The value is clamped between 0 and the max time allowed
        counter = Mathf.Clamp(counter, 0f, maxTime);
    }
}