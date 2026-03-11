using UnityEngine;

public class UIFollow : MonoBehaviour
{
    private Player player;
    public Vector3 offset;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    void LateUpdate()
    {
        transform.position = player.GetActiveCharacterTransform().position + offset;
    }
}
