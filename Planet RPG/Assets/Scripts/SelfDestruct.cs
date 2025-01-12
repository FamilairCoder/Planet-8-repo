using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelfDestruct : MonoBehaviour, IPointerDownHandler
{
    public GameObject explosion;
    private GameObject player;
    public ShipStats shipStats;
    private float checkTime, amount_broken;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMining>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (shipStats != null)
        {
            checkTime -= Time.deltaTime;
            if (checkTime < 0)
            {
                if (shipStats != null)
                {
                    amount_broken = 0;
                    foreach (var t in shipStats.thrusters)
                    {
                        if (t.activeSelf)
                        {
                            if (t.GetComponent<Health>() != null && t.GetComponent<Health>().hp <= 0)
                            {
                                amount_broken++;
                            }
                        }

                    }

                }
                checkTime = .2f;
            }
   
        }
        if (amount_broken == shipStats.thrusters.Count) { GetComponent<Image>().enabled = true; transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = true; }
        else { GetComponent<Image>().enabled = false; transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = false; }

    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (amount_broken == shipStats.thrusters.Count)
        {

            shipStats.core.GetComponent<Health>().hp = 0;
            Instantiate(explosion, player.transform.position, Quaternion.identity);

        }

    }
}
