using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }
}
