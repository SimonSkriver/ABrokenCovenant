using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CutsceneControls : MonoBehaviour
{
    private InputActionMap cutsceneInput;
    private InputAction skipCutscene;

    void Start()
    {
        cutsceneInput = InputSystem.actions.FindActionMap("Cutscene");
        cutsceneInput.Enable();
        skipCutscene = InputSystem.actions.FindAction("SkipCutscene");
    }

    void Update()
    {
        if (skipCutscene.WasPressedThisFrame())
        {
            SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
        }
    }
}