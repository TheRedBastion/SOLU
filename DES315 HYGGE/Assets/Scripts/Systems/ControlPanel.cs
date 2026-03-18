using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    public static ControlPanel Instance;

    [Header("Audio Settings")]
    public bool audioEnabled = true;
    public string masterVolumeRTPC = "Master_Volume_Control";

    private void OnValidate()
    {
        if (!Application.isPlaying) return;

        ApplyAudioState();
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
    }

    public void SetAudioEnabled(bool enabled)
    {
        audioEnabled = enabled;
        ApplyAudioState();
    }

    private void ApplyAudioState()
    {
        float volume = audioEnabled ? 100f : 0f;
        AkUnitySoundEngine.SetRTPCValue(masterVolumeRTPC, volume);

        AudioListener.pause = !audioEnabled;
    }
}
