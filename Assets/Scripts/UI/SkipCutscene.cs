using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SkipCutscene : MonoBehaviour
{
    private InputActionMap cutsceneInputMap;
    private InputAction skipCutscene;

    void Start()
    {
        cutsceneInputMap = InputSystem.actions.FindActionMap("Cutscene");
        cutsceneInputMap.Enable();
        skipCutscene = InputSystem.actions.FindAction("SkipCutscene");
    }

    void Update()
    {
        if (skipCutscene.WasPressedThisFrame())
        {
            SceneManager.LoadSceneAsync("Town", LoadSceneMode.Single); // Load level scene, if skipCutscene is pressed
        }
    }
}