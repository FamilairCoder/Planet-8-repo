using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSFXHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().pitch = Random.Range(.2f, .4f);
            //GetComponent<AudioSource>().pitch = .;
            GetComponent<AudioSource>().Play();
        }

    }
}
