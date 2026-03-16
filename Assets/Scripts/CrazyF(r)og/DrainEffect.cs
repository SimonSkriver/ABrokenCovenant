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

        counter = Mathf.Clamp(counter, 0f, maxTime);

        if (counter >= maxTime)
        {
            KillPlayer();
        }
    }

    void KillPlayer()
    {
        Debug.Log("You died lol");
    }
}
