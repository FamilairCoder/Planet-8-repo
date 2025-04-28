using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
