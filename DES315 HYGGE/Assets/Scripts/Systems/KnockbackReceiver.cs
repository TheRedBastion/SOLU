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
}
