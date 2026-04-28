using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour, ITrackableActivator
{
    [SerializeField] private bool isEnd = false;
    public List<PuzzleDoor> doors = new List<PuzzleDoor>();
    public List<PPEndState> ending = new List<PPEndState>();
    
    private bool isActive = false;
    public bool IsActive => isActive;

    private Vector3 originalPos;

    private void Awake()
    {
        originalPos = transform.localPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive && other.CompareTag("Player"))
        {
            isActive = true;
            foreach (var a in doors)
            {
                a.ActivatorChanged();
            }

            if (isEnd)
            {
                foreach (var a in ending)
                {
                    a.ActivatorChanged();
                }
            }
            AnimatePlate(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        GameObject otherGO = other.gameObject;
        if (isActive && other.CompareTag("Player") && otherGO.activeInHierarchy)
        {
            isActive = false;
            foreach (var a in doors)
            {
                a.ActivatorChanged();
            }

            if (isEnd)
            {
                foreach (var a in ending)
                {
                    a.ActivatorChanged();
                }
            }
            AnimatePlate(false);
        }
    }

    private void AnimatePlate(bool pressed)
    {
        if (pressed)
            transform.localPosition += Vector3.down * 0.1f;
        else
            transform.localPosition = originalPos;
    }
}
