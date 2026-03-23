using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField] private WaveRoom room;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            room.StartRoom();
            gameObject.SetActive(false);
        }
    }
}