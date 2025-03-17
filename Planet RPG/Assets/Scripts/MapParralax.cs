/*using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.U2D.Animation;
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
    private bool tracking, checkDid = true, onDid;
    private GameObject player;
    private Vector3 origScale;
    private float origCamScale;
    private LineRenderer lr;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt(savekey + "enabled", 0) == 1) on = true;
        if (PlayerPrefs.GetInt(savekey + "tracking", 0) == 1) tracking = true;
        player = FindObjectOfType<PlayerMining>().gameObject;
        origScale = transform.localScale;
        origCamScale = 1500;
        if (GetComponent<LineRenderer>() != null) lr = GetComponent<LineRenderer>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (on)
        {
            if (!did)
            {
                PlayerPrefs.SetInt(savekey + "enabled", 1);
                did = true;
            }
            onDid = false;
            if (HUDmanage.on_map && !checkDid)
            {
                GetComponent<SpriteRenderer>().enabled = true;
                if (transform.childCount > 0)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        if (transform.GetChild(i).GetComponent<SpriteRenderer>() != null) transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
                    }
                }
                checkDid = true;
            }
            else if (!HUDmanage.on_map && checkDid)
            {
                GetComponent<SpriteRenderer>().enabled = false;
                if (transform.childCount > 0)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        if (transform.GetChild(i).GetComponent<SpriteRenderer>() != null) transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
                        else if (transform.GetChild(i).GetComponent<TextMeshPro>() != null) transform.GetChild(i).GetComponent<TextMeshPro>().enabled = false;
                    }
                }
                checkDid = false;
            }
        }
        else if (!on && !onDid)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<SpriteRenderer>() != null) transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
                else if (transform.GetChild(i).GetComponent<TextMeshPro>() != null) transform.GetChild(i).GetComponent<TextMeshPro>().enabled = false;
            }
            onDid = true;
        }



        //GetComponent<SpriteRenderer>().enabled = HUDmanage.on_map;

*//*        if (!HUDmanage.on_map)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<TextMeshPro>() != null) transform.GetChild(i).GetComponent<TextMeshPro>().enabled = false;
            }
        }*/



/*        if (!on)
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

        }*//*
        if (rotate) transform.rotation = linked_obj.transform.rotation;
        if (tracking)
        {
            PlayerPrefs.SetInt(savekey + "tracking", 1);

            var player_pos = player.transform.position;
            var other_pos = linked_obj.transform.position;


            lr.enabled = true;
            lr.SetPosition(0, player_pos);
            lr.SetPosition(1, other_pos);

            if (Vector2.Distance(player_pos, other_pos) < 30) tracking = false;

            if (HUDmanage.on_map) track_button.GetComponent<TextMeshPro>().enabled = true;
            track_button.GetComponent<TextMeshPro>().text = "Tracking";
        }
        else if (GetComponent<LineRenderer>() != null && track_button != null)
        {
            PlayerPrefs.SetInt(savekey + "tracking", 0);
            lr.enabled = false;
            track_button.GetComponent<TextMeshPro>().text = "Track";
        }
        if (deliveryText != null) { deliveryText.GetComponent<TextMeshPro>().text = "Deliveries: " + deliveryAmount.ToString(); }


        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        var camSize = Camera.main.orthographicSize;
        
        if (!dontScale)// && camSize > 500 && camSize < 10000)
        {
            transform.localScale = (camSize / origCamScale) * origScale;
            transform.localScale = origScale * (camSize/origCamScale);
*//*            if (scrollWheel < 0)
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
            }*//*
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
}*/




using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapParralax : MonoBehaviour
{
    public GameObject track_button, deliveryText, linked_obj;
    public bool rotate, dontScale;
    public int deliveryAmount;
    public string savekey;

    private bool did;
    private bool tracking, checkDid = true, onDid;
    private GameObject player;
    private Vector3 origScale;
    private float origCamScale;
    private LineRenderer lr;

    [Header("State Flags")]
    public bool on;
    private bool mapJustOpened = true;

    void Start()
    {
        if (!on) on = PlayerPrefs.GetInt(savekey + "enabled", 0) == 1;
        tracking = PlayerPrefs.GetInt(savekey + "tracking", 0) == 1;
        player = FindObjectOfType<PlayerMining>().gameObject;
        origScale = transform.localScale;
        origCamScale = 1500;

        if (GetComponent<LineRenderer>() != null)
            lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (on)
        {
            HandleActivationState();
        }
        else if (!on && !onDid)
        {
            DisableAllChildComponents();
            onDid = true;
        }

        if (rotate)
            transform.rotation = linked_obj.transform.rotation;

        HandleTracking();
        HandleDeliveryText();
        AdjustScale();
    }

    void FixedUpdate()
    {
        transform.position = new Vector3(linked_obj.transform.position.x, linked_obj.transform.position.y, -6);
    }

    private void OnMouseOver()
    {
        if (on && GetComponent<SpriteRenderer>().enabled)
        {
            if (!mapJustOpened) // Ensure texts only appear after the map is fully opened
            {
                track_button.GetComponent<TextMeshPro>().enabled = true;
                if (deliveryText != null)
                    deliveryText.GetComponent<TextMeshPro>().enabled = true;
            }

            if (Input.GetMouseButtonDown(0))
                tracking = !tracking;
        }
    }

    private void OnMouseExit()
    {
        track_button.GetComponent<TextMeshPro>().enabled = false;
        if (deliveryText != null)
            deliveryText.GetComponent<TextMeshPro>().enabled = false;
    }

    private void HandleActivationState()
    {
        if (!did)
        {
            PlayerPrefs.SetInt(savekey + "enabled", 1);
            did = true;
        }

        onDid = false;
        if (HUDmanage.on_map && !checkDid)
        {
            EnableMapComponents();
            mapJustOpened = false; // Reset map visibility flag once components are enabled
            checkDid = true;
        }
        else if (!HUDmanage.on_map && checkDid)
        {
            DisableAllChildComponents();
            mapJustOpened = true; // Set flag to true when map is closed
            checkDid = false;
        }
    }

    private void HandleTracking()
    {
        if (tracking)
        {
            PlayerPrefs.SetInt(savekey + "tracking", 1);

            Vector3 playerPos = player.transform.position;
            Vector3 targetPos = linked_obj.transform.position;

            lr.enabled = true;
            lr.SetPosition(0, playerPos);
            lr.SetPosition(1, targetPos);

            if (Vector2.Distance(playerPos, targetPos) < 40)
                tracking = false;

            if (HUDmanage.on_map && !mapJustOpened)
                track_button.GetComponent<TextMeshPro>().enabled = true;

            track_button.GetComponent<TextMeshPro>().text = "Tracking";
        }
        else if (lr != null && track_button != null)
        {
            PlayerPrefs.SetInt(savekey + "tracking", 0);
            lr.enabled = false;
            track_button.GetComponent<TextMeshPro>().text = "Track";
        }
    }

    private void HandleDeliveryText()
    {
        if (deliveryText != null)
            deliveryText.GetComponent<TextMeshPro>().text = "Deliveries: " + deliveryAmount.ToString();
    }

    private void AdjustScale()
    {
        if (!dontScale)
        {
            float camSize = Camera.main.orthographicSize;
            transform.localScale = origScale * (camSize / origCamScale);
        }
    }

    private void EnableMapComponents()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (child.GetComponent<SpriteRenderer>() != null)
                child.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    private void DisableAllChildComponents()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (child.GetComponent<SpriteRenderer>() != null)
                child.GetComponent<SpriteRenderer>().enabled = false;
            else if (child.GetComponent<TextMeshPro>() != null)
                child.GetComponent<TextMeshPro>().enabled = false;
        }
    }
}
