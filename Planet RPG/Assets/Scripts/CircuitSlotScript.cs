using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class CircuitSlotScript : MonoBehaviour, IPointerDownHandler
{
    public bool isWeapon, isBasicLaser, isLaserRod, isLaserBeam;
    public Image image;
    private Sprite orig;
    private GameObject picked_up_obj, linked_part;
    public bool filled = false, did;
    public GameObject ship_stats;
    public List<Sprite> part_images = new List<Sprite>();
    public List<Sprite> weaponPart_images = new List<Sprite>();
    // Start is called before the first frame update
    void Start()
    {
        orig = image.sprite;


        var key = transform.parent.transform.parent.GetComponent<CVstationLink>().station.GetComponent<ShipSpawner>().savekey;


        if (!isBasicLaser && !isLaserBeam && !isLaserRod)
        {
            image.sprite = part_images[PlayerPrefs.GetInt("part number" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex(), part_images.Count - 1)];
        }



        if (PlayerPrefs.GetInt("filled" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex(), 0) == 1)
        {
            filled = true;

        }

        if (isBasicLaser) { PlayerWeapon.basic_lasers++; PlayerPrefs.SetInt("part number" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex(), 0); }
        if (isLaserRod) { PlayerWeapon.laser_rods++; PlayerPrefs.SetInt("part number" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex(), 1); }
        if (isLaserBeam) { PlayerWeapon.laser_beams++; PlayerPrefs.SetInt("part number" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex(), 2); }


    }

    // Update is called once per frame
    void Update()
    {
        

        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, 25);
        foreach (var a in hit)
        {
            if (filled) break;
            var pscript = a.GetComponent<PartScript>();
            if (pscript != null && a.CompareTag("partslot") && !pscript.did && !a.Equals(linked_part) && !a.Equals(picked_up_obj))
            {
                bool isWeaponPart = pscript.isBasicLaser || pscript.isLaserRod || pscript.isLaserBeam;
                if ((isWeaponPart && isWeapon) || (!isWeaponPart && !isWeapon)) InvisPartOnDisplay(a.gameObject);
                break;
            }
        }
        

        if (hit.Length == 0)
        {

            image.sprite = part_images[PlayerPrefs.GetInt("part number" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex(), part_images.Count - 1)];
            


            if (PlayerPrefs.GetInt("filled" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex()) == 1)//, 0) == 1)
            {
                filled = true;                
            }
        }
        if (picked_up_obj != null)
        {
            if (Vector2.Distance(transform.position, picked_up_obj.transform.position) > 25 && filled)
            {
                InvisDisplayOnPart();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                SlotPart();
            }
        }



    }


    public void OnPointerDown(PointerEventData eventData)
    {


    }

    void InvisPartOnDisplay(GameObject a)
    {
        image.sprite = a.GetComponent<Image>().sprite;
        a.GetComponent<Image>().enabled = false;
        picked_up_obj = a.gameObject;
        a.GetComponent<PartScript>().circuit_slot = gameObject;
        a.GetComponent<PartScript>().did = true;
        filled = true;
    }

    void InvisDisplayOnPart()
    {
        image.sprite = orig;
        picked_up_obj.GetComponent<Image>().enabled = true;
        picked_up_obj.GetComponent<PartScript>().circuit_slot = null;
        picked_up_obj.GetComponent<PartScript>().did = false;
        picked_up_obj = null;
        filled = false;
    }

    void SlotPart()
    {
        image.sprite = picked_up_obj.GetComponent<Image>().sprite;
        picked_up_obj = picked_up_obj.gameObject;
        picked_up_obj.GetComponent<PartScript>().circuit_slot = gameObject;
        filled = true;
        linked_part = picked_up_obj;


        if (!did && HUDmanage.money >= picked_up_obj.GetComponent<PartScript>().cost)
        {
            HUDmanage.money -= picked_up_obj.GetComponent<PartScript>().cost;
            var ship = FindObjectOfType<PlayerWeapon>().current_ship.GetComponent<ShipStats>();
            for(int i = 0; i < ship.transform.childCount; i++)
            {
                if (ship.transform.GetChild(i).GetComponent<Health>() != null)
                {
                    ship.transform.GetChild(i).GetComponent<Health>().hp += picked_up_obj.GetComponent<PartScript>().armor_bonus;
                    ship.transform.GetChild(i).GetComponent<Health>().orig_hp = ship.transform.GetChild(i).GetComponent<Health>().hp;
                }
            }
            ship.dmg_bonus += picked_up_obj.GetComponent<PartScript>().dmg_bonus;            
            if (picked_up_obj.GetComponent<PartScript>().firerate_bonus > 0) { ship.firerate_bonus *= picked_up_obj.GetComponent<PartScript>().firerate_bonus; }
            ship.thrust_bonus += picked_up_obj.GetComponent<PartScript>().thrust_bonus;
            ship.turnspd_bonus += picked_up_obj.GetComponent<PartScript>().turnspd_bonus;

            ship.cargo_bonus += picked_up_obj.GetComponent<PartScript>().cargo_bonus;
            ship.mining_bonus += picked_up_obj.GetComponent<PartScript>().mining_bonus;
            ship.ore_bonus += picked_up_obj.GetComponent<PartScript>().ore_bonus;

            var partScript = picked_up_obj.GetComponent<PartScript>();

            var img = picked_up_obj.GetComponent<PartScript>().came_from.GetComponent<PartSlotScript>().part_numb;
            //var key = picked_up_obj.GetComponent<PartScript>().came_from.GetComponent<PartSlotScript>().station.GetComponent<ShipSpawner>().savekey;
            PlayerPrefs.SetInt("part number" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex(), img);
            PlayerPrefs.SetInt("filled" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex(), 1);
            

            //ship_stats.GetComponent<ShipStats>().images[transform.GetSiblingIndex()] = img;
            //ship_stats.GetComponent<ShipStats>().filled[transform.GetSiblingIndex()] = 1;

            picked_up_obj.GetComponent<PartScript>().came_from.GetComponent<PartSlotScript>().part = null;



            if (partScript.isBasicLaser) { PlayerWeapon.basic_lasers++; isBasicLaser = true; }
            if (partScript.isLaserRod) { PlayerWeapon.laser_rods++; isLaserRod = true; }
            if (partScript.isLaserBeam) { PlayerWeapon.laser_beams++; isLaserBeam = true; }


            did = true;
        }

        Destroy(picked_up_obj);
        
    }

    void DormantDisplay()
    {
        image.sprite = orig;
        filled = false;
        linked_part = null;
        picked_up_obj = null;
    }
}