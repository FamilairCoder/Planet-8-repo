using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBulletScript : MonoBehaviour
{
    private float timeleft = 5, spd;
    // Start is called before the first frame update
    void Start()
    {
        spd = Random.Range(20f, 50f);
    }

    // Update is called once per frame
    void Update()
    {
        timeleft -= Time.deltaTime;
        if (timeleft < 0)
        {
            Destroy(gameObject);
        }
        transform.position += transform.up * spd * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.GetComponent<Health>() != null)
        {
            collision.GetComponent<Health>().hp -= collision.GetComponent<Health>().hp;
        }
    }
}
