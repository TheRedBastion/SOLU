using UnityEngine;
using UnityEngine.SceneManagement;



public class MainMenu : MonoBehaviour
{

    public GameObject Godmode;
    public GameObject GodmodeCheck;

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadOptions()
    {
        SceneManager.LoadScene(3);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GameplayButton()
    {
        Godmode.SetActive(true);
        GodmodeCheck.SetActive(true);
    }

    public void AudioButton()
    {
        Godmode.SetActive(false);
        GodmodeCheck.SetActive(false);
    }

    public void CreditsButton()
    {
        Godmode.SetActive(false);
        GodmodeCheck.SetActive(false);
    }

    public void VisualsButton()
    {
        Godmode.SetActive(false);
        GodmodeCheck.SetActive(false);
    }
}
