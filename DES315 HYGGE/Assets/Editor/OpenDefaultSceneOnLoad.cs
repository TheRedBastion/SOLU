using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class OpenDefaultSceneOnLoad
{
    static OpenDefaultSceneOnLoad()
    {
        if (!EditorApplication.isPlayingOrWillChangePlaymode)
        {
            string scenePath = "Assets/Scenes/SampleScene.unity";

            //Only open if no scene is loaded
            if (EditorSceneManager.GetActiveScene().path != scenePath)
            {
                EditorSceneManager.OpenScene(scenePath);
            }
        }
    }
}
