using System.Collections;
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public bool isGodmode;

    public GameObject MoonObject;
    public GameObject SunObject;
    private GameObject activeObject;

    public InputActionAsset InputActions;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;

    public PlayerStats moonStats = new PlayerStats(150, 150, 5f, 15f);
    public PlayerStats sunStats = new PlayerStats(100, 100, 7f, 12f);

    private PlayerStats currentStats;

    //0 = moon 1 = sun
    public int character = 0;

    private Vector2 moveAmt;

    public bool OnGround;
    public bool OnSwap;

    public bool sprintActive = false;

    //combat
    private PlayerCombat combat;
    private InputAction attackAction;

    private KnockbackReceiver knockback;
    //health
    private Health currentHealth;

    //movement
    private PlayerMovement movement;

    public Stamina stamina;
    public float curStam;

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
        sprintAction = playerMap.FindAction("Sprint");

    }

    public void SetActiveCharacter(int newCharacter)
    {
        if (currentHealth != null)
        {
            currentHealth.ResetInvincibility();
            currentHealth.OnDamageTaken.RemoveListener(HandleDamage);
        }

        character = newCharacter;

        activeObject = (character == 0) ? MoonObject : SunObject;
        currentStats = (character == 0) ? moonStats : sunStats;

        movement = activeObject.GetComponent<PlayerMovement>();
        currentHealth = activeObject.GetComponent<Health>();
        knockback = activeObject.GetComponent<KnockbackReceiver>();

        movement.Init(currentStats.moveSpeed, currentStats.jumpForce);

        currentHealth.OnDamageTaken.AddListener(HandleDamage);

        curStam = stamina.currentStamina;
    }

    void Start()
    {
        OnGround = true;
        //OnSwap = false;
    }

    void Update()
    {
        //Debug.Log(curStam);
        moveAmt = moveAction.ReadValue<Vector2>();
        movement.SetMoveInput(moveAmt);

        if (jumpAction.WasPressedThisFrame() && OnGround)
            movement.JumpPressed();

        if (jumpAction.WasReleasedThisFrame())
            movement.JumpReleased();

        if (attackAction.WasPressedThisFrame())
            combat.Attack();

        HandleSprint();

        movement.sprintActive = sprintActive;

        if (currentHealth.CurrentHealth <= 0)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadScene(2);
            Debug.Log("Player has died.");
        }
    }

    void HandleSprint()
    {
        if (sprintAction.IsPressed() && curStam > 0)
        {
            sprintActive = true;

            if (!isGodmode)
                curStam -= stamina.lossRate * Time.deltaTime;
        }
        else
        {
            sprintActive = false;

            if (curStam < stamina.maxStamina)
                curStam += stamina.regenRate * Time.deltaTime;
        }

        curStam = Mathf.Clamp(curStam, 0, stamina.maxStamina);
    }

    public Transform GetActiveCharacterTransform()
    {
        return activeObject.transform;
    }

    private void HandleDamage(int damage, KnockbackData hitDirection)
    {
        currentStats.health = currentHealth.CurrentHealth;

        knockback?.ApplyKnockback(hitDirection);
    }
}
