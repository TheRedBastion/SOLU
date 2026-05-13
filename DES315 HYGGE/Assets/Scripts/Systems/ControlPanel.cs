using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    public static ControlPanel Instance;

    [Header("Audio Settings")]
    public bool audioEnabled = true;
    public string masterVolumeRTPC = "Master_Volume_Control";
    public string musicVolumeRTPC = "Music_Volume_Control";
    public string SFXVolumeRTPC = "Sound_Effects_Volume_Control";

    [Header("Gameplay Settings")]
    public bool doorsEnabled = true;
    public GameObject doors;

    private void OnValidate()
    {
        if (!Application.isPlaying) return;

        ApplyAudioState();
        ApplyDoors();
    }

    private void Awake()
    {
        //singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        ApplyAudioState();
        ApplyDoors();
    }

    public void SetAudioEnabled(bool enabled)
    {
        audioEnabled = enabled;
        ApplyAudioState();
    }

    private void ApplyAudioState()
    {

        AkUnitySoundEngine.SetRTPCValue(masterVolumeRTPC, gamevar.MasterValueFloat);
        AkUnitySoundEngine.SetRTPCValue(musicVolumeRTPC, gamevar.MusicVolumeFloat);
        AkUnitySoundEngine.SetRTPCValue(SFXVolumeRTPC, gamevar.SFXVolumeFloat);
        AudioListener.pause = !audioEnabled;

#if UNITY_EDITOR
        AkUnitySoundEngine.SetRTPCValue(masterVolumeRTPC, audioEnabled ? 100 : 0);
        
#endif
    }

    private void ApplyDoors()
    {
        if (doors != null)
        {
            doors.SetActive(doorsEnabled);
        }
    }
}
