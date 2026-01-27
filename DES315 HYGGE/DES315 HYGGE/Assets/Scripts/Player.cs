using UnityEngine;

using UnityEngine.InputSystem;


public class NewMonoBehaviourScript : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float jumpForce = 15f;

    public InputActionAsset InputActionsM;
    public InputActionAsset InputActionsS;
    

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
            //rb.SetActive(true);
        }
        else
        {
            InputActionsS.FindActionMap("Player2").Enable();
        }
    }

    private void OnDisable()
    {
        if (Character == 1)
        {
            //InputActionsM.FindActionMap("Player").Disable();
        }
        else
        {
            //InputActionsS.FindActionMap("Player2").Disable();
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
        //m_animator = GetComponent<Animator>();
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

        //if(m_moveAction.IsPressed())
        //{
        //    //m_animator.SetBool("isWalking", true);
        //    Walking();
        //}
        //else
        //{
        //    //m_animator.SetBool("isWalking", false);
        //}
    }

    public void Jump()
    {
        Vector2 lv = rb.linearVelocity;
        lv.y = jumpForce;
        rb.linearVelocity = lv;
        OnGround = false;
        //m_animator.SetTrigger("Jump");
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
    }

}
