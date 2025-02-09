using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorPositioning : MonoBehaviour
{
    private PatrolID patrolID;
    public GameObject tiedObj;
    private float offset = -2f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (tiedObj != null)
        {
            transform.position = tiedObj.transform.position + new Vector3(0f, offset);
        }
    }
}
