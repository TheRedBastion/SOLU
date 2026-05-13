using UnityEngine;

public static class gamevar
{
    public static float MasterValueFloat = 100.0f;
    public static float MusicVolumeFloat = 100.0f;
    public static float SFXVolumeFloat = 100.0f;
    public static bool GodModeToggle = false;
    public static bool FreeCamToggle = false;

    public static void Save()
    {
        PlayerPrefs.SetFloat("MasterValueFloat", MasterValueFloat);
        PlayerPrefs.SetFloat("MusicVolumeFloat", MusicVolumeFloat);
        PlayerPrefs.SetFloat("SFXVolumeFloat", SFXVolumeFloat);

        PlayerPrefs.SetInt("GodModeToggle", GodModeToggle ? 1 : 0);
        PlayerPrefs.SetInt("FreeCamToggle", FreeCamToggle ? 1 : 0);

        PlayerPrefs.Save();
    }

    public static void Load()
    {
        MasterValueFloat = PlayerPrefs.GetFloat("MasterValueFloat", 100.0f);
        MusicVolumeFloat = PlayerPrefs.GetFloat("MusicVolumeFloat", 100.0f);
        SFXVolumeFloat = PlayerPrefs.GetFloat("SFXVolumeFloat", 100.0f);

        GodModeToggle = PlayerPrefs.GetInt("GodModeToggle", 0) == 1;
        FreeCamToggle = PlayerPrefs.GetInt("FreeCamToggle", 0) == 1;
    }
}
