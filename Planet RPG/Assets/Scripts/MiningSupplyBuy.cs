using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MiningSupplyBuy : MonoBehaviour, IPointerDownHandler
{
    public int cost;
    private int buypart_index;
    public GameObject buy_part;
    //public bool add_probe, increase_price;
    private float delay_time;
    private bool did_delay;
    private HUDmanage HUD;
    // Start is called before the first frame update
    void Start()
    {
        HUD = FindObjectOfType<HUDmanage>();
        delay_time = .01f;
        for(int i = 0; i < HUD.secondaries.Count; i++)
        {
            if (HUD.secondaries[i].name == buy_part.name) buypart_index = i;

        }
        //Debug.Log(buypart_index);
        if (SaveManager.GetInt("part " + buypart_index + " bought", 0) == 1)
        {
            //if (buy_part.name == "AccumulatorIcon") Debug.Log(buy_part.name);
            BuyPart(SaveManager.GetInt("part index" + buypart_index));
        }
    }

    // Update is called once per frame
    void Update()
    {
        delay_time -= Time.deltaTime;
        if (!did_delay && delay_time <= 0f)
        {

            did_delay = true;
        }
        //if (increase_price) transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = cost.ToString() + " photons";
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (HUDmanage.money >= cost)
        {
            if (SaveManager.GetInt("part " + buypart_index + " bought", 0) == 0)
            {
                BuyPart();
                HUDmanage.money -= cost;
            }
/*            else if (add_probe)
            {
                if (SaveManager.GetInt("part " + buypart_index + " bought", 0) == 0)
                {
                    HUD.has_secondary[buypart_index] = true;
                    HUD.code_has_secondary.Add(buy_part);
                    HUD.PublicGotNew();
                    SaveManager.SetInt("part " + buypart_index + " bought", 1);
                }

                ProbeUIScript.probe_amount++;
                HUDmanage.money -= cost;
                if (increase_price) cost += 50;
            }*/

        }

    }

    void BuyPart()
    {
        HUD.has_secondary[buypart_index] = true;
        SaveManager.SetInt("part index" + buypart_index, HUD.code_has_secondary.Count);
        HUD.code_has_secondary.Add(buy_part);
        HUD.PublicGotNew();
        SaveManager.SetInt("part " + buypart_index + " bought", 1);
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Bought";
    }
    void BuyPart(int index)
    {
        HUD.has_secondary[buypart_index] = true;
        var i = 0;
        while (HUD.code_has_secondary.Count <= index)
        {
            HUD.code_has_secondary.Add(null);
            //if (buy_part.name == "AccumulatorIcon") Debug.Log("added null at " + i);
            i++;
        }
        //if (buy_part.name == "AccumulatorIcon") Debug.Log("Inserting at " + index);
        HUD.code_has_secondary[index] = buy_part;
        HUD.index = index;
        HUD.PublicGotNew();
        SaveManager.SetInt("part " + buypart_index + " bought", 1);
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Bought";
    }
}
