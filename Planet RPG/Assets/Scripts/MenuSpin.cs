using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSpin : MonoBehaviour
{
    public float spinSpd;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<RectTransform>() == null) transform.rotation *= Quaternion.Euler(0, 0, spinSpd);
        else GetComponent<RectTransform>().transform.rotation *= Quaternion.Euler(0, 0, spinSpd * Time.deltaTime);
    }
}
