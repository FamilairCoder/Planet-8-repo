using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayOnObject : MonoBehaviour
{
    public Transform stayOn;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = stayOn.position + offset;
    }
}
