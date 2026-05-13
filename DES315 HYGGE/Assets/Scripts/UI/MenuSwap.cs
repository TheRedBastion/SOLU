using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AK.Wwise;


public class MainMenu : MonoBehaviour
{
    //gameplay
    public GameObject Godmode;
    public GameObject GodmodeCheck;
    public GameObject FreeCam;
    public GameObject FreeCamToggle;


    //audio
    public GameObject SliderGO;
    public GameObject MasterVolume;

    public GameObject MasterVolumeSlider;


    //visuals
    public GameObject Fullscreen;
    public GameObject FullscreenCheck;

    //credits
    public GameObject HyggeLogo;
    public GameObject HyggeCredits;
    public GameObject AdditionalCredits;

    //wwise
    public string masterVolumeRTPC = "Master_Volume_Control";
    public string musicVolumeRTPC = "Music_Volume_Control";
    public string SFXVolumeRTPC = "Sound_Effects_Volume_Control";

    //slidervariables
    public Slider MasterValue;
    public Slider MusicValue;
    public Slider SFXValue;

    public void LoadGame()
    {
        AkUnitySoundEngine.StopAll();

        SceneManager.LoadScene(1);
    }

    public void LoadMainMenu()
    {
        AkUnitySoundEngine.StopAll();
        SceneManager.LoadScene(0);
    }

    public void LoadOptions()
    {
        //AkUnitySoundEngine.StopAll();
        SceneManager.LoadScene(3);
    }

    public void LoadEndState()
    {
        AkUnitySoundEngine.StopAll();
        SceneManager.LoadScene(4);
    }

    public void QuitGame()
    {
        AkUnitySoundEngine.StopAll();
        Application.Quit();
    }

    void Start()
    {
        gamevar.Load();


        //apply Wwise volume immediately
        AkUnitySoundEngine.SetRTPCValue(masterVolumeRTPC, gamevar.MasterValueFloat);
        AkUnitySoundEngine.SetRTPCValue(musicVolumeRTPC, gamevar.MusicVolumeFloat);
        AkUnitySoundEngine.SetRTPCValue(SFXVolumeRTPC, gamevar.SFXVolumeFloat);
    }

    public void OnMasterVolumeChanged(float value)
    {
        gamevar.MasterValueFloat = value;

        AkUnitySoundEngine.SetRTPCValue(masterVolumeRTPC, value);

        gamevar.Save();
    }

    public void OnMusicVolumeChanged(float value)
    {
        gamevar.MusicVolumeFloat = value;

        AkUnitySoundEngine.SetRTPCValue(musicVolumeRTPC, value);

        gamevar.Save();
    }

    public void OnSFXVolumeChanged(float value)
    {
        gamevar.SFXVolumeFloat = value;

        AkUnitySoundEngine.SetRTPCValue(SFXVolumeRTPC, value);

        gamevar.Save();
    }

    public void OnGodModeChanged(bool value)
    {
        gamevar.GodModeToggle = value;

        gamevar.Save();
    }

    public void OnFreeCamChanged(bool value)
    {
        gamevar.FreeCamToggle = value;

        gamevar.Save();
    }

    void Update()
    {
        string cur = SceneManager.GetActiveScene().name;
        //Debug.Log(cur);

    }

    public void GameplayButton()
    {
        //gameplay
        Godmode.SetActive(true);
        GodmodeCheck.SetActive(true);
        FreeCam.SetActive(true);
        FreeCamToggle.SetActive(true);

        GodmodeCheck.GetComponent<Toggle>().isOn = gamevar.GodModeToggle;
        FreeCamToggle.GetComponent<Toggle>().isOn = gamevar.FreeCamToggle;

        //audio
        SliderGO.SetActive(false);

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
        FreeCam.SetActive(false);
        FreeCamToggle.SetActive(false);


        //audio
        SliderGO.SetActive(true);

        //apply saved values to UI
        MasterValue.value = gamevar.MasterValueFloat;
        MusicValue.value = gamevar.MusicVolumeFloat;
        SFXValue.value = gamevar.SFXVolumeFloat;

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
        FreeCam.SetActive(false);
        FreeCamToggle.SetActive(false);

        //audio
        SliderGO.SetActive(false);



        //visuals
        Fullscreen.SetActive(true);
        FullscreenCheck.SetActive(true);

        //credits
        HyggeLogo.SetActive(false);
        HyggeCredits.SetActive(false);
        AdditionalCredits.SetActive(false);
    }

    public void CreditsButton()
    {
        //gameplay
        Godmode.SetActive(false);
        GodmodeCheck.SetActive(false);
        FreeCam.SetActive(false);
        FreeCamToggle.SetActive(false);

        //audio
        SliderGO.SetActive(false);

        //visuals
        Fullscreen.SetActive(false);
        FullscreenCheck.SetActive(false);

        //credits
        HyggeLogo.SetActive(true);
        HyggeCredits.SetActive(true);
        AdditionalCredits.SetActive(true);

    }


}
