using UnityEngine;

public class HeadBoxDetector : MonoBehaviour
{
    private GroundDetector groundDetector;

    private void Awake()
    {
        groundDetector = GetComponentInParent<GroundDetector>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            groundDetector.SetInWater(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            groundDetector.SetInWater(false);
        }
    }
}
