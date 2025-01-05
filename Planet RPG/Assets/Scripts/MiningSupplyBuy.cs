using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MiningSupplyBuy : MonoBehaviour, IPointerDownHandler
{
    public int cost, buypart_index;
    public GameObject buy_part;
    public bool add_probe, increase_price;
    private float delay_time;
    private bool did_delay;
    // Start is called before the first frame update
    void Start()
    {
        delay_time = Random.Range(0f, .1f);

    }

    // Update is called once per frame
    void Update()
    {
        delay_time -= Time.deltaTime;
        if (!did_delay && delay_time <= 0f)
        {
            if (PlayerPrefs.GetInt("part " + buypart_index + " bought", 0) == 1)
            {
                FindObjectOfType<HUDmanage>().has_secondary[buypart_index] = true;
                FindObjectOfType<HUDmanage>().code_has_secondary.Add(buy_part);
                FindObjectOfType<HUDmanage>().PublicGotNew();
                if (!add_probe) transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Bought";
                if (add_probe) ProbeUIScript.probe_amount = PlayerPrefs.GetFloat("probes", 0);
            }
            did_delay = true;
        }
        if (increase_price) transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = cost.ToString() + " photons";
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (HUDmanage.money >= cost)
        {
            if (!add_probe && PlayerPrefs.GetInt("part " + buypart_index + " bought", 0) == 0)
            {
                FindObjectOfType<HUDmanage>().has_secondary[buypart_index] = true;
                FindObjectOfType<HUDmanage>().code_has_secondary.Add(buy_part);
                FindObjectOfType<HUDmanage>().PublicGotNew();
                PlayerPrefs.SetInt("part " + buypart_index + " bought", 1);
                transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Bought";
                HUDmanage.money -= cost;
            }
            else if (add_probe)
            {
                if (PlayerPrefs.GetInt("part " + buypart_index + " bought", 0) == 0)
                {
                    FindObjectOfType<HUDmanage>().has_secondary[buypart_index] = true;
                    FindObjectOfType<HUDmanage>().code_has_secondary.Add(buy_part);
                    FindObjectOfType<HUDmanage>().PublicGotNew();
                    PlayerPrefs.SetInt("part " + buypart_index + " bought", 1);
                }

                ProbeUIScript.probe_amount++;
                HUDmanage.money -= cost;
                if (increase_price) cost += 50;
            }

        }

    }
}
