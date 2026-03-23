using UnityEngine;

public class MainGameParallax : MonoBehaviour
{
    private float startPos;
    public GameObject cam;
    public float parallaxEffect;
    void Start()
    {
        startPos = transform.position.x;

    }

    // Update is called once per frame
    void Update()
    {
        float dist = (cam.transform.position.x * parallaxEffect);// 0 with cam 1 wont move

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);
    }
}
