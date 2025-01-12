using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMenu : MonoBehaviour
{
    public GameObject menu;
    
    // Start is called before the first frame update
    void Start()
    {
        //menu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log("Mouse Position: " + mousePos);

        if (Input.GetMouseButtonDown(0) && GetComponent<Collider2D>().OverlapPoint(mousePos) && !HUDmanage.on_map)
        {
            if (!PlayerWeapon.using_weapon && !menu.GetComponent<MenuScript>().active)
            {
                menu.GetComponent<MenuScript>().active = true;
                menu.SetActive(true);
            }
        }
    }

}
