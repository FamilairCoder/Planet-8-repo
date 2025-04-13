using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathObjRightRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localRotation = Quaternion.Euler(0, -90f, -90f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
