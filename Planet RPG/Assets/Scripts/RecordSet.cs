using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordSet : MonoBehaviour
{
    public int index;
    // Start is called before the first frame update
    void Start()
    {
        index = Random.Range(0, RecordTextManager.recordAmount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
