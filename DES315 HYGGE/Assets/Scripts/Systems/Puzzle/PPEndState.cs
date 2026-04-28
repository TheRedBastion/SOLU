using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PPEndState : MonoBehaviour
{
    public List<MonoBehaviour> activators = new List<MonoBehaviour>();

    private bool isOpen = false;

    public void ActivatorChanged()
    {
        if (isOpen) return;

        foreach (var a in activators)
        {
            var trackable = a as ITrackableActivator;
            if (trackable != null && !trackable.IsActive)
                return;
        }

        toEndState();//all activators
    }

    private void toEndState()
    {
        SceneManager.LoadScene(4);
    }
}
