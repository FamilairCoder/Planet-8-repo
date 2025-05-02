using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetDelivery : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject station, stationTo, BG, toText, toPay, towObj;
    private string stationName;
    public string key;
    private float pay, setTime, timesClicked;
    private bool did;
    
    // Start is called before the first frame update
    void Start()
    {
        
        station = transform.parent.transform.parent.GetComponent<MenuScript>().station;
        key = station.GetComponent<ShipSpawner>().savekey + "deliveryMenuObject" + transform.GetSiblingIndex();
        timesClicked = PlayerPrefs.GetFloat(key + "timesClicked", 0);

        //setTime = PlayerPrefs.GetFloat(key + "setTime" + timesClicked, 0);

        if (transform.parent.transform.parent.GetComponent<MenuScript>().isOutpost) HUDmanage.sd.Add(GetComponent<SetDelivery>());

        setTime = 60;
        


        
    }

    // Update is called once per frame
    public void MyUpdate()
    {
/*        if (!did)
        {
            var linked = station.GetComponent<StationStats>().linked_stations;
            if (PlayerPrefs.GetInt(key + "exists" + timesClicked, 0) == 1)
            {
                
                var stationNumb = PlayerPrefs.GetInt(key + "station" + timesClicked);
                stationTo = linked[stationNumb];

                var sprites = GetComponent<RandomSprite>().sprites;
                var spriteNumb = PlayerPrefs.GetInt(key + "spriteNumb" + timesClicked, Random.Range(0, sprites.Count));
                GetComponent<Image>().sprite = sprites[spriteNumb];

                stationName = stationTo.GetComponent<StationStats>().name;
                pay = Mathf.Round(Vector2.Distance(station.transform.position, stationTo.transform.position) / 60);
            }
            else
            {
                
                stationTo = null;
                
            }


            for (int i = 0; i < timesClicked; i++)
            {
                linked = station.GetComponent<StationStats>().linked_stations;

                var stationNumb = PlayerPrefs.GetInt(key + "station" + i, Random.Range(0, linked.Count));
                stationTo = linked[stationNumb];
                var sprites = GetComponent<RandomSprite>().sprites;
                var spriteNumb = PlayerPrefs.GetInt(key + "spriteNumb" + i, Random.Range(0, sprites.Count));

                pay = Mathf.Round(Vector2.Distance(station.transform.position, stationTo.transform.position) / 60);

                var t = Instantiate(towObj, FindObjectOfType<PlayerMining>().gameObject.transform.position, Quaternion.identity);
                t.GetComponent<SpriteRenderer>().sprite = sprites[spriteNumb];
                t.GetComponent<TowPartScript>().stationTo = stationTo;
                t.GetComponent<TowPartScript>().toPay = pay;
                PlayerTow.towObjects.Add(t);
                stationTo.GetComponent<SetNav>().deliverAmount += 1;
                ThingSpawner.totalDeliveries += 1;
                stationTo.GetComponent<SetNav>().SetBeacon();

                stationTo = null;

                PlayerPrefs.Save();
            }
            did = true;
        }*/

        if (!did)
        {
            var linked = station.GetComponent<StationStats>().linked_stations;

            if (PlayerPrefs.GetInt(key + "exists" + timesClicked, 0) == 1)
            {

                var stationNumb = PlayerPrefs.GetInt(key + "station" + timesClicked);
                stationTo = linked[stationNumb];

                var sprites = GetComponent<RandomSprite>().sprites;
                var spriteNumb = PlayerPrefs.GetInt(key + "spriteNumb" + timesClicked, Random.Range(0, sprites.Count));
                GetComponent<Image>().sprite = sprites[spriteNumb];

                stationName = stationTo.GetComponent<StationStats>().name;
                pay = Mathf.Round(Vector2.Distance(station.transform.position, stationTo.transform.position) / 60);
            }
            else
            {

                stationTo = null;
                setTime = 0;
            }



            for (int i = 1; i < timesClicked; i++)
            {
                // Check if this delivery was already completed
                //Debug.Log(PlayerPrefs.GetInt(key + "completed" + i, 0));
                //Debug.Log("Reading from " + i);
                if (PlayerPrefs.GetInt(key + "completed" + i, 0) == 1)
                {
                    continue; // Skip completed deliveries
                }

                // Process saved deliveries
                var stationNumb = PlayerPrefs.GetInt(key + "station" + i, Random.Range(0, linked.Count));
                stationTo = linked[stationNumb];
                var sprites = GetComponent<RandomSprite>().sprites;
                var spriteNumb = PlayerPrefs.GetInt(key + "spriteNumb" + i, Random.Range(0, sprites.Count));

                pay = Mathf.Round(Vector2.Distance(station.transform.position, stationTo.transform.position) / 60);

                // Instantiate delivery object
                //Debug.Log("Creating from last time");
                var t = Instantiate(towObj, FindObjectOfType<PlayerMining>().gameObject.transform.position, Quaternion.identity);
                t.GetComponent<SpriteRenderer>().sprite = sprites[spriteNumb];
                t.GetComponent<TowPartScript>().stationTo = stationTo;
                t.GetComponent<TowPartScript>().toPay = pay;
                t.GetComponent<TowPartScript>().linkedDelivery = GetComponent<SetDelivery>();
                PlayerTow.towObjects.Add(t);

                // Update station stats
                stationTo.GetComponent<SetNav>().deliverAmount += 1;
                ThingSpawner.totalDeliveries += 1;
                stationTo.GetComponent<SetNav>().SetBeacon();

                // Update station stats


                stationTo = null;

                PlayerPrefs.Save();
            }
            did = true;
        }

        if (stationTo == null)
        {
            setTime -= Time.deltaTime;
           // PlayerPrefs.SetFloat(key + "setTime" + timesClicked, setTime);
            if (setTime < 0 && station.GetComponent<StationStats>().linked_stations.Count > 0)
            {
                
                var linked = station.GetComponent<StationStats>().linked_stations;

                var stationNumb = PlayerPrefs.GetInt(key + "station" + timesClicked, Random.Range(0, linked.Count));
                stationTo = linked[stationNumb];
                PlayerPrefs.SetInt(key + "station" + timesClicked, stationNumb);

                var sprites = GetComponent<RandomSprite>().sprites;
                var spriteNumb = PlayerPrefs.GetInt(key + "spriteNumb" + timesClicked, Random.Range(0, sprites.Count));
                GetComponent<Image>().sprite = sprites[spriteNumb];
                PlayerPrefs.SetInt(key + "spriteNumb" + timesClicked, spriteNumb);

                //GetComponent<RandomSprite>().RandomizeSprite();
                stationName = stationTo.GetComponent<StationStats>().name;
                pay = Mathf.Round(Vector2.Distance(station.transform.position, stationTo.transform.position) / 60);

                GetComponent<Image>().enabled = true;
                PlayerPrefs.SetInt(key + "exists" + timesClicked, 1);
                setTime = Random.Range(60f, 180f);
                //PlayerPrefs.SetFloat(key + "setTime" + timesClicked, setTime);
                PlayerPrefs.Save();
            }

        }
        toText.GetComponent<TextMeshProUGUI>().text = "To " + stationName;
        toPay.GetComponent<TextMeshProUGUI>().text = "Pays "+ pay.ToString() + " photons";
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        // Create towed part
        var t = Instantiate(towObj, FindObjectOfType<PlayerMining>().gameObject.transform.position, Quaternion.identity);
        t.GetComponent<SpriteRenderer>().sprite = GetComponent<Image>().sprite;
        t.GetComponent<TowPartScript>().stationTo = stationTo;
        t.GetComponent<TowPartScript>().toPay = pay;
        t.GetComponent<TowPartScript>().linkedDelivery = GetComponent<SetDelivery>();
        PlayerTow.towObjects.Add(t);

        // Update delivery stats
        stationTo.GetComponent<SetNav>().deliverAmount += 1;
        ThingSpawner.totalDeliveries += 1;
        stationTo.GetComponent<SetNav>().SetBeacon();



        stationTo = null;
        GetComponent<Image>().enabled = false;
        BG.SetActive(false);
        toText.SetActive(false);
        toPay.SetActive(false);

        timesClicked++;
        PlayerPrefs.SetFloat(key + "timesClicked", timesClicked);
        PlayerPrefs.SetInt(key + "exists" + timesClicked, 0);
        PlayerPrefs.Save();

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GetComponent<Image>().enabled)
        {
            BG.SetActive(true);
            toText.SetActive(true);
            toPay.SetActive(true);
        }

    }
    public void OnPointerExit(PointerEventData eventData)
    {

        BG.SetActive(false);
        toText.SetActive(false);
        toPay.SetActive(false);
        
    }



    public void CompleteDelivery()
    {
       // Debug.Log("Saved at " + timesClicked);
        //Debug.Log(PlayerPrefs.GetInt(key + "completed" + 0, 0));
        PlayerPrefs.SetInt(key + "completed" + timesClicked, 1); // Mark as completed
        PlayerPrefs.Save();
    }
}



