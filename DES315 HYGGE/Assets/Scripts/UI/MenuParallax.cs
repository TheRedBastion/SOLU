using UnityEngine;
using UnityEngine.InputSystem;

public class MenuParallax : MonoBehaviour
{
    public float offsetMultiplier = 20f;
    public float smoothTime = 0.3f;

    private RectTransform rectTransform;
    private Vector2 startPosition;
    private Vector2 velocity;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
    }

    void Update()
    {
        if (Mouse.current == null) return;
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 normalizer = new Vector2((mousePos.x / Screen.width) - 0.5f, (mousePos.y / Screen.height) - 0.5f);
        Vector2 target = startPosition + normalizer * offsetMultiplier;
        rectTransform.anchoredPosition = Vector2.SmoothDamp(rectTransform.anchoredPosition, target, ref velocity, smoothTime);
    }
}