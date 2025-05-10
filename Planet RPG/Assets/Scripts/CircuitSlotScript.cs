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
    public GameObject sellHighlight;
    private float savedCost;
    private float dmg_bonus, firerate_bonus, thrust_bonus, turnspd_bonus, cargo_bonus, mining_bonus, ore_bonus;
    public AudioClip sellSFX;
    // Start is called before the first frame update
    void Start()
    {
        orig = image.sprite;


        var key = transform.parent.transform.parent.GetComponent<CVstationLink>().station.GetComponent<ShipSpawner>().savekey;


        if (!isBasicLaser && !isLaserBeam && !isLaserRod)
        {
            image.sprite = part_images[SaveManager.GetInt("part number" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex(), part_images.Count - 1)];
        }



        if (SaveManager.GetInt("filled" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex(), 0) == 1)
        {
            filled = true;

        }

        if (isBasicLaser) { PlayerWeapon.basic_lasers++; SaveManager.SetInt("part number" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex(), 0); }
        if (isLaserRod) { PlayerWeapon.laser_rods++; SaveManager.SetInt("part number" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex(), 1); }
        if (isLaserBeam) { PlayerWeapon.laser_beams++; SaveManager.SetInt("part number" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex(), 2); }


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






            if (SaveManager.GetInt("filled" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex()) == 1)//, 0) == 1)
            {
                filled = true;                
            }

            if (!filled)
            {
                image.sprite = part_images[SaveManager.GetInt("part number" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex(), part_images.Count - 1)];
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
        if (sellHighlight != null)
        {
            if (SellButtonScript.sell && filled)
            {
                sellHighlight.SetActive(true);
            }
            else
            {
                sellHighlight.SetActive(false);
            }
        }



    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (sellHighlight != null)
        {
            if (SellButtonScript.sell && filled)
            {
                HUDmanage.money += SaveManager.GetFloat("cost" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex()) * .7f;
                filled = false;
                linked_part = null;
                picked_up_obj = null;                
                image.sprite = orig;
                did = false;
                GUIAudioplayer.clip = sellSFX;
                SaveManager.SetFloat("cost" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex(), 0);
                SaveManager.SetInt("filled" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex(), 0);
                SaveManager.SetInt("part number" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex(), part_images.Count - 1);

                var ship = FindObjectOfType<PlayerWeapon>().current_ship.GetComponent<ShipStats>();
                ship.dmg_bonus -= SaveManager.GetFloat("dmg_bonus" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex());
                ship.armor_bonus -= SaveManager.GetFloat("armor_bonus" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex());
                ship.firerate_bonus -= SaveManager.GetFloat("firerate_bonus" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex());
                ship.thrust_bonus -= SaveManager.GetFloat("thrust_bonus" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex());
                ship.turnspd_bonus -= SaveManager.GetFloat("turnspd_bonus" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex());
                
                ship.cargo_bonus -= SaveManager.GetFloat("cargo_bonus" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex());
                ship.mining_bonus -= SaveManager.GetFloat("mining_bonus" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex());
                ship.ore_bonus -= SaveManager.GetFloat("ore_bonus" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex());

                ship.energyRegenBonus -= SaveManager.GetFloat("regenBonus" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex());
                ship.energyCapacityBonus -= SaveManager.GetFloat("capacityBonus" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex());

                for (int i = 0; i < ship.transform.childCount; i++)
                {
                    if (ship.transform.GetChild(i).GetComponent<Health>() != null)
                    {
                        var healthScript = ship.transform.GetChild(i).GetComponent<Health>();
                        if (healthScript.hp == healthScript.orig_hp || !ship.transform.GetChild(i).gameObject.activeSelf)
                        {
                            healthScript.hp -= SaveManager.GetFloat("armor_bonus" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex());
                            healthScript.orig_hp = healthScript.hp;
                        }
                        else if (healthScript.hp < healthScript.orig_hp)
                        {
                            healthScript.orig_hp -= SaveManager.GetFloat("armor_bonus" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex());
                        }
                        if (ship.transform.GetChild(i).gameObject.activeSelf) ship.origHp -= SaveManager.GetFloat("armor_bonus" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex());

                    }
                }

                SaveBonus("armor_bonus", 0);
                SaveBonus("dmg_bonus", 0);
                SaveBonus("firerate_bonus", 0);
                SaveBonus("thrust_bonus", 0);
                SaveBonus("turnspd_bonus", 0);

                SaveBonus("cargo_bonus", 0);
                SaveBonus("mining_bonus", 0);
                SaveBonus("ore_bonus", 0);

                SaveBonus("regenBonus", 0);
                SaveBonus("capacityBonus", 0);





                
            }
        }


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
        SaveManager.SetFloat("cost" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex(), picked_up_obj.GetComponent<PartScript>().cost);
        
        if (!did && HUDmanage.money >= picked_up_obj.GetComponent<PartScript>().cost)
        {
            HUDmanage.money -= picked_up_obj.GetComponent<PartScript>().cost;
            var ship = FindObjectOfType<PlayerWeapon>().current_ship.GetComponent<ShipStats>();
            //ship.origHp = 0;
            for (int i = 0; i < ship.transform.childCount; i++)
            {
                if (ship.transform.GetChild(i).GetComponent<Health>() != null)
                {
                    var healthScript = ship.transform.GetChild(i).GetComponent<Health>();
                    if (healthScript.hp == healthScript.orig_hp || !ship.transform.GetChild(i).gameObject.activeSelf)
                    {
                        healthScript.hp += picked_up_obj.GetComponent<PartScript>().armor_bonus;
                        healthScript.orig_hp = healthScript.hp;
                    }
                    else if (healthScript.hp < healthScript.orig_hp)
                    {
                        healthScript.orig_hp += picked_up_obj.GetComponent<PartScript>().armor_bonus;
                    }
                    //ship.origHp += picked_up_obj.GetComponent<PartScript>().armor_bonus;
                }
            }

            var partScript = picked_up_obj.GetComponent<PartScript>();


            ship.armor_bonus += partScript.armor_bonus;            
            ship.dmg_bonus += partScript.dmg_bonus;            
            ship.firerate_bonus += partScript.firerate_bonus;
            ship.thrust_bonus += partScript.thrust_bonus;
            ship.turnspd_bonus += partScript.turnspd_bonus;

            ship.cargo_bonus += partScript.cargo_bonus;
            ship.mining_bonus += partScript.mining_bonus;
            ship.ore_bonus += partScript.ore_bonus;

            ship.energyRegenBonus += partScript.energyRegenBonus;
            ship.energyCapacityBonus += partScript.energyCapacityBonus;


            SaveBonus("armor_bonus", partScript.armor_bonus);
            SaveBonus("dmg_bonus", partScript.dmg_bonus);
            SaveBonus("firerate_bonus", partScript.firerate_bonus);
            SaveBonus("thrust_bonus", partScript.thrust_bonus);
            SaveBonus("turnspd_bonus", partScript.turnspd_bonus);

            SaveBonus("cargo_bonus", partScript.cargo_bonus);
            SaveBonus("mining_bonus", partScript.mining_bonus);
            SaveBonus("ore_bonus", partScript.ore_bonus);

            SaveBonus("regenBonus", partScript.energyRegenBonus);
            SaveBonus("capacityBonus", partScript.energyCapacityBonus);

            



            var img = picked_up_obj.GetComponent<PartScript>().came_from.GetComponent<PartSlotScript>().part_numb;
            //var key = picked_up_obj.GetComponent<PartScript>().came_from.GetComponent<PartSlotScript>().station.GetComponent<ShipSpawner>().savekey;
            SaveManager.SetInt("part number" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex(), img);
            SaveManager.SetInt("filled" + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex(), 1);
            

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


    void SaveBonus(string bonusText, float bonus)
    {
        SaveManager.SetFloat(bonusText + transform.GetSiblingIndex() + transform.parent.GetSiblingIndex(), bonus);
    }
}