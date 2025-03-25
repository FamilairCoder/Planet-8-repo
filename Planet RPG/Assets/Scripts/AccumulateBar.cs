using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccumulateBar : MonoBehaviour
{
    public static float yScale = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector2(1, yScale);
    }
}
