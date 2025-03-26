using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleStayOn : MonoBehaviour
{
    public GameObject stayOn;
    public Material mat;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<ParticleSystemRenderer>().material = mat;
        GetComponent<ParticleSystemRenderer>().trailMaterial = mat;
    }

    // Update is called once per frame
    void Update()
    {
        if (stayOn != null)
        {
            transform.position = stayOn.transform.position;
            transform.position += new Vector3(0, 0, -1);
            if ((stayOn.GetComponent<NPCmovement>() != null && stayOn.GetComponent<NPCmovement>().stunTime <= 0) || (stayOn.GetComponent<PlayerWeapon>() != null && stayOn.GetComponent<PlayerWeapon>().empTime <= 0)) Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
