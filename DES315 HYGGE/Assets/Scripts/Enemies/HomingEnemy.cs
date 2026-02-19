using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingEnemy : BaseEnemy
{
    public float speed = 2f;
    public float detectionRange = 5f;

    protected override void Update()
    {
        base.Update();

        if (knockback != null && knockback.IsKnockedBack) return;
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            //move towards player
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * speed;
        }
        else
        {
            //stop moving if out of range
            rb.linearVelocity = Vector2.zero;
        }
    }
}
