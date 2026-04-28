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

    private Quaternion initialLocalRot;

    public AK.Wwise.Event LeverFlipped = new AK.Wwise.Event();

    private void Awake()
    {
        if (handlePivot != null)
            initialLocalRot = handlePivot.localRotation;
    }

    protected override void Interact()
    {
        isActive = !isActive;
        foreach (var a in doors)
        {
            a.ActivatorChanged();
            LeverFlipped.Post(gameObject);
        }
        AnimateLever();
    }

    private void AnimateLever()
    {
        if (handlePivot == null) return;

        float targetAngle = isActive ? -90f : 0f;
        Quaternion targetRot = initialLocalRot * Quaternion.Euler(0, 0, targetAngle);

        handlePivot.localRotation = targetRot;
        if (handlePivotOutline != null)
            handlePivotOutline.localRotation = targetRot;
    }
}
