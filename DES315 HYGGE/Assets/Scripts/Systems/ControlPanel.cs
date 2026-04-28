using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    public static ControlPanel Instance;

    [Header("Audio Settings")]
    public bool audioEnabled = true;
    public string masterVolumeRTPC = "Master_Volume_Control";

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

        AudioListener.pause = !audioEnabled;
    }

    private void ApplyDoors()
    {
        if (doors != null)
        {
            doors.SetActive(doorsEnabled);
        }
    }
}
