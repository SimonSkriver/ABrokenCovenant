using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    void OnEnable()
    {
        Debug.Log("Loading new scene");
        if (SceneManager.GetActiveScene().name == "MainMenu") 
        {
            RemoveCursor();
            SceneManager.LoadSceneAsync("Cutscene scene", LoadSceneMode.Single);
        }
        else if (SceneManager.GetActiveScene().name == "Cutscene scene") 
        {
            RemoveCursor();
            SceneManager.LoadSceneAsync("Town", LoadSceneMode.Single);
        }
        else if (SceneManager.GetActiveScene().name == "Town")
        {
            SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
        }
    }

    private void RemoveCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }
}
