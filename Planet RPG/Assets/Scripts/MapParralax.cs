using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.U2D.Animation;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

public class MapParralax : MonoBehaviour
{
    public GameObject track_button, deliveryText, linked_obj;
    public bool rotate, dontScale;
    public int deliveryAmount;
    public string savekey;
    private bool did;
    [Header("dont have to set--------------")]
    public bool on;
    private bool tracking;
    private GameObject player;
    private Vector3 origScale;
    private float origCamScale;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt(savekey + "enabled", 0) == 1) on = true;
        if (PlayerPrefs.GetInt(savekey + "tracking", 0) == 1) tracking = true;
        player = FindObjectOfType<PlayerMining>().gameObject;
        origScale = transform.localScale;
        origCamScale = 1500;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().enabled = HUDmanage.on_map;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<SpriteRenderer>() != null) transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = HUDmanage.on_map;
        }
        if (!HUDmanage.on_map)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<TextMeshPro>() != null) transform.GetChild(i).GetComponent<TextMeshPro>().enabled = false;
            }
        }

            

        if (!on)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<SpriteRenderer>() != null) transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
            }

            for (int i = 0; i < transform.childCount; i++)
            {

                if (transform.GetChild(i).GetComponent<TextMeshPro>() != null) transform.GetChild(i).GetComponent<TextMeshPro>().enabled = false;
            }
                
        }
        else if (!did)
        {
            PlayerPrefs.SetInt(savekey + "enabled", 1);
            did = true;
        }
        if (rotate) transform.rotation = linked_obj.transform.rotation;
        if (tracking)
        {
            PlayerPrefs.SetInt(savekey + "tracking", 1);

            var player_pos = player.transform.position;
            var other_pos = linked_obj.transform.position;
    

            GetComponent<LineRenderer>().enabled = true;
            GetComponent<LineRenderer>().SetPosition(0, player_pos);
            GetComponent<LineRenderer>().SetPosition(1, other_pos);

            if (Vector2.Distance(player_pos, other_pos) < 30) tracking = false;

            if (HUDmanage.on_map) track_button.GetComponent<TextMeshPro>().enabled = true;
            track_button.GetComponent<TextMeshPro>().text = "Tracking";
        }
        else if (GetComponent<LineRenderer>() != null && track_button != null)
        {
            PlayerPrefs.SetInt(savekey + "tracking", 0);
            GetComponent<LineRenderer>().enabled = false;
            track_button.GetComponent<TextMeshPro>().text = "Track";
        }
        if (deliveryText != null) { deliveryText.GetComponent<TextMeshPro>().text = "Deliveries: " + deliveryAmount.ToString(); }


        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        var camSize = Camera.main.orthographicSize;
        
        if (!dontScale)// && camSize > 500 && camSize < 10000)
        {
            transform.localScale = (camSize / origCamScale) * origScale;
            transform.localScale = origScale * (camSize/origCamScale);
/*            if (scrollWheel < 0)
            {
                transform.localScale += Vector3.one * 60;
            }
            else if (scrollWheel > 0)
            {
                transform.localScale -= Vector3.one * 60;
            }
            if (transform.localScale.x < origScale.x)
            {
                transform.localScale = origScale;
            }*/
        }


    }

    void FixedUpdate()
    {
        transform.position = new Vector3(linked_obj.transform.position.x, linked_obj.transform.position.y, -6);
    


    }

    private void OnMouseOver()
    {
        if (on && GetComponent<SpriteRenderer>().enabled) 
        {
            track_button.GetComponent<TextMeshPro>().enabled = true;
            if (deliveryText != null) deliveryText.GetComponent<TextMeshPro>().enabled = true;
            if (Input.GetMouseButtonDown(0))
            {
                if (!tracking) tracking = true;
                else tracking = false;
            }
        }
    }
    private void OnMouseExit()
    {
        track_button.GetComponent<TextMeshPro>().enabled = false;
        if (deliveryText != null) deliveryText.GetComponent<TextMeshPro>().enabled = false;
        
    }
}
