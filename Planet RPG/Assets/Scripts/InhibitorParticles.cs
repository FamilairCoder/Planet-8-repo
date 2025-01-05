using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InhibitorParticles : MonoBehaviour
{
    private PlayerMovement pm;
    private bool did;
    // Start is called before the first frame update
    void Start()
    {
        pm = transform.parent.GetComponent<PlayerMovement>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!did && pm.inhibited)
        {
            GetComponent<ParticleSystem>().Play();
            did = true;
        }
        else if (!pm.inhibited)
        {
            GetComponent<ParticleSystem>().Stop();
            did = false;
        }
    }
}
