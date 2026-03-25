using UnityEngine;

public class MainGameParallax : MonoBehaviour
{
    private float startPos , leng;
    public GameObject cam;
    public float parallaxEffect;

    public int layer;
    void Start()
    {
        startPos = transform.position.x;
        leng = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float dist = (cam.transform.position.x * parallaxEffect);
        float move = cam.transform.position.x * (1 - parallaxEffect);


        if (layer == 1)
        {
            transform.position = new Vector3(startPos + dist, cam.transform.position.y + 1, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(startPos + dist, /*cam.*/transform.position.y, transform.position.z);
        }
        transform.position = new Vector3(startPos + dist, /*cam.*/transform.position.y, transform.position.z);
        if (move > startPos + leng)
        {
            startPos += leng;
        }
        else if (move < startPos - leng)
        {
            startPos -= leng;
        }

    }
}
