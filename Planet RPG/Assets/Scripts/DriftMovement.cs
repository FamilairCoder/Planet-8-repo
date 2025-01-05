using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriftMovement : MonoBehaviour
{
    private float driftSpd, rotationSpd;
    
    private Vector3 dir;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dir = Random.insideUnitCircle.normalized;
        //driftSpd = Random.Range(0f, 1f);
        //rotationSpd = Random.Range(-30f, 30f);
        rb.AddForce(dir * Random.Range(1f, 5f), ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        //transform.position += dir * driftSpd * Time.deltaTime;
        //transform.localRotation *= Quaternion.Euler(0, 0, rotationSpd * Time.deltaTime);
    }
}
