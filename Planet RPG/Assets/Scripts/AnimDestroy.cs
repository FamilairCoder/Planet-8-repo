using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void AlertObservers(string message)
    {
        if (message.Equals("AttackAnimationEnded"))
        {
            Destroy(gameObject);
            // Do other things based on an attack ending.
        }
    }
}