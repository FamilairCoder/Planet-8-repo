using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitMovement : MonoBehaviour
{
    public GameObject obj_around;
    public float spd;
    private float vec, distx, disty;
    private bool did;
    // Start is called before the first frame update
    void Start()
    {
        distx = Vector2.Distance(transform.localPosition, new(0, 0));
        disty = distx / Random.Range(2f, 4f);
        vec = 90;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var dist = Vector2.Distance(transform.localPosition, new(0, 0));

        if (Mathf.Abs(dist - distx) < .5f && !did)
        {
            if (GetComponent<SpriteRenderer>().sortingOrder == 3)            
                GetComponent<SpriteRenderer>().sortingOrder = 1;   
            else 
                GetComponent<SpriteRenderer>().sortingOrder = 3;
            did = true;
        }
        else if (Mathf.Abs(dist - distx) > .5f)
        {
            did = false;
        }
        vec += spd * Time.deltaTime;
        var pos = new Vector2(distx * Mathf.Sin(Mathf.Deg2Rad * vec), disty * Mathf.Cos(Mathf.Deg2Rad * vec));
        transform.localPosition = pos;
    }
}
