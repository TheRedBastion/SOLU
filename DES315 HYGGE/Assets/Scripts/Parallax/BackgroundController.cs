using UnityEngine;

public class MainGameParallax : MonoBehaviour
{
    private float startPos , leng, lengy, startPosy;
    public GameObject cam;
    public float parallaxEffect;
    public float layer;

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


        if(layer==2)//tree
        {
            transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(startPos + dist, startPosy + disty, transform.position.z);
        }


        //cloud was being annoying so i removed the inf loop for it i copy pasted it like 3 times
        if (layer == 1)
        {

        }
        else
        {
            if (move > startPos + leng)
            {
                startPos += leng;
            }
            else if (move < startPos - leng)
            {
                startPos -= leng;
            }
        }


        //if (movey > startPosy + lengy)
        //{
        //    startPosy += lengy;
        //}
        //else if (movey < startPosy - lengy)
        //{
        //    startPosy -= lengy;
        //}



    }
}
