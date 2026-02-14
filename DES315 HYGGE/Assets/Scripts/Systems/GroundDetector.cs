using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public Player player;
    public LayerMask ground;

    private int groundContacts = 0;
    //TODO: when going to higher surface its kinda buggy need to look at this
    private void OnCollisionEnter2D(Collision2D other)
    {
        if ((ground.value & (1 << other.gameObject.layer)) != 0)
        {
            groundContacts++;
            player.OnGround = true;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Swap"))
        {
            player.OnSwap = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if ((ground.value & (1 << other.gameObject.layer)) != 0)
        {
            groundContacts--;

            if (groundContacts <= 0)
            {
                groundContacts = 0;
                player.OnGround = false;
            }
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Swap"))
        {

            player.OnSwap = false;
            
        }
    }
}
