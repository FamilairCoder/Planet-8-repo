using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Occlusion : MonoBehaviour
{
    private Renderer r;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<SpriteRenderer>() != null) r = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnBecameInvisible()
    {
        Debug.Log("invisible");
        if (r != null) r.enabled = false; // Disable the sprite renderer when off-screen

    }



    void OnBecameVisible()
    {
        Debug.Log("visible");
        if (r != null) r.enabled = true; // Enable the sprite renderer when back on-screen

    }
}
