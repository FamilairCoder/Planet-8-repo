using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScript : MonoBehaviour
{
    public static bool signal;
    public Sprite signal_sprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (signal)
        {
            GetComponent<SpriteRenderer>().sprite = signal_sprite;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = null;
        }
    }
}
