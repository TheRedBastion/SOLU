using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Gravity")]
    [SerializeField] private float riseGravity = 1f;
    [SerializeField] private float fallGravity = 3.5f;
    [SerializeField] private float jumpCutMultiplier = 0.5f;
    [SerializeField] private float terminalVelocity = 20f;

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

        Vector2 lv = rb.linearVelocity;

        //horizontal
        lv.x = moveInput.x * moveSpeed;

        if (sprintActive)
            lv.x *= 1.5f;

        //jump
        if (jumpPressed)
        {
            lv.y = jumpForce;
            jumpPressed = false;
        }

        float baseGravity = Physics2D.gravity.y * rb.gravityScale;

        if (lv.y > 0)
        {
            if (!jumpReleased)
                lv.y += baseGravity * (riseGravity - 1) * Time.fixedDeltaTime;
            else
                lv.y += baseGravity * (fallGravity - 1) * Time.fixedDeltaTime;
        }
        else if (lv.y < 0)
        {
            lv.y += baseGravity * (fallGravity - 1) * Time.fixedDeltaTime;
        }

        //clamp fall speed
        if (lv.y < -terminalVelocity)
            lv.y = -terminalVelocity;

        rb.linearVelocity = lv;

        //flip
        if (lv.x > 0)
        {
            rb.transform.localScale = new Vector3(.5f, .5f, 1);
        }
        else if (lv.x < 0)
        {
            rb.transform.localScale = new Vector3(-.5f, .5f, 1);
        }
    }
}