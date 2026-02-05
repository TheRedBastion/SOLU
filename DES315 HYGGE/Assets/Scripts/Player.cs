using UnityEngine;

using UnityEngine.InputSystem;

[System.Serializable]
public class PlayerStats
{
    public int health;
    public int maxHealth;
    public float moveSpeed;
    public float jumpForce;
    public PlayerStats(int health, int maxHealth, float moveSpeed, float jumpForce)
    {
        this.health = health;
        this.maxHealth = maxHealth;
        this.moveSpeed = moveSpeed;
        this.jumpForce = jumpForce;
    }
}

public class Player : MonoBehaviour
{

    private float moveSpeed;
    private float jumpForce;

    public GameObject MoonObject;
    public GameObject SunObject;

    public InputActionAsset InputActions;
    private InputAction moveAction;
    private InputAction jumpAction;

    public PlayerStats moonStats = new PlayerStats(150, 150, 5f, 15f);
    public PlayerStats sunStats = new PlayerStats(100, 100, 7f, 12f);

    private PlayerStats currentStats;

    //0 = moon 1 = sun
    public int character = 0;

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

        if (character == 0)
        {
            currentStats = moonStats;
            rb = MoonObject.GetComponent<Rigidbody2D>();
        }
        else if (character == 1)
        {
            currentStats = sunStats;
            rb = SunObject.GetComponent<Rigidbody2D>();
        }
        //rb = GetComponent<Rigidbody2D>();
        ApplyStats();
    }

    public void RefreshInput()
    {
        moveAction = InputActions.FindActionMap("Player").FindAction("Move");
        jumpAction = InputActions.FindActionMap("Player").FindAction("Jump");

        attackAction = InputActions.FindActionMap("Player").FindAction("Attack");
        if (character == 0)
        {
            currentStats = moonStats;
            rb = MoonObject.GetComponent<Rigidbody2D>();
        }
        else if (character == 1)
        {
            currentStats = sunStats;
            rb = SunObject.GetComponent<Rigidbody2D>();
        }

        ApplyStats();
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

        if (currentStats.health <= 0)
        {
            Destroy(character == 0 ? MoonObject : SunObject);
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

    private void FixedUpdate()
    {
        GameObject currentGameObj = (character == 0) ? MoonObject : SunObject;

        Vector2 lv = rb.linearVelocity;
        lv.x = moveAmt.x * moveSpeed;
        rb.linearVelocity = lv;

        if (lv.x > 0)
        {
            rb.transform.localScale = new Vector3(.5f, .5f, 1);
        }
        else if (lv.x < 0)
        {
            rb.transform.localScale = new Vector3(-.5f, .5f, 1);
        }
    }
    private void ApplyStats()
    {
        moveSpeed = currentStats.moveSpeed;
        jumpForce = currentStats.jumpForce;
    }

    public void TakeDamage(int damage)
    {
        if (character == 0)
        {
            moonStats.health -= damage;
            Debug.Log("Moon health: " + moonStats.health);
        }
        else
        {
            sunStats.health -= damage;
            Debug.Log("Sun health: " + sunStats.health);
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
