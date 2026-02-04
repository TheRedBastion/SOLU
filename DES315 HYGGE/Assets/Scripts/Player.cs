using UnityEngine;

using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float jumpForce = 15f;

    public InputActionAsset InputActions;

    public int playerHealthM = 150;
    public int playerHealthS = 100;

    private InputAction moveAction;
    private InputAction jumpAction;

    //1 = moon 2 = sun
    public int Character = 1;

    private Vector2 moveAmt;

    public LayerMask GroundLayer;
    public bool OnGround;

    private Rigidbody2D rb;

    //attack
    public GameObject attackPoint;
    public float radius = 1f;
    public LayerMask enemies;
    private InputAction attackAction;

    private void OnEnable()
    {
        //rb.SetActive(false);
        InputActions.FindActionMap("Player").Enable();

    }

    private void Awake()
    {
        moveAction = InputActions.FindActionMap("Player").FindAction("Move");
        jumpAction = InputActions.FindActionMap("Player").FindAction("Jump");

        attackAction = InputActions.FindActionMap("Player").FindAction("Attack");

        rb = GetComponent<Rigidbody2D>();
    }

    public void RefreshInput()
    {
        moveAction = InputActions.FindActionMap("Player").FindAction("Move");
        jumpAction = InputActions.FindActionMap("Player").FindAction("Jump");

        attackAction = InputActions.FindActionMap("Player").FindAction("Attack");

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

        if (Character == 1)
        {
            if (playerHealthM <= 0)
            {
                Destroy(gameObject);
                //Implement death animation and menu for respawn later
            }
        }
        else if (Character == 2)
        {
            if (playerHealthS <= 0)
            {
                Destroy(gameObject);
                //Implement death animation and menu for respawn later
            }

        }

       

        if (attackAction.WasPressedThisFrame())
        {
            attack();
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

    public void attack()
    {
        Collider2D[] enemy = Physics2D.OverlapCircleAll(attackPoint.transform.position, radius, enemies);

        foreach (Collider2D enemyGameobject in enemy)
        {
            Debug.Log("Hit enemy");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.transform.position, radius); 
    }

    public enum PlayerType
    {
        Moon = 0,
        Sun = 1
    }
}
