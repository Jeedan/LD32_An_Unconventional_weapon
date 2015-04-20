using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        Application.LoadLevel("Base");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void CreditsScene()
    {
        Application.LoadLevel("Credits");
    }

    public void BackToMainMenu()
    {
        Application.LoadLevel("main_menu");
    }
}
