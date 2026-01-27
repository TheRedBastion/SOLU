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
        InputActions.FindActionMap("Player").Disable();
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

    //public void Swap()
    //{


    //    character = characterOptions[currentCharacter];
    //    character.GetComponent<PlayerInput>().enabled = true;
    //    for (int i = 0; i < characterOptions.Count; i++)
    //    {
    //        if (characterOptions[i] != character)
    //        {
    //            characterOptions[i].GetComponent<PlayerInput>().enabled = false;
    //            characterOptions[i].GetComponent<NewMonoBehaviourScript>().enabled = false;
    //        }
    //    }
    //}

    public void Swap()
    {
        int other;
        character = characterOptions[currentCharacter];

        if (currentCharacter == 0)
        {
            other = 1;
        }
        else
        {
            other = 0;
        }


            bool isActive = (characterOptions[currentCharacter] == character);
            PlayerInput input = characterOptions[currentCharacter].GetComponentInChildren<PlayerInput>();
            if (input != null)
            {
                input.enabled = isActive;
            }
            NewMonoBehaviourScript move = characterOptions[currentCharacter].GetComponentInChildren<NewMonoBehaviourScript>();
            if (move != null)
            {
                move.enabled = isActive;
            }

            PlayerInput otherInput = characterOptions[other].GetComponentInChildren<PlayerInput>();
            if (otherInput != null)
            {
                otherInput.enabled = false;
            }

            NewMonoBehaviourScript otherMove = characterOptions[other].GetComponentInChildren<NewMonoBehaviourScript>();
            if (otherMove != null)
            {
                otherMove.enabled = false;
            }
        if (move != null && isActive)
        { 
            move.RefreshInput(); 
        }

    }

}
