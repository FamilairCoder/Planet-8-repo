using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CVstationLink : MonoBehaviour
{
    public GameObject station;//, ship_stats;
    // Start is called before the first frame update
    void Start()
    {
        station = transform.parent.GetComponent<MenuScript>().station;
        var index = transform.GetSiblingIndex();
        transform.SetSiblingIndex(index - 6);
    }

    // Update is called once per frame
    void Update()
    {
/*        for (int i = 0; i < transform.GetChild(0).transform.childCount; i++)
        {
            if (ship_stats != null)
                transform.GetChild(0).transform.GetChild(i).GetComponent<CircuitSlotScript>().ship_stats = ship_stats;
        }*/
        
    }
}
