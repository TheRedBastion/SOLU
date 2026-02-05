using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public LayerMask groundLayer;
    public Player player;

    private int contacts;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsGround(collision.gameObject))
            return;

        contacts++;
        player.OnGround = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!IsGround(collision.gameObject))
            return;

        contacts--;
        if (contacts <= 0)
            player.OnGround = false;
    }

    private bool IsGround(GameObject obj)
    {
        return (groundLayer.value & (1 << obj.layer)) != 0;
    }
}
