using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectDisappear : MonoBehaviour
{
    public Transform offsetPos;
    private float offsetY;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        offsetY += .5f * Time.deltaTime;
        transform.position = new Vector3(offsetPos.position.x, offsetPos.position.y + offsetY);

        //transform.localPosition += new Vector3(0, .5f) * Time.deltaTime;
        GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, .9f) * Time.deltaTime;
        if (GetComponent<SpriteRenderer>().color.a < 0) Destroy(gameObject);
    }
}
