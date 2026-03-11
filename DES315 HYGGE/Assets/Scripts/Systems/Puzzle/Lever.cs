using UnityEngine;

public class Lever : Interactable, ITrackableActivator
{
    public PuzzleDoor door;
    private bool isActive = false;
    public bool IsActive => isActive;
    public Transform handlePivot;
    public Transform handlePivotOutline;

    protected override void Interact()
    {
        isActive = !isActive;
        door.ActivatorChanged();
        AnimateLever();
    }

    private void AnimateLever()
    {
        float angle = isActive ? -90 : 0;

        handlePivot.rotation = Quaternion.Euler(0, 0, angle);
        handlePivotOutline.rotation = Quaternion.Euler(0, 0, angle);
    }
}
