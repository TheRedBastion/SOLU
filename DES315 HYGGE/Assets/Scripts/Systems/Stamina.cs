using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    public float currentStamina = 100f;
    [SerializeField] public float maxStamina = 100f;
    public bool staminaFull = true;

    private Player player;

    [SerializeField] public float lossRate = 5f;
    [SerializeField] public float regenRate = 5f;

    [SerializeField] private Image staminaBarUI = null;
    [SerializeField] private CanvasGroup staminaBarCanvasGroup = null;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }
    public void Update()
    {
        if (player.sprintActive)
        {
            currentStamina -= lossRate * Time.deltaTime;

            if (currentStamina <= 0)
            {
                currentStamina = 0;
                staminaBarCanvasGroup.alpha = 0f;
            }
        }
        else if (!player.sprintActive && currentStamina < maxStamina)
        {
            currentStamina += regenRate * Time.deltaTime;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        StaminaUpdate(currentStamina > 0 ? 1 : 0);
    }

    void StaminaUpdate(int value)
    {
        staminaBarUI.fillAmount = currentStamina / maxStamina;

        if (value == 0)
        {
            staminaBarCanvasGroup.alpha = 0f;
        }
        else
        {
            staminaBarCanvasGroup.alpha = 1f;
        }
    }
}
