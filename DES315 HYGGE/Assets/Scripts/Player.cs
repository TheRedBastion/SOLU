using System.Collections;
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
    private GameObject activeObject;

    public InputActionAsset InputActions;
    private InputAction moveAction;
    private InputAction jumpAction;

    public PlayerStats moonStats = new PlayerStats(150, 150, 5f, 15f);
    public PlayerStats sunStats = new PlayerStats(100, 100, 7f, 12f);

    private PlayerStats currentStats;

    //0 = moon 1 = sun
    public int character = 0;

    private Vector2 moveAmt;

    public bool OnGround;

    private Rigidbody2D rb;

    //combat
    private PlayerCombat combat;
    private InputAction attackAction;

    //health
    private Health currentHealth;

    private void Awake()
    {
        RefreshInput();
        SetActiveCharacter(character);
        combat = GetComponent<PlayerCombat>();
    }

    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        currentHealth.OnDamageTaken.RemoveListener(HandleDamage);
        InputActions.FindActionMap("Player").Disable();
    }

    public void RefreshInput()
    {
        var playerMap = InputActions.FindActionMap("Player");
        moveAction = playerMap.FindAction("Move");
        jumpAction = playerMap.FindAction("Jump");
        attackAction = playerMap.FindAction("Attack");
    }

    public void SetActiveCharacter(int newCharacter)
    {
        if (currentHealth != null)
            currentHealth.OnDamageTaken.RemoveListener(HandleDamage);

        character = newCharacter;

        activeObject = (character == 0) ? MoonObject : SunObject;
        currentStats = (character == 0) ? moonStats : sunStats;

        currentHealth = activeObject.GetComponent<Health>();
        currentHealth.OnDamageTaken.AddListener(HandleDamage);

        rb = activeObject.GetComponent<Rigidbody2D>();
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
            Destroy(activeObject);
        }

        if (attackAction.WasPressedThisFrame())
        {
            combat.Attack();
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

    public Transform GetActiveCharacterTransform()
    {
        return activeObject.transform;
    }

    private void HandleDamage(int damage)
    {
        currentStats.health = currentHealth.CurrentHealth;
    }

    public enum PlayerType
    {
        Moon = 0,
        Sun = 1
    }
}
