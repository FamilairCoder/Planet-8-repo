using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowPartScript : MonoBehaviour
{
    public GameObject stationTo, deliveryText;
    private GameObject player;
    public SetDelivery linkedDelivery;
    public float toPay;
    private float checkTime;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerTow>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        checkTime -= Time.deltaTime;
        if (checkTime < 0)
        {
            if (Vector2.Distance(player.transform.position, stationTo.transform.position) < 30)
            {
                HUDmanage.money += toPay;
                Instantiate(deliveryText, transform.position, Quaternion.identity).GetComponent<ErrorTextDisappear>().offsetPos = transform.position;
                stationTo.GetComponent<SetNav>().deliverAmount -= 1;
                ThingSpawner.totalDeliveries -= 1;
                linkedDelivery.CompleteDelivery();
                Destroy(gameObject);
            }
            checkTime = .1f;
            if (PlayerMovement.dead) 
            {
                stationTo.GetComponent<SetNav>().deliverAmount -= 1;
                ThingSpawner.totalDeliveries -= 1;
                linkedDelivery.CompleteDelivery();
                Destroy(gameObject);             
            }
        }
    }
}
