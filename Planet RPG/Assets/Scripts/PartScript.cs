using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PartScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public string description;
    public float cost;
    public bool isBasicLaser, isLaserRod, isLaserBeam;
    [Header("Bonus")]
    public float armor_bonus;
    public float dmg_bonus, firerate_bonus, thrust_bonus, turnspd_bonus, cargo_bonus, mining_bonus, ore_bonus, energyRegenBonus, energyCapacityBonus;
    [Header("dont have to set-----------")]
    public bool down;
    private Vector3 orig_pos;    
    public GameObject came_from, circuit_slot;
    public bool did;
    public float delay_time = .05f;
    public bool do_delay;
    private bool dida;

    void Start()
    {
        down = false;
        delay_time = .05f;
        do_delay = false;
        orig_pos = GetComponent<RectTransform>().localPosition;
        GetComponent<RectTransform>().SetAsLastSibling();
    }

    void Update()
    {


        if (!gameObject.activeSelf && !dida)
        {

            dida = true;
        }

        if (down)
        {
            Vector2 localPoint;
            var rect = transform.parent.GetComponent<RectTransform>();

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, new Vector2(Input.mousePosition.x, Input.mousePosition.y), null, out localPoint))
            {
                GetComponent<RectTransform>().localPosition = localPoint;
            }
        }

        if (do_delay)
        {
            delay_time -= Time.deltaTime;
            if (delay_time <= 0)
            {
                GetComponent<RectTransform>().localPosition = orig_pos;
                down = false;
                do_delay = false;
                delay_time = .05f;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (HUDmanage.money >= cost)
        {
            SellButtonScript.sell = false;
            down = true;
            orig_pos = GetComponent<RectTransform>().localPosition;
        } 

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        down = false;
        if (circuit_slot == null)
        {
            do_delay = true;
        }
    }
}