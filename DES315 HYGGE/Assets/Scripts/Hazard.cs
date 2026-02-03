using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hazard : MonoBehaviour
{
    public Transform character;
    public List<Transform> characterOptions;

    public GameObject CharacterSwapGO;
    [SerializeField] int currentChar;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentChar = CharacterSwapGO.GetComponent<CharacterSwap>().currentCharacter;

        if (currentChar == 0)
        {
            currentChar = 1;
        }
        else if (currentChar == 1)
        {
            currentChar = 0;
        }

        Player ins = characterOptions[currentChar].GetComponentInChildren<Player>();



        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has collided with a hazard!");

            if (ins.Character == 1)
            {
                ins.playerHealthM = ins.playerHealthM - 10;
            }
            else if (ins.Character == 2)
            {
                ins.playerHealthS = ins.playerHealthS - 10;
            }
            Debug.Log(ins.playerHealthM);
            Debug.Log(ins.playerHealthS);

        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
