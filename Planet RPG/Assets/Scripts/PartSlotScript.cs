using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PartSlotScript : MonoBehaviour
{
    public GameObject part, station;
    private MenuScript menu;
    private bool added;
    public int part_min, partExcludeLower, partExcludeUpper;
    public List<GameObject> parts = new List<GameObject>();
    public List<GameObject> weapon_parts = new List<GameObject>();
    public TextMeshProUGUI desc_text, cost_text;
    private float cost, part_time = 360f;
    private string desc;
    public int part_numb;
    public GameObject part_button;
    private bool did;
    [Header("For weapon---------")]
    public bool isWeapon;
    // Start is called before the first frame update
    void Start()
    {
        station = transform.parent.GetComponent<MenuScript>().station;
        menu = transform.parent.GetComponent<MenuScript>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!did)
        {
            part_time += menu.restockAdd;
            var key = station.GetComponent<ShipSpawner>().savekey;
            if (SaveManager.GetInt(key + "part slot" + transform.GetSiblingIndex(), 0) != parts.Count - 1 && SaveManager.GetInt(key + "empty" + transform.GetSiblingIndex(), 0) == 0)
                ChooseNewPart();

            did = true;
        }
        if (part != null)
        {
            desc_text.text = desc;
            cost_text.text = cost.ToString() + " photons";
            if (HUDmanage.money < cost) cost_text.color = new Color(.7f, 0, 0);
            else if (HUDmanage.money >= cost) cost_text.color = new Color(0, .72f, 0);
            added = false;
        }
        else
        {
            var key = station.GetComponent<ShipSpawner>().savekey;
            SaveManager.SetInt(key + "part slot" + transform.GetSiblingIndex(), parts.Count - 1);
            SaveManager.SetInt(key + "empty" + transform.GetSiblingIndex(), 1);
            desc_text.text = "Restocking...";
            cost_text.text = Mathf.Round(part_time).ToString();
            cost_text.color = new Color(.7f, 0, 0);
           
            if (!added) { part_time = 360 + menu.restockAdd; menu.restockAdd += 30; added = true; }
            if (part_time < 0)
            {
                SaveManager.SetInt(key + "part slot" + transform.GetSiblingIndex(), Random.Range(part_min, parts.Count));
                ChooseNewPart();
                SaveManager.SetInt(key + "empty" + transform.GetSiblingIndex(), 0);
                //part_time = 60 + menu.restockAdd;
            }
        }



    }

    private void ChooseNewPart()
    {
        var key = station.GetComponent<ShipSpawner>().savekey;

        int randomIndex = Random.Range(part_min, parts.Count);
        if (partExcludeLower != 0 && partExcludeUpper != 0)
        {
            do
            {
                randomIndex = Random.Range(part_min, parts.Count);
            } while (randomIndex >= partExcludeLower && randomIndex <= partExcludeUpper);
        }


        if (!isWeapon) part_numb = SaveManager.GetInt(key + "part slot" + transform.GetSiblingIndex(), randomIndex);
        else part_numb = SaveManager.GetInt(key + "part slot" + transform.GetSiblingIndex(), Random.Range(part_min, weapon_parts.Count));
        SaveManager.SetInt(key + "part slot" + transform.GetSiblingIndex(), part_numb);
        GameObject p = null;
        if (!isWeapon) p = Instantiate(parts[part_numb], transform.position, Quaternion.identity, transform);
        else p = Instantiate(weapon_parts[part_numb], transform.position, Quaternion.identity, transform);
        p.transform.parent = transform.parent;
        p.GetComponent<AppearTextOnHover>().texts.Add(desc_text);
        p.GetComponent<AppearTextOnHover>().texts.Add(cost_text);
        p.GetComponent<PartScript>().came_from = gameObject;
        part = p;
        cost = part.GetComponent<PartScript>().cost;
        desc = part.GetComponent<PartScript>().description;
        if (part_button != null) { part_button.GetComponent<MenuButton>().show.Add(p); }

    }
    public void Countdown()
    {
        //Debug.Log(part_time);
        if (part == null)
        {
            part_time -= Time.deltaTime;
        }
    }
}
