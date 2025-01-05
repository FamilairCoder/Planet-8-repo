using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetNav : MonoBehaviour
{
    private GameObject player;
    public GameObject nav_text;
    public MapParralax map_obj;
    public bool create_nav;
    public GameObject nav_obj;
    private bool did;
    public string nav_sakekey;
    public int deliverAmount;
    // Start is called before the first frame update
    void Start()
    {
        if (nav_sakekey == "") nav_sakekey = gameObject.name + "mapobj";
        player = FindObjectOfType<PlayerMining>().gameObject;
        if (create_nav)
        {
            var nav = Instantiate(nav_obj, transform.position, Quaternion.identity);
            map_obj = nav.GetComponent<MapParralax>();
            map_obj.linked_obj = gameObject;
            map_obj.savekey = nav_sakekey;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!did && Physics2D.OverlapCircleAll(transform.position, 30, LayerMask.GetMask("playerparts")).Length > 0 && !map_obj.on) { SetBeacon(); }
        map_obj.deliveryAmount = deliverAmount;
    }

    public void SetBeacon()
    {
        map_obj.on = true; 
        did = true; 
        Instantiate(nav_text, transform.position, Quaternion.identity);
    }
}
