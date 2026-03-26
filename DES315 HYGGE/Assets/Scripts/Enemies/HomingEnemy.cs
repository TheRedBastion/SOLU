using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingEnemy : BaseEnemy
{
    [Header("Enemy Vars")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float detectionRange = 5f;

    protected override void Update()
    {
        base.Update();

        if (knockback != null && (knockback.IsKnockedBack || knockback.IsStunned)) return;
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            //move towards player
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * speed;

            UpdateFacingDirection(direction.x);
        }
        else
        {
            //stop moving if out of range
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void SetDetectionRange(float newRange)
    {
        detectionRange = newRange;
    }

    void UpdateFacingDirection(float directionX)
    {
        Vector3 scale = transform.localScale;

        if (directionX > 0)//SIGNS ARE FLIPPED AS BAT SPRITE IS FLIPPED TO LEFT
            scale.x = -Mathf.Abs(scale.x);
        else if (directionX < 0)
            scale.x = Mathf.Abs(scale.x);

        transform.localScale = scale;
    }
}
