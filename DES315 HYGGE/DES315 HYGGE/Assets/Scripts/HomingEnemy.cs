using UnityEngine;

public class HomingEnemy : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public float detectionRange = 5f;

    CharacterSwap characterSwap;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //Find player if not assigned in inspector
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        characterSwap = GetComponentInParent<CharacterSwap>();
        if (characterSwap.currentCharacter == 0)
        {
            player = GameObject.FindGameObjectWithTag("Player2").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;


        //todo fix this so it only updates when character is swapped
        player = characterSwap.characterOptions[characterSwap.currentCharacter];

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if(distanceToPlayer <= detectionRange)
        {
            //move towards player
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * speed;
        }
        else
        {
            //stop if player is out of range
            rb.linearVelocity = Vector2.zero;
        }

    }
}
