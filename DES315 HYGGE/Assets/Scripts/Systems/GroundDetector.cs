using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public Player player;
    public LayerMask ground;

    public int groundContacts = 0;

    [Header("Coyote Time")]
    public float coyoteTime = 0.15f;
    private float coyoteTimer;

    public bool CanJump()
    {
        return groundContacts > 0 || coyoteTimer > 0f;
    }
    public void ConsumeCoyoteTime()
    {
        coyoteTimer = 0f;
        player.OnGround = false;
    }

    private void Update()
    {
        //decrease timer if not grounded
        if (groundContacts <= 0)
        {
            coyoteTimer -= Time.deltaTime;

            if (coyoteTimer <= 0f)
            {
                player.OnGround = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if ((ground.value & (1 << other.gameObject.layer)) != 0)
        {
            groundContacts++;

            player.OnGround = true;

            //reset coyote timer when touching ground
            coyoteTimer = coyoteTime;
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

            if (groundContacts < 0)
                groundContacts = 0;

            //coyote timer will set it false
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Swap"))
        {
            player.OnSwap = false;
        }
    }
}
