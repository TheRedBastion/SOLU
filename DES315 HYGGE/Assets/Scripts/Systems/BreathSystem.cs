using UnityEngine;
using UnityEngine.UI;

public class BreathSystem : MonoBehaviour
{
    [SerializeField] private float maxBreath = 5f;
    [SerializeField] private float breathRecoverySpeed = 2f;

    private float currentBreath;
    private GroundDetector groundDetector;
    private Health health;

    public Image airCircle;

    public float BreathPercent => currentBreath / maxBreath;

    private void Awake()
    {
        groundDetector = GetComponent<GroundDetector>();
        health = GetComponent<Health>();
        currentBreath = maxBreath;
    }

    private void Update()
    {
        Player player = GetComponentInParent<Player>();

        if (player != null && player.isGodmode)
        {
            currentBreath = maxBreath;
            airCircle.gameObject.SetActive(false);
            return;
        }

        if (groundDetector.InWater)
        {
            currentBreath -= Time.deltaTime;

            if (currentBreath <= 0f)
            {
                currentBreath = 0f;
                Drown();
            }
        }
        else
        {
            currentBreath += breathRecoverySpeed * Time.deltaTime;
            currentBreath = Mathf.Min(currentBreath, maxBreath);
        }

        airCircle.fillAmount = currentBreath / maxBreath;
        bool showAir = groundDetector.InWater || currentBreath < maxBreath;
        airCircle.gameObject.SetActive(showAir);
    }

    void Drown()
    {
        KnockbackData kb = new KnockbackData(Vector2.zero, 0f, 0f, true);
        health.TakeDamage(1, kb);
    }
}