using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPackCollide : MonoBehaviour
{
    public GameObject part;
    private float coll;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            coll++;
            if (coll == 1) Instantiate(part, transform.position, Quaternion.identity);
            HUDmanage.money += Random.Range(30, 50) / 2;
            Destroy(gameObject);
        }
    }
}
