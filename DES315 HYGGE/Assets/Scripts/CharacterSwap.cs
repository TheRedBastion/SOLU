using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSwap : MonoBehaviour
{
    public Transform character;
    public List<Transform> characterOptions;
    public int currentCharacter = 0;

    public Player player;

    public InputActionAsset InputActions;
    private InputAction swapAction;

    public float swapCooldown = 5f;
    private float canSwap = -1f;

    public bool swappedThisFrame { get; private set; } //child can read it, only parent can write it
    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }

    private void Awake()
    {
        swapAction = InputActions.FindActionMap("Player").FindAction("Interact");
        
    }

    private void OnDisable()
    {
        //InputActions.FindActionMap("Player").Disable();
    }
    void Start()
    {
        if (character == null && characterOptions.Count >= 1)
        {
            character = characterOptions[0];
        }
        Swap();
    }

    void Update()
    {
        swappedThisFrame = false;

        if (swapAction.WasPressedThisFrame() && player.OnGround == true && Time.time >= canSwap)
        {
            if (currentCharacter == 0)
            {
                currentCharacter = 1;
                Swap();
            }
            else
            {
                currentCharacter = 0;
                Swap();

            }
            swappedThisFrame = true;
        }

        
    }

    public void Swap()
    {
        int other = (player.character == 0) ? 1 : 0;

        character = characterOptions[currentCharacter];

        if (currentCharacter == 1)
        {
            player.SunObject.SetActive(true);
            player.MoonObject.SetActive(false);
        }
        else
        {
            player.MoonObject.SetActive(true);
            player.SunObject.SetActive(false);
        }

        //characterOptions[currentCharacter].gameObject.SetActive(true);
        //characterOptions[other].gameObject.SetActive(false);

        if (player != null)
        {
            player.SetActiveCharacter(currentCharacter);
        }

        canSwap = Time.time + swapCooldown;
    }

}
