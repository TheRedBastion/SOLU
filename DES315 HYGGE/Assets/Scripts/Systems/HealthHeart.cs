using UnityEngine;
using UnityEngine.UI;

public class HealthHeart : MonoBehaviour
{
    public Sprite FullHeart, EmptyHeart;
    Image heartImage;

    private void Awake()
    {
        heartImage = GetComponent<Image>();
    }

    public void SetHeartStatus(HeartStatus status)
    {
        switch (status)
        {
            case HeartStatus.Empty:
                heartImage.sprite = EmptyHeart;
                break;
            case HeartStatus.Full:
                heartImage.sprite = FullHeart;
                break;
        }
    }
}

public enum HeartStatus
{
    Empty = 0,
    Full = 1
}