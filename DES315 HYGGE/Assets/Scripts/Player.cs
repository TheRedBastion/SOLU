using UnityEngine;

using UnityEngine.InputSystem;



public class NewMonoBehaviourScript : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float jumpForce = 15f;

    public InputActionAsset InputActions;

    private InputAction m_moveAction;
    private InputAction m_jumpAction;

    private Vector2 m_moveAmt;

    public LayerMask GroundLayer;
    public bool OnGround;

    private Rigidbody2D rb;

    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }

    private void Awake()
    {
        m_moveAction = InputActions.FindActionMap("Player").FindAction("Move");
        m_jumpAction = InputActions.FindActionMap("Player").FindAction("Jump");
        rb = GetComponent<Rigidbody2D>();
        //m_animator = GetComponent<Animator>();
    }

    void Start()
    {
        OnGround = true;
    }

    void Update()
    {
        m_moveAmt = m_moveAction.ReadValue<Vector2>();

        if (m_jumpAction.WasPressedThisFrame() && OnGround == true)
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
        lv.x = m_moveAmt.x * moveSpeed;
        rb.linearVelocity = lv;
    }

}
