using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningProbeBar : MonoBehaviour
{
    public Color green, red;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var parent = transform.parent.GetComponent<MiningProbeLogic>();
        transform.localScale = Vector2.Lerp(new(1, 0), new(1, 1.1245f), parent.ore_amount / parent.ore_capacity);
        var spr = transform.GetChild(0).GetComponent<SpriteRenderer>();

        if (parent.ore_amount / parent.ore_capacity >= .85f) spr.color = Color.Lerp(green, red, ((parent.ore_amount / parent.ore_capacity) - .85f) / .15f);
        else spr.color = green;

    }
}
