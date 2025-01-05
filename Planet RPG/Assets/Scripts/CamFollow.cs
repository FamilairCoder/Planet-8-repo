using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public GameObject tofollow;
    public bool not_at_10;
    public float zoffset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var targetpos = tofollow.transform.position;
        if (!not_at_10) transform.position = new(targetpos.x, targetpos.y, -10);
        else transform.position = new(targetpos.x, targetpos.y, zoffset);
    }
}
