using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    [Header("Stamina Values")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float currentStamina = 100f;
    [SerializeField] private float sprintLockThreshold = 10f;

    [Header("Rates")]
    public float lossRate = 5f;
    public float regenRate = 5f;

    [Header("UI")]
    [SerializeField] private Image staminaBarUI = null;
    [SerializeField] private CanvasGroup staminaBarCanvasGroup = null;

    private Player player;
    private bool sprintLocked = false;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        currentStamina = maxStamina;
    }

    private void OnEnable()
    {
        currentStamina = maxStamina;
    }
    private void OnDisable()
    {
        currentStamina = maxStamina;
    }

    private void Update()
    {
        if (player == null || player.isGodmode) return;

        HandleStamina();
        UpdateUI();
    }

    private void HandleStamina()
    {
        if (currentStamina <= 0f)
        {
            sprintLocked = true;
        }

        if (sprintLocked && currentStamina >= sprintLockThreshold)
        {
            sprintLocked = false;
        }

        if (sprintLocked)
        {
            player.sprintActive = false;
        }

        if (player.sprintActive && !sprintLocked)
        {
            currentStamina -= lossRate * Time.deltaTime;

            if (currentStamina < 0f)
                currentStamina = 0f;
        }
        else
        {
            if (currentStamina < maxStamina)
            {
                currentStamina += regenRate * Time.deltaTime;
                if (currentStamina > maxStamina)
                    currentStamina = maxStamina;
            }
        }
    }

    private void UpdateUI()
    {
        if (staminaBarUI != null)
            staminaBarUI.fillAmount = currentStamina / maxStamina;

        if (staminaBarCanvasGroup != null)
        {
            staminaBarCanvasGroup.alpha = !sprintLocked ? 1f : 0.5f;
        }
    }

    public bool CanSprint() => !sprintLocked && currentStamina > 0f;

    public void ResetStamina()
    {
        currentStamina = maxStamina;
        sprintLocked = false;
        if (player != null)
            player.sprintActive = false;
    }
}
