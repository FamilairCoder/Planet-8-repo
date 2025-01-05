using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flicker : MonoBehaviour
{
    private SpriteRenderer sp;
    private Image img;
    private float timeleft, times;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<SpriteRenderer>() != null) sp = GetComponent<SpriteRenderer>();
        else if (GetComponent<Image>() != null) img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        timeleft -= Time.deltaTime;
        if (timeleft < 0)
        {
            if (sp != null) sp.enabled = true;
            else if (img != null) img.enabled = true;
            var chance = Random.Range(0f, 1f);
            if (chance < .75f)
            {
                if (sp != null) sp.enabled = false;
                else if (img != null) img.enabled = false;
                if (GetComponent<ParticleSystem>() != null)
                {
                    GetComponent<ParticleSystem>().Play();
                }
                timeleft = Random.Range(.01f, .03f);
                chance = Random.Range(0f, 1f);
                if (chance < .01f)
                {
                    timeleft = Random.Range(1f, 4f);
                }
            }
            else
            {
                timeleft = Random.Range(0, 1f);
                chance = Random.Range(0f, 1f);
                if (chance < .05f)
                {
                    timeleft = Random.Range(3f, 6f);
                }
            }
        }
    }



}
