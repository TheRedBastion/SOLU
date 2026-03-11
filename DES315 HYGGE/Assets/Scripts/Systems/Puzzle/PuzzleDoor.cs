using UnityEngine;
using System.Collections.Generic;

public class PuzzleDoor : MonoBehaviour
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
                return;//one inactive door stays closed
        }

        OpenDoor();//all activators active open forever
    }

    private void OpenDoor()
    {
        isOpen = true;
        gameObject.SetActive(false);
    }
}
