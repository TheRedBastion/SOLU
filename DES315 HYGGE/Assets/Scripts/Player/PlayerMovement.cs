using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Gravity")]
    [SerializeField] private float riseGravity = 1f;
    [SerializeField] private float fallGravity = 3.5f;
    [SerializeField] private float jumpCutMultiplier = 0.5f;
    [SerializeField] private float terminalVelocity = 20f;

    [Header("Running")]
    [SerializeField] float acceleration = 60f;
    [SerializeField] float deceleration = 15f;
    //[SerializeField] float maxSpeed = 50f; //might need later for speed caps

    private Rigidbody2D rb;
    private GroundDetector groundDetector;
    private KnockbackReceiver knockback;

    private float moveSpeed;
    private float jumpForce;

    private Vector2 moveInput;
    private bool jumpPressed;
    private bool jumpReleased;

    public bool sprintActive;

    public void Init(float moveSpeed, float jumpForce)
    {
        this.moveSpeed = moveSpeed;
        this.jumpForce = jumpForce;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        groundDetector = GetComponentInChildren<GroundDetector>();
        knockback = GetComponent<KnockbackReceiver>();
    }

    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    public void JumpPressed()
    {
        if (groundDetector.CanJump())
        {
            jumpPressed = true;
            jumpReleased = false;
            groundDetector.ConsumeCoyoteTime();
        }
    }

    public void JumpReleased()
    {
        jumpReleased = true;

        if (rb.linearVelocity.y > 0)
        {
            Vector2 lv = rb.linearVelocity;
            lv.y *= jumpCutMultiplier;
            rb.linearVelocity = lv;
        }
    }

    private void FixedUpdate()
    {
        if (knockback != null && knockback.IsKnockedBack)
            return;

        Vector2 velocity = rb.linearVelocity;

        //HORIZ
        float targetVelocityX = moveInput.x * moveSpeed;

        if (sprintActive)
            targetVelocityX *= 1.5f;

        float velocityDiff = targetVelocityX - velocity.x;

        //weaker force when stopping
        float accelRate = (Mathf.Abs(targetVelocityX) > 0.01f) ? acceleration : deceleration;

        float force = velocityDiff * accelRate;

        rb.AddForce(Vector2.right * force, ForceMode2D.Force);


        //JUMPING

        //if (jumpPressed)
        //{
        //    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);  //WORKS WEIRD WITH COYOTE TIME
        //    jumpPressed = false;
        //}

        if(jumpPressed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpPressed = false;
        }

        velocity = rb.linearVelocity;


        //GRAVITY

        float baseGravity = Physics2D.gravity.y * rb.gravityScale;

        if (velocity.y > 0)
        {
            //rising
            if (!jumpReleased)
                velocity.y += baseGravity * (riseGravity - 1f) * Time.fixedDeltaTime;
            else
                velocity.y += baseGravity * (fallGravity - 1f) * Time.fixedDeltaTime;
        }
        else if (velocity.y < 0)
        {
            //falling
            velocity.y += baseGravity * (fallGravity - 1f) * Time.fixedDeltaTime;
        }


        //FALL SPEED CAP

        if (velocity.y < -terminalVelocity)
            velocity.y = -terminalVelocity;


        rb.linearVelocity = velocity;


        //FLIP

        if (velocity.x > 0.05f)
        {
            transform.localScale = new Vector3(.5f, .5f, 1);
        }
        else if (velocity.x < -0.05f)
        {
            transform.localScale = new Vector3(-.5f, .5f, 1);
        }
    }
}