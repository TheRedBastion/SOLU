using UnityEngine;
using UnityEngine.SceneManagement;



public class MainMenu : MonoBehaviour
{
    //gameplay
    public GameObject Godmode;
    public GameObject GodmodeCheck;
    
    //audio
    public GameObject MasterVolume;
    public GameObject MusicVolume;
    public GameObject SFXVolume;
    public GameObject MasterVolumeSlider;
    public GameObject MusicVolumeSlider;
    public GameObject SFXVolumeSlider;

    //visuals
    public GameObject Fullscreen;
    public GameObject FullscreenCheck;

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
        //gameplay
        Godmode.SetActive(true);
        GodmodeCheck.SetActive(true);

        //audio
        MasterVolume.SetActive(false);
        MusicVolume.SetActive(false);
        SFXVolume.SetActive(false);
        MasterVolumeSlider.SetActive(false);
        MusicVolumeSlider.SetActive(false);
        SFXVolumeSlider.SetActive(false);

        //visuals
        Fullscreen.SetActive(false);
        FullscreenCheck.SetActive(false);
    }

    public void AudioButton()
    {
        //gameplay
        Godmode.SetActive(false);
        GodmodeCheck.SetActive(false);

        //audio
        MasterVolume.SetActive(true);
        MusicVolume.SetActive(true);
        SFXVolume.SetActive(true);
        MasterVolumeSlider.SetActive(true);
        MusicVolumeSlider.SetActive(true);
        SFXVolumeSlider.SetActive(true);

        //visuals
        Fullscreen.SetActive(false);
        FullscreenCheck.SetActive(false);
    }

    public void VisualsButton()
    {
        //gameplay
        Godmode.SetActive(false);
        GodmodeCheck.SetActive(false);

        //audio
        MasterVolume.SetActive(false);
        MusicVolume.SetActive(false);
        SFXVolume.SetActive(false);
        MasterVolumeSlider.SetActive(false);
        MusicVolumeSlider.SetActive(false);
        SFXVolumeSlider.SetActive(false);

        //visuals
        Fullscreen.SetActive(true);
        FullscreenCheck.SetActive(true);
    }
    public void CreditsButton()
    {
        //gameplay
        Godmode.SetActive(false);
        GodmodeCheck.SetActive(false);

        //audio
        MasterVolume.SetActive(false);
        MusicVolume.SetActive(false);
        SFXVolume.SetActive(false);
        MasterVolumeSlider.SetActive(false);
        MusicVolumeSlider.SetActive(false);
        SFXVolumeSlider.SetActive(false);

        //visuals
        Fullscreen.SetActive(false);
        FullscreenCheck.SetActive(false);
    }


}
