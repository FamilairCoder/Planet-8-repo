using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OpenMenu : MonoBehaviour
{
    public GameObject menu;
    public bool forPatrol, bigRecord;
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
            
            if (!PlayerWeapon.using_weapon && ((forPatrol && createdMenu == null && !GetComponent<PatrolID>().taken) || (menu != null && menu.GetComponent<MenuScript>() != null && !menu.GetComponent<MenuScript>().active) || GetComponent<RecordSet>() != null || bigRecord))
            {
                //Debug.Log("BBBBBBB");
                //if ()
                if (!forPatrol)
                {
                    if (GetComponent<RecordSet>() == null)
                    {
                        menu.GetComponent<MenuScript>().active = true;
                        menu.SetActive(true);
                    }
                    else
                    {
                        RecordTextManager.active = true;
                        RecordTextManager.index = GetComponent<RecordSet>().index;
}
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
        if (thisOpened && GetComponent<RecordSet>() != null && !RecordTextManager.active)
        {
            thisOpened = false;
        }
    }

}
