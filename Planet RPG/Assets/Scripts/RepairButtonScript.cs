using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RepairButtonScript : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject text;
    public ParticleSystem repair_part;
    private float cost;
    private ShipStats ship;
    // Start is called before the first frame update
    void Start()
    {
        ship = FindObjectOfType<PlayerMovement>().transform.GetChild(0).GetComponent<ShipStats>();

    }

    // Update is called once per frame
    void Update()
    {
        var current_hp = 0f;
        for (int i = 0; i < ship.transform.childCount; i++)
        {
            if (ship.transform.GetChild(i).GetComponent<Health>() != null && ship.transform.GetChild(i).gameObject.activeSelf)
            {
                current_hp += ship.transform.GetChild(i).GetComponent<Health>().hp;
            }
        }

        cost = Mathf.Round((ship.total_hp - current_hp) * 5);
        text.GetComponent<TextMeshProUGUI>().text = cost.ToString() + " photons";
        if (cost > 0)
        {
            GetComponent<Image>().color = Color.green;
        }
        else GetComponent<Image>().color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.GetComponent<TextMeshProUGUI>().enabled = true;

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        text.GetComponent<TextMeshProUGUI>().enabled = false;

    }
    public void OnPointerClick(PointerEventData eventData)
    {

        if (HUDmanage.money >= cost)
        {
            var ship = FindObjectOfType<PlayerMovement>().transform.GetChild(0).GetComponent<ShipStats>();
            for (int i = 0; i < ship.transform.childCount; i++)
            {
                if (ship.transform.GetChild(i).GetComponent<Health>() != null)
                {
                    ship.transform.GetChild(i).GetComponent<Health>().hp = ship.transform.GetChild(i).GetComponent<Health>().orig_hp;
                }
            }
            HUDmanage.money -= cost;
        }

    }

}
