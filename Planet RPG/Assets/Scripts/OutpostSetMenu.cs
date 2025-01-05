using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutpostSetMenu : MonoBehaviour
{
    public GameObject menu;
    // Start is called before the first frame update
    void Start()
    {
        var canvas = FindObjectOfType<Canvas>();
        var apos = new Vector2(0, 10000 + Random.Range(0, 20000));
        var m = Instantiate(menu, apos, Quaternion.identity, canvas.transform);
        m.GetComponent<MenuScript>().station = gameObject;
        GetComponent<OpenMenu>().menu = m;
        m.GetComponent<MenuScript>().isOutpost = true;
        m.GetComponent<MenuScript>().away_pos = apos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
