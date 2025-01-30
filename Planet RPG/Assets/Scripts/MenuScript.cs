using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public bool active;
    public Vector2 away_pos, active_pos, circuit_view_pos;
    public GameObject station;
    private GameObject player;
    public bool dontSpawnCV;
    public bool isOutpost;
    public float restockAdd;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMining>().gameObject;
        StartCoroutine(Delay());
        restockAdd = PlayerPrefs.GetFloat(station.GetComponent<ShipSpawner>().savekey + "menu restock addition", 0);
        StartCoroutine(RestockTime());
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            //GetComponent<RectTransform>().localPosition = Vector2.Lerp(GetComponent<RectTransform>().localPosition, active_pos, .1f);
            GetComponent<RectTransform>().localPosition = active_pos;
            if (Input.GetKeyDown(KeyCode.Escape) || Vector2.Distance(station.transform.position, player.transform.position) > 150 || HUDmanage.on_map)
            {
                active = false;
                GetComponent<RectTransform>().localPosition = away_pos;
                gameObject.SetActive(false);

            }
        }
        else
        {
            GetComponent<RectTransform>().localPosition = Vector2.Lerp(GetComponent<RectTransform>().localPosition, away_pos, .1f);
            
        }
    }


    private IEnumerator Delay()
    {


        yield return new WaitForSeconds(.3f);

        gameObject.SetActive(false);
    }

    private IEnumerator RestockTime()
    {

        while (true)
        {
            PlayerPrefs.SetFloat(station.GetComponent<ShipSpawner>().savekey + "menu restock addition", restockAdd);
            PlayerPrefs.Save();
            if (restockAdd > 0) restockAdd--;
            yield return new WaitForSeconds(10f);
        }
        

        //gameObject.SetActive(false);
    }

}
