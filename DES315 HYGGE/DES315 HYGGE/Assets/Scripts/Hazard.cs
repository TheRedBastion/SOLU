using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hazard : MonoBehaviour
{
    public Transform character;
    public List<Transform> characterOptions;
    public int currentCharacter;

    //NewMonoBehaviourScript Swap = CharacterSwap.GetComponent<NewMonoBehaviourScript>();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //NewMonoBehaviourScript move = characterOptions[currentCharacter].GetComponentInChildren<NewMonoBehaviourScript>();

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
