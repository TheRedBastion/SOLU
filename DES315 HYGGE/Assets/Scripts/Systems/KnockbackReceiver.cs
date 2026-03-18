using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class KnockbackReceiver : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isKnockback;
    private bool isStunned;

    public bool IsKnockedBack => isKnockback;
    public bool IsStunned => isStunned;

    private Coroutine currentKnockback;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ApplyKnockback(KnockbackData data)
    {
        //face the knockback source
        FaceDirection(-data.direction.x);

        if (data.noKnockback) return;
        if (isKnockback) return;

        if (currentKnockback != null)
        {
            StopCoroutine(currentKnockback);
        }

        currentKnockback = StartCoroutine(KnockbackCoroutine(data));
    }

    private IEnumerator KnockbackCoroutine(KnockbackData data)
    {
        isKnockback = true;
        isStunned = true;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(data.direction * data.knockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(data.duration);

        rb.linearVelocity = Vector2.zero;
        isKnockback = false;

        float remainingStun = Mathf.Max(0f, data.stunDuration - data.duration);
        yield return new WaitForSeconds(remainingStun);

        isStunned = false;
        currentKnockback = null;
    }

    private void FaceDirection(float xDirection)
    {
        if (Mathf.Abs(xDirection) < 0.01f) return;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(xDirection);
        transform.localScale = scale;
    }
}
