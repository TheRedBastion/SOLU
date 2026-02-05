using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hazard : MonoBehaviour
{

    public int damage = 10;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponentInParent<Player>();

        if (player != null)
        {
            Debug.Log("Player has collided with a hazard!");
            player.TakeDamage(damage);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
