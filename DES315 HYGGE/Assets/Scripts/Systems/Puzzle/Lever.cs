using System;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable, ITrackableActivator
{
    public List<PuzzleDoor> doors = new List<PuzzleDoor>();
    private bool isActive = false;
    public bool IsActive => isActive;
    public Transform handlePivot;
    public Transform handlePivotOutline;

    protected override void Interact()
    {
        isActive = !isActive;
        foreach (var a in doors)
        {
            a.ActivatorChanged();
        }
        AnimateLever();
    }

    private void AnimateLever()
    {
        float angle = isActive ? -90 : 0;

        handlePivot.rotation = Quaternion.Euler(0, 0, angle);
        handlePivotOutline.rotation = Quaternion.Euler(0, 0, angle);
    }
}
