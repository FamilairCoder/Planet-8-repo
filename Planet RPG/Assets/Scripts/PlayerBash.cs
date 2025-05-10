using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBash : MonoBehaviour
{
    public static bool bash;
    private bool bashing;
    private ParticleSystem bashParticles;
    public ParticleSystem bashExplosion;
    private float failSafeTime;
    // Start is called before the first frame update
    void Start()
    {
        bashParticles = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bash)// && Input.GetKeyDown(KeyCode.LeftShift))
        {
            failSafeTime -= Time.deltaTime;
            if (failSafeTime < 0)
            {
                bash = false;
                failSafeTime = 10;
            }
            if (!bashing)
            {
                bashParticles.Play();
                bashing = true;
            }
            


        }
        else
        {
            if (bashing)
            {
                bashParticles.Stop();                
                bashing = false;
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (bashing && Input.GetKey(KeyCode.LeftShift) && collision.GetComponent<Health>() != null && collision.transform.parent.GetComponent<NPCmovement>().retreat)
        {
            bashExplosion.Play();
            collision.GetComponent<Health>().hp = 0;
            GetComponent<AudioSource>().Play();
        }
    }
}
