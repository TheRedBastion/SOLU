using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hazard : MonoBehaviour
{
    public Transform character;
    public List<Transform> characterOptions;
    public int currentCharacter;

    //Player Swap = CharacterSwap.GetComponent<Player>();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Player move = characterOptions[currentCharacter].GetComponentInChildren<Player>();

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has collided with a hazard!");
            
            
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
