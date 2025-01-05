using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayInPlace : MonoBehaviour
{
    public float spd;
    private Vector3 stay_pos;
    // Start is called before the first frame update
    void Start()
    {
        stay_pos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var rb = GetComponent<Rigidbody2D>();
        if (Vector2.Distance(transform.position, stay_pos) > 1f)
        {
            var dir = (stay_pos - transform.position).normalized;
            rb.AddForce(dir * spd * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, new(0, 0), .1f);
        }
        //rb.velocity = Vector2.ClampMagnitude(rb.velocity, spd);
    }
}
