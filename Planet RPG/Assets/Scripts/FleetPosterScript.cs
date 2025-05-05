using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FleetPosterScript : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI costText;
    public Sprite picked_ind;
    public GameObject target_ind, bg_ind, leaderImage;
    public List<GameObject> shipImages = new List<GameObject>();
    public float lvl, totalBounty;
    public TextMeshProUGUI outpostLeftText;
    private FleetShipSpawner linkedSpawner;
    private float timeleft;
    private bool picked, did;
    // Start is called before the first frame update
    void Start()
    {
        linkedSpawner = GetComponentInParent<MenuScript>().station.GetComponent<FleetShipSpawner>();
        if (SaveManager.GetFloat(linkedSpawner.key + "fleetposter picked" + lvl, 0) == 1)
        {
            if (lvl == 1) linkedSpawner.createdLvl1Nav.GetComponent<MapParralax>().on = true;
            if (lvl == 2) linkedSpawner.createdLvl2Nav.GetComponent<MapParralax>().on = true;
            if (lvl == 3) linkedSpawner.createdLvl3Nav.GetComponent<MapParralax>().on = true;
            if (lvl == 4)
            {
                var outpostList = linkedSpawner.createdOutpostNavObjs;
                for (int i = 0; i < outpostList.Count; i++)
                {
                    outpostList[i].GetComponent<MapParralax>().on = true;
                }
            }
            if (lvl == 5)
            {
                linkedSpawner.linkedStation.GetComponent<MapParralax>().on = true;
            }
            picked = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeleft -= Time.deltaTime;
        if (timeleft < 0)
        {
            if (lvl < 4)
            {
                var cost = 0f;
                SquadLocationSetter ship = null;
                if (lvl == 1 && linkedSpawner.chosenLvl1 != null)
                {
                    leaderImage.GetComponent<Image>().sprite = linkedSpawner.chosenLvl1.GetComponent<SpriteRenderer>().sprite;
                    ship = linkedSpawner.chosenLvl1.GetComponent<SquadLocationSetter>();
                    cost += linkedSpawner.chosenLvl1.GetComponent<NPCmovement>().bounty_cost;
                }
                else if (lvl == 2 && linkedSpawner.chosenLvl2 != null)
                {
                    leaderImage.GetComponent<Image>().sprite = linkedSpawner.chosenLvl2.GetComponent<SpriteRenderer>().sprite;
                    ship = linkedSpawner.chosenLvl2.GetComponent<SquadLocationSetter>();
                    cost += linkedSpawner.chosenLvl2.GetComponent<NPCmovement>().bounty_cost;
                }
                else if (lvl == 3 && linkedSpawner.chosenLvl3 != null)
                {
                    leaderImage.GetComponent<Image>().sprite = linkedSpawner.chosenLvl3.GetComponent<SpriteRenderer>().sprite;
                    ship = linkedSpawner.chosenLvl3.GetComponent<SquadLocationSetter>();
                    cost += linkedSpawner.chosenLvl3.GetComponent<NPCmovement>().bounty_cost;
                }

                for (int i = 0; i < shipImages.Count; i++)
                {
                    if (ship != null && ship.shipsSpawned.Count > i && ship.shipsSpawned[i] != null)
                    {
                        shipImages[i].GetComponent<Image>().sprite = ship.shipsSpawned[i].GetComponent<SpriteRenderer>().sprite;
                        cost += ship.shipsSpawned[i].GetComponent<NPCmovement>().bounty_cost;
                    }

                }

                costText.text = (cost * 1.5f).ToString() + " P";
            }

            else if (outpostLeftText != null)
            {
                var count = 0;
                for (int i = 0;i < linkedSpawner.linkedOutposts.Count;i++)
                {
                    if (linkedSpawner.linkedOutposts[i] != null) count++;
                }
                outpostLeftText.text = count.ToString() + " left";
            }


            timeleft = .5f;

        }
        if (picked)
        {
            target_ind.SetActive(false);
            bg_ind.SetActive(false);
            GetComponent<Image>().sprite = picked_ind;
        }
        
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        picked = true;
        
        if (lvl == 1) linkedSpawner.createdLvl1Nav.GetComponent<MapParralax>().on = true;
        if (lvl == 2) linkedSpawner.createdLvl2Nav.GetComponent<MapParralax>().on = true;
        if (lvl == 3) linkedSpawner.createdLvl3Nav.GetComponent<MapParralax>().on = true;
        if (lvl == 4)
        {
            var outpostList = linkedSpawner.createdOutpostNavObjs;
            for (int i = 0; i < outpostList.Count; i++)
            {
                outpostList[i].GetComponent<MapParralax>().on = true;
            }
        }
        if (lvl == 5)
        {
            linkedSpawner.linkedStation.GetComponent<MapParralax>().on = true;
        }
        SaveManager.SetFloat(linkedSpawner.key + "fleetposter picked" + lvl, 1);
        
        //Debug.Log(SaveManager.GetInt(key + "taken"));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

            target_ind.SetActive(true);
            bg_ind.SetActive(true);
        

    }
    public void OnPointerExit(PointerEventData eventData)
    {

            target_ind.SetActive(false);
            bg_ind.SetActive(false);
        

    }


}
