using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationStats : MonoBehaviour
{
    public List<GameObject> turrets = new List<GameObject>();
    public List<GameObject> linked_stations = new List<GameObject>();
    public string target_tag, name;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject t in turrets)
        {
            t.GetComponent<StationWeapon>().target_tag = target_tag;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
