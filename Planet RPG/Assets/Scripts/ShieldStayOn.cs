using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldStayOn : MonoBehaviour
{
    public GameObject stayOn;
    // Start is called before the first frame update
    void Start()
    {
        if (stayOn.GetComponent<Collider2D>() != null)
        {
            var scal = stayOn.GetComponent<Collider2D>().bounds.size.x * 1.3f;

            transform.localScale = new Vector3(scal, scal);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (stayOn != null)
        {
            transform.position = stayOn.transform.position;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
