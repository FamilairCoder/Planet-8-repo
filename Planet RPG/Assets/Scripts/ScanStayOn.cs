using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanStayOn : MonoBehaviour
{
    
    public GameObject asteroid;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<ParticleSystem>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, .1f) * Time.deltaTime;
        if (GetComponent<SpriteRenderer>().color.a <= 0) { Destroy(gameObject); asteroid.GetComponent<AsteroidInfo>().has_detected = false; }
        transform.position = asteroid.transform.position;
    }
}
