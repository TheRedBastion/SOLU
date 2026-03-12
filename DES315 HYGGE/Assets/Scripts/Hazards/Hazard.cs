using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hazard : MonoBehaviour
{

    public int damage = 10;
    //public float knockbackForce = 10f;
    //public float knockbackDuration = 0.2f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Health health = collision.gameObject.GetComponent<Health>();
        if (health != null)
        {
            Player player = collision.gameObject.GetComponentInParent<Player>();

            if (player != null && !player.isGodmode)
            {
                health.TakeDamage(1);
            }

        }

        //KnockbackReceiver kb = collision.gameObject.GetComponentInParent<KnockbackReceiver>();
        //if (kb != null)
        //{
        //    Vector2 direction = (collision.transform.position - transform.position).normalized;
        //
        //    direction.y = Mathf.Max(direction.y, 0.2f);
        //
        //    KnockbackData data = new KnockbackData(
        //        direction,
        //        knockbackForce,
        //        knockbackDuration
        //    );
        //
        //
        //    if (health != null)
        //    {
        //        health.TakeDamage(damage, data);
        //    }
        //}

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
