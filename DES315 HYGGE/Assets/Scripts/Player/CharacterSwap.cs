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

    public GameObject MoonLights;
    public GameObject SunLights;
    private float canSwap = -1f;

    private float onetime = 2;

    public bool swappedThisFrame { get; private set; } //child can read it, only parent can write it
    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }

    private void Awake()
    {
        swapAction = InputActions.FindActionMap("Player").FindAction("Swap");
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
        if(onetime == 2)
        {
            currentCharacter = 1 - currentCharacter; //toggle between 0 and 1 (math trick)
            Swap();
            //swappedThisFrame = true;
            onetime = 1;
        }
        else if (onetime == 1)
        {
            currentCharacter = 1 - currentCharacter; //toggle between 0 and 1 (math trick)
            Swap();
            swappedThisFrame = true;
            onetime = 0;
        }

    }

    public void Swap()
    {
        int other = 1 - currentCharacter;

        character = characterOptions[currentCharacter];

        characterOptions[currentCharacter].gameObject.SetActive(true);
        characterOptions[other].gameObject.SetActive(false);

        if (currentCharacter == 0)
        {
            SunLights.SetActive(false);
        }
        else if (currentCharacter == 1)
        {
            MoonLights.SetActive(false);
        }

        if (player != null)
        {
            player.SetActiveCharacter(currentCharacter);
        }

        canSwap = Time.time + swapCooldown;

        if (currentCharacter == 0)
        {
            MoonLights.SetActive(true);
        }
        else if (currentCharacter == 1)
        {
            SunLights.SetActive(true);
        }

        //healthTracker.DrawHearts(0); causes null ref??
    }

}
