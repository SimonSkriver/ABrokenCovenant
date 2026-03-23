using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void CloseGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }
}
