using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTrail : MonoBehaviour
{
    public List<Vector2> points = new List<Vector2>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var lr = GetComponent<LineRenderer>();
        for (int i = 0; i < points.Count; i++)
        {
            if (i == 0)
            {                
                points[i] = Vector2.Lerp(transform.position, points[i], 0);
                lr.SetPosition(i, points[i]);
            }
            else
            {
                points[i] = Vector2.Lerp(points[i - 1], points[i], .5f);
                lr.SetPosition(i, new(points[i].x, points[i].y, -.05f));
            }
            //points[i] = transform.parent.transform.position + transform.parent.transform.up * 2 * i;
        }
    }
}
