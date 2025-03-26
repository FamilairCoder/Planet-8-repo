using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnTimer : MonoBehaviour
{
    private float timeleft = 20;
    private SpriteRenderer spr;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<SpriteRenderer>() != null) spr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        timeleft -= Time.deltaTime;

        if (GetComponent<SpriteRenderer>() != null) spr.color = Color.Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), timeleft / 20);
        if (timeleft < 5f && GetComponent<Rigidbody2D>() != null)
        {
            Destroy(GetComponent<Rigidbody2D>());
        }
        if (timeleft < 0 )
        {
            Destroy(gameObject);
        }

    }
}
