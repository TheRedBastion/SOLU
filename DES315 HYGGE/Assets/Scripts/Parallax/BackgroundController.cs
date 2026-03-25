using UnityEngine;

public class MainGameParallax : MonoBehaviour
{
    private float startPos , leng, lengy, startPosy;
    public GameObject cam;
    public float parallaxEffect;
    public int layer;

    void Start()
    {
        startPos = transform.position.x;
        leng = GetComponent<SpriteRenderer>().bounds.size.x;

        startPosy = transform.position.y;
        lengy = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float dist = (cam.transform.position.x * parallaxEffect);
        float move = cam.transform.position.x * (1 - parallaxEffect);

        float disty = (cam.transform.position.y * parallaxEffect);
        float movey = cam.transform.position.y * (1 - parallaxEffect);


        if (layer == 1)
        {
            transform.position = new Vector3(startPos + dist, startPosy + disty , transform.position.z);
        }
            
        else
        {
            transform.position = new Vector3(startPos + dist, startPosy + disty , transform.position.z);
        }
                


        if (move > startPos + leng)
        {
            startPos += leng;
        }
        else if (move < startPos - leng)
        {
            startPos -= leng;
        }

        if (movey > startPosy + lengy)
        {
            startPos += lengy;
        }
        else if (movey < startPosy - lengy)
        {
            startPosy -= lengy;
        }
    }
}
