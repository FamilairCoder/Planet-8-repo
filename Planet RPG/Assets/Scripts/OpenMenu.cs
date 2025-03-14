using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OpenMenu : MonoBehaviour
{
    public GameObject menu;
    public bool forPatrol;
    public float hireCost;
    public static bool opened = false;
    private bool thisOpened = false;
    [Header("dont set--------------------")]
    public GameObject createdMenu;
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

        if (!PatrolManager.focusFire && !EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0) && GetComponent<Collider2D>().OverlapPoint(mousePos) && !HUDmanage.on_map && !opened && !HUDmanage.pauseMenu)
        {
            //Debug.Log("AAAAAAAAAAA");
            if (!PlayerWeapon.using_weapon && ((forPatrol && createdMenu == null && !GetComponent<PatrolID>().taken) || !menu.GetComponent<MenuScript>().active))
            {
                //Debug.Log("BBBBBBB");
                if (!forPatrol)
                {
                    menu.GetComponent<MenuScript>().active = true;
                    menu.SetActive(true);
                }
                else
                {
                    var m = Instantiate(menu, new Vector2(0, 0), Quaternion.identity, FindObjectOfType<Canvas>().transform);
                    m.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                    m.GetComponent<MenuScript>().active = true;
                    m.GetComponent<MenuScript>().station = gameObject;
                    m.GetComponent<MenuScript>().hireCost = hireCost;

                    m.GetComponent<PatrolHireVisual>().patrolObj = gameObject;
                    createdMenu = m;
                }
                opened = true;
                thisOpened = true;
            }
        }
        if (thisOpened && ((!forPatrol && menu != null && !menu.activeSelf) || (forPatrol && createdMenu == null)))
        {
            thisOpened = false;
            opened = false;
        }
    }

}
