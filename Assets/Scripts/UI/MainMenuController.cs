using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject sceneLoader;

    // Ensures the cursor is usable in the menu
    void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // SceneLoader handles the switch to correct scene
    public void StartGame()
    {
        sceneLoader.SetActive(true);
    }
}
