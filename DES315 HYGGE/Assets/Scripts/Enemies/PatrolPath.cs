using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    public Vector2 startPosition, endPosition;

    void Reset()
    {
        startPosition = Vector3.left;
        endPosition = Vector3.right;
    }
}
