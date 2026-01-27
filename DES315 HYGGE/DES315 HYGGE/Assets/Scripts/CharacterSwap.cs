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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (character == null && characterOptions.Count >= 1)
        {
            character = characterOptions[0];
        }
        Swap();
    }

    // Update is called once per frame
    void Update()
    {
        if (swapAction.WasPressedThisFrame())
        {
            if (currentCharacter == 0)
            {
                //currentCharacter = characterOptions.Count - 1;
                currentCharacter = 1;
                Swap();
            }
            else
            {
                //currentCharacter -= 1;
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

        PlayerInput input = characterOptions[currentCharacter].GetComponentInChildren<PlayerInput>();

        if (input != null)
        {
            input.enabled = true;
        }

        NewMonoBehaviourScript move = characterOptions[currentCharacter].GetComponentInChildren<NewMonoBehaviourScript>();

        if (move != null)
        {
            move.enabled = true;
            move.RefreshInput();
        }
    }

}
