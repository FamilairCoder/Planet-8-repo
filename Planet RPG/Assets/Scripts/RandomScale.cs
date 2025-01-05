using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomScale : MonoBehaviour
{
    public float min, max;
    // Start is called before the first frame update
    void Start()
    {
        var x = Random.Range(min, max);
        transform.localScale = new Vector3(x, x + Random.Range(min/2, max/2));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
