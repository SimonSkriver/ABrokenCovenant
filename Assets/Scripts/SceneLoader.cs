using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    void OnEnable()
    {
        Debug.Log("Loading new scene");
        if (SceneManager.GetActiveScene().name == "Mainscreen") 
        {
            SceneManager.LoadSceneAsync("Cutscene scene", LoadSceneMode.Single);
        }
        else if (SceneManager.GetActiveScene().name == "Cutscene scene") 
        {
            SceneManager.LoadSceneAsync("Town", LoadSceneMode.Single);
        }
        //SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }
}
