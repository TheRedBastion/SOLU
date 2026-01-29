using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSwap : MonoBehaviour
{
    public Transform character;
    public List<Transform> characterOptions;
    public int currentCharacter;

    public InputActionAsset InputActions;
    private InputAction swapAction;

    public float swapCooldown = 5f;
    private float canSwap = -1f;


    private void Awake()
    {
        swapAction = InputActions.FindActionMap("Player").FindAction("Interact");
        
    }
    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
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
        NewMonoBehaviourScript move = characterOptions[currentCharacter].GetComponentInChildren<NewMonoBehaviourScript>();

        if (swapAction.WasPressedThisFrame() && move.OnGround == true && Time.time >= canSwap)
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
            
        }

        
    }

    public void Swap()
    {
        int other = (currentCharacter == 0) ? 1 : 0;

        characterOptions[currentCharacter].gameObject.SetActive(true);
        characterOptions[other].gameObject.SetActive(false);

        //PlayerInput input = characterOptions[currentCharacter].GetComponentInChildren<PlayerInput>();

        //if (input != null)
        //{
        //    input.enabled = true;
        //}

        NewMonoBehaviourScript move = characterOptions[currentCharacter].GetComponentInChildren<NewMonoBehaviourScript>();

        if (move != null)
        {
            move.enabled = true;
            move.RefreshInput();
        }

        canSwap = Time.time + swapCooldown;
    }

}
