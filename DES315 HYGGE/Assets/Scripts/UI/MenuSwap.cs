using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AK.Wwise;


public class MainMenu : MonoBehaviour
{
    //gameplay
    public GameObject Godmode;
    public GameObject GodmodeCheck;
    public bool GodModeToggleMS;
    
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

    //credits
    public GameObject HyggeLogo;
    public GameObject HyggeCredits;
    public GameObject AdditionalCredits;

    //wwise
    public string masterVolumeRTPC = "Master_Volume_Control";

    //slidervariables
    public Slider MasterValue;
    


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

    public void LoadEndState()
    {
        SceneManager.LoadScene(4);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void Update()
    {
        string cur = SceneManager.GetActiveScene().name;
        //Debug.Log(cur);
        if(cur == "Options")
        {
            gamevar.MasterValueFloat = MasterValue.value;
            AkUnitySoundEngine.SetRTPCValue(masterVolumeRTPC, gamevar.MasterValueFloat);
            Debug.Log(gamevar.MasterValueFloat);
            gamevar.GodModeToggle = GodmodeCheck.GetComponent<Toggle>().isOn;
        }
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

        //credits
        HyggeLogo.SetActive(false);
        HyggeCredits.SetActive(false);
        AdditionalCredits.SetActive(false);

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
        //wwise
        AkUnitySoundEngine.SetRTPCValue(masterVolumeRTPC, gamevar.MasterValueFloat);
        
        




        //visuals
        Fullscreen.SetActive(false);
        FullscreenCheck.SetActive(false);

        //credits
        HyggeLogo.SetActive(false);
        HyggeCredits.SetActive(false);
        AdditionalCredits.SetActive(false);

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

        //credits
        HyggeLogo.SetActive(false);
        HyggeCredits.SetActive(false);
        AdditionalCredits.SetActive(false);
    }

    public void OnMasterVolumeChanged(float value)
    {
        gamevar.MasterValueFloat = value;
        AkUnitySoundEngine.SetRTPCValue(masterVolumeRTPC, value);
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

        //credits
        HyggeLogo.SetActive(true);
        HyggeCredits.SetActive(true);
        AdditionalCredits.SetActive(true);

    }


}
