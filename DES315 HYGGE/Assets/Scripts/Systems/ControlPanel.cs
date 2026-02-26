using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    public static ControlPanel Instance;

    [Header("Audio Settings")]
    public bool audioEnabled = true;

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

    private void Update()
    {
        //apply audio state continuously in case it changes at runtime
        ApplyAudioState();
    }

    public void SetAudioEnabled(bool enabled)
    {
        audioEnabled = enabled;
        ApplyAudioState();
    }

    private void ApplyAudioState()
    {
        AudioListener.pause = !audioEnabled;
    }
}
