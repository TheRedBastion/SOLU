using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public Player player;
    public LayerMask ground;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((ground.value & (1 << other.gameObject.layer)) != 0)
            player.OnGround = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if ((ground.value & (1 << other.gameObject.layer)) != 0)
            player.OnGround = false;
    }
}
