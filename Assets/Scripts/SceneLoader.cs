using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    void OnEnable()
    {
        Debug.Log("Loading new scene");
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }
}
