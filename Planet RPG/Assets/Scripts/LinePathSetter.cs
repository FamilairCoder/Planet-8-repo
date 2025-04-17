using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class LinePathSetter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var path = GetComponent<PathCreator>().path;
        var lr = GetComponent<LineRenderer>();
        for (int i = 0; i < path.NumPoints; i++)
        {
            lr.positionCount = path.NumPoints;
            lr.SetPosition(i, path.GetPoint(i));
        }
    }

    // Update is called once per frame
    void Update()
    {
 
    }
}
