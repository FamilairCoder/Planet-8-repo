using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BountyPosterScript : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject picture_obj, cost_text, dist_obj, track_obj, tied_enemy, tied_poster, player, target_ind, cancel_ind, station;
    public Sprite picture;
    
    public float cost;
    public bool on_screen;

    public bool close;
    private bool clicked, delayed, playerKilled;
    public string key;
    public string keyString;
    public float delayTime;
    // Start is called before the first frame update
    void Start()
    {
        
        //delayTime = Random.Range(0f, 5f);
        if (transform.parent.GetComponent<Canvas>() != null)
        {
            GetComponent<RectTransform>().SetAsFirstSibling();
        }

    }

    // Update is called once per frame
    void Update()
    {
/*        delayTime -= Time.deltaTime;
        if (!delayed && delayTime < 0 && station != null)
        {
            key = station.GetComponent<ShipSpawner>().savekey + "taken bounty" + keyString;
            if (PlayerPrefs.GetInt(key + "taken", 0) == 1 && !clicked)
            {
                //Debug.Log(key);
                var HUD = FindObjectOfType<HUDmanage>();
                clicked = true;
                HUD.picture = picture;
                HUD.cost = cost;
                HUD.tied_enemy = tied_enemy;
                HUD.tied_poster = gameObject;
                HUD.new_bounty = true;

                target_ind.SetActive(true);
            }
            delayed = true;
        }*/
        if (tied_enemy == null)
        {
            
            if (on_screen || playerKilled) { HUDmanage.money += cost; HUDmanage.bountySound.Play(); }
            else PlayerPrefs.SetInt(key + "taken", 0);
            Destroy(gameObject);
        }
        else
        {
            playerKilled = tied_enemy.GetComponent<NPCmovement>().attackedByPlayer;
        }
        cost_text.GetComponent<TextMeshProUGUI>().text = cost.ToString() + " P";
        picture_obj.GetComponent<Image>().sprite = picture;
        //if (dist_obj != null && tied_enemy != null) 
        //{
            //var dist = Vector2.Distance(player.transform.position, tied_enemy.transform.position);
            //dist_obj.GetComponent<TextMeshProUGUI>().text = Mathf.Round(dist).ToString() + " miles";
                
        //}
        if (tied_poster != null && tied_poster.GetComponent<BountyPosterScript>().close)
        {
            tied_poster.GetComponent<BountyPosterScript>().clicked = false;
            tied_poster.GetComponent<BountyPosterScript>().target_ind.SetActive(false);
            tied_poster.GetComponent<BountyPosterScript>().close = false;
            Destroy(gameObject);
        }
    }



    // This method is called when the UI element is clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerEnter == target_ind || eventData.pointerEnter == gameObject || eventData.pointerEnter == cancel_ind)
        {
            var HUD = FindObjectOfType<HUDmanage>();
            if (!clicked && !on_screen)
            {
                //key = station.GetComponent<ShipSpawner>().savekey + "taken bounty" + keyString;
                PlayerPrefs.SetInt(key + "taken", 1);
                clicked = true;
                HUD.picture = picture;
                HUD.cost = cost;
                HUD.tied_enemy = tied_enemy;
                HUD.tied_poster = gameObject;
                HUD.new_bounty = true;
            }
            else if (on_screen)
            {
                PlayerPrefs.SetInt(tied_poster.GetComponent<BountyPosterScript>().key + "taken", 0);
                tied_poster.GetComponent<BountyPosterScript>().clicked = false;
                tied_poster.GetComponent<BountyPosterScript>().target_ind.SetActive(false);
                Destroy(gameObject);
            }
            else if (clicked && !on_screen)
            {
                PlayerPrefs.SetInt(key + "taken", 0);
                close = true;
            }

            PlayerPrefs.Save();
        }

        //Debug.Log(PlayerPrefs.GetInt(key + "taken"));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!on_screen && eventData.pointerEnter == gameObject)
        {
            target_ind.SetActive(true);
        }
        if (on_screen && eventData.pointerEnter == gameObject)
        {
            cancel_ind.SetActive(true);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!on_screen && !clicked)
        {
            target_ind.SetActive(false);
        }
        if (on_screen)
        {
            cancel_ind.SetActive(false);
        }
    }
}
