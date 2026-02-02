using UnityEngine;

using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float jumpForce = 15f;

    public InputActionAsset InputActionsM;
    public InputActionAsset InputActionsS;

    public int playerHealth;

    private InputAction moveAction;
    private InputAction jumpAction;

    //1 = moon 2 = sun
    public int Character;

    private Vector2 moveAmt;

    public LayerMask GroundLayer;
    public bool OnGround;

    private Rigidbody2D rb;

    private void OnEnable()
    {
        //rb.SetActive(false);

        if (Character == 1)
        {
            InputActionsM.FindActionMap("Player").Enable();
            playerHealth = 150;
            //rb.SetActive(true);
        }
        else
        {
            InputActionsS.FindActionMap("Player2").Enable();
            playerHealth = 100;
        }
    }

    private void Awake()
    {
        if (Character == 1)
        {
            moveAction = InputActionsM.FindActionMap("Player").FindAction("Move");
            jumpAction = InputActionsM.FindActionMap("Player").FindAction("Jump");
        }
        else
        {
            moveAction = InputActionsS.FindActionMap("Player2").FindAction("Move");
            jumpAction = InputActionsS.FindActionMap("Player2").FindAction("Jump");
        }

        rb = GetComponent<Rigidbody2D>();
    }

    public void RefreshInput()
    {
        if (Character == 1)
        {
            moveAction = InputActionsM.FindActionMap("Player").FindAction("Move");
            jumpAction = InputActionsM.FindActionMap("Player").FindAction("Jump");
        }
        else if (Character == 2)
        {
            moveAction = InputActionsS.FindActionMap("Player2").FindAction("Move");
            jumpAction = InputActionsS.FindActionMap("Player2").FindAction("Jump");
        }
    }

    void Start()
    {
        OnGround = true;
    }

    void Update()
    {
        moveAmt = moveAction.ReadValue<Vector2>();

        if (jumpAction.WasPressedThisFrame() && OnGround == true)
        {
            Jump();
        }

        if (playerHealth <= 0)
        {
            Destroy(gameObject);
            //Implement death animation and menu for respawn later
        }

    }

    public void Jump()
    {
        Vector2 lv = rb.linearVelocity;
        lv.y = jumpForce;
        rb.linearVelocity = lv;
        OnGround = false;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if((GroundLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            OnGround = true;
        }
    }

    private void FixedUpdate()
    {
        Vector2 lv = rb.linearVelocity;
        lv.x = moveAmt.x * moveSpeed;
        rb.linearVelocity = lv;
        if (lv.x > 0)
        {
            transform.localScale = new Vector3(.5f, .5f, 1);
        }
        else if (lv.x < 0)
        {
            transform.localScale = new Vector3(-.5f, .5f, 1);
        }
    }

}
