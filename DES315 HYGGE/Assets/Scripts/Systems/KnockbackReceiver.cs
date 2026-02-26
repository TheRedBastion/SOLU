using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class KnockbackReceiver : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isKnockback;

    public bool IsKnockedBack => isKnockback;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ApplyKnockback(KnockbackData data)
    {
        //face the knockback source
        FaceDirection(-data.direction.x);

        StartCoroutine(KnockbackCoroutine(data));
    }

    private IEnumerator KnockbackCoroutine(KnockbackData data)
    {
        isKnockback = true;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(data.direction * data.knockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(data.duration);

        isKnockback = false;
    }

    private void FaceDirection(float xDirection)
    {
        if (Mathf.Abs(xDirection) < 0.01f) return;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(xDirection);
        transform.localScale = scale;
    }
}
