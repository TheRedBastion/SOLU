using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public Player player;
    public LayerMask ground;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((ground.value & (1 << other.gameObject.layer)) != 0)
        {
            player.OnGround = true;
        }

        //The way we did it for ground wasnt working here so i just hard coded the layer name, not sure why it didnt work for swap
        if (other.gameObject.layer == LayerMask.NameToLayer("Swap"))
        {
            player.OnSwap = true;
        }

    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if ((ground.value & (1 << other.gameObject.layer)) != 0)
        {
            player.OnGround = false;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Swap"))
        {
            player.OnSwap = false;
        }

    }
}
