using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private string interactionText = "[FILL IN FIELD]";
    [SerializeField] private GameObject OutlineGO;//messed around with shader for outline, doesnt f work, maybe later
    public InputActionAsset InputActions;
    private InputAction interactAction;

    [SerializeField] private Sprite[] sprites;
    private SpriteRenderer spriteR;
    private bool playerInRange;
    private bool isInteracting;
    private CharacterUI currUI;

    private void Awake()
    {
        spriteR = GetComponent<SpriteRenderer>();
        interactAction = InputActions.FindActionMap("Player").FindAction("Interact");
    }

    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }


    void Update()
    {
        if (playerInRange && !isInteracting && interactAction.WasPressedThisFrame())
        {
            Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponentInParent<Player>())
        {
            playerInRange = true;
            currUI = other.GetComponent<CharacterUI>();

            currUI.pressEUI.SetActive(true);
            SetOutline(true);
            if(sprites.Length > 0)
                spriteR.sprite = sprites[1];
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponentInParent<Player>())
        {
            playerInRange = false;
            isInteracting = false;
            currUI = other.GetComponent<CharacterUI>();

            currUI.pressEUI.SetActive(false);
            currUI.dialogueUI.SetActive(false);
            SetOutline(false);
            if (sprites.Length > 0)
                spriteR.sprite = sprites[0];
        }
    }

    protected virtual void Interact()
    {
        isInteracting = true;

        currUI.pressEUI.SetActive(false);
        currUI.dialogueUI.SetActive(true);
        currUI.dialogueText.text = interactionText;
    }

    void SetOutline(bool enabled)
    {
        if (OutlineGO != null)
        {
            OutlineGO.SetActive(enabled);
        }
    }
}