using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMenu : MonoBehaviour
{
    public GameObject menu;
    
    // Start is called before the first frame update
    void Start()
    {
        
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
            }
        }
    }

    private void OnMouseOver()
    {
        //Debug.Log("balls");
/*        var player_pos = FindObjectOfType<PlayerMovement>().transform.position;
        if (Vector2.Distance(player_pos, transform.position) < 20)
        {
            MouseScript.signal = true;

        }*/
    }
    private void OnMouseDown()
    {
        //Debug.Log("penis");

        
        
    }
}
