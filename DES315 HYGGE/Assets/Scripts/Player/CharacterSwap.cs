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
    //public HealthTracker healthTracker;

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
    }

    void Update()
    {
        swappedThisFrame = false;

        if (swapAction.WasPressedThisFrame() && player.OnSwap == true && Time.time >= canSwap)
        {
            currentCharacter = 1 - currentCharacter; //toggle between 0 and 1 (math trick)
            Swap();
            swappedThisFrame = true;
        }

    }

    public void Swap()
    {
        int other = 1 - currentCharacter;

        character = characterOptions[currentCharacter];

        characterOptions[currentCharacter].gameObject.SetActive(true);
        characterOptions[other].gameObject.SetActive(false);

        if (player != null)
        { 
            player.SetActiveCharacter(currentCharacter);
        }

        canSwap = Time.time + swapCooldown;

        //healthTracker.DrawHearts(0); causes null ref??
    }

}
