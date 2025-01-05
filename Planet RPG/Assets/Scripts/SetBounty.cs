using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class SetBounty : MonoBehaviour
{
    public GameObject station, menu, bounty_poster;
    public float lvl;
    public List<GameObject> bounties = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        station = transform.parent.GetComponent<MenuScript>().station;
    }

    // Update is called once per frame
    void Update()
    {
        var hit = Physics2D.OverlapCircleAll(station.transform.position, station.GetComponent<ShipSpawner>().max_dist);
        var keynumb = 0f;
        foreach (var a in hit)
        {
 
            
            if (a.CompareTag("enemy") && a.GetComponent<NPCmovement>() != null && !a.GetComponent<NPCmovement>().has_bounty && bounties.Count < 4 && a.GetComponent<NPCmovement>().lvl == lvl)
            {

                var b = Instantiate(bounty_poster, GetComponent<RectTransform>().transform.position, Quaternion.identity, menu.transform);
                a.GetComponent<NPCmovement>().has_bounty = true;
                b.GetComponent<BountyPosterScript>().cost = a.GetComponent<NPCmovement>().bounty_cost + a.GetComponent<ShipStats>().bounty_bonus;
                b.GetComponent<BountyPosterScript>().picture = a.GetComponent<SpriteRenderer>().sprite;
                b.GetComponent<BountyPosterScript>().tied_enemy = a.gameObject;
                b.GetComponent<BountyPosterScript>().station = station;
                b.GetComponent<BountyPosterScript>().delayTime = keynumb / 2;
                b.GetComponent<BountyPosterScript>().keyString = a.GetComponent<NPCmovement>().key;
                Destroy(b.GetComponent<BountyPosterScript>().dist_obj);
                Destroy(b.GetComponent<BountyPosterScript>().track_obj);
                bounties.Add(b);
                if (GetComponent<MenuButton>() != null)
                {
                    GetComponent<MenuButton>().show.Add(b);
                }
                else if (GetComponent<SubMenuButton>() != null)
                {
                    GetComponent<SubMenuButton>().show.Add(b);
                }
                //Camera.main.GetComponent<HUDmanage>().active_bounties.Add(b);
                keynumb += Random.Range(1f, 2f);
            }
            else if (bounties.Count >= 4) break;
        }
        var pos = GetComponent<RectTransform>().position;
        if (GetComponent<SubMenuButton>() != null)
        {
            pos = GetComponent<SubMenuButton>().mainButton.GetComponent<RectTransform>().position;
        }
        

        for (int i = 0; i < bounties.Count; i++)
        {
            if (bounties[i] == null) 
            { 
                bounties.Remove(bounties[i]);
                //if (GetComponent<MenuButton>() != null) GetComponent<MenuButton>().show.Remove(bounties[i]); 
                break; 
            
            }
            bounties[i].GetComponent<RectTransform>().position = new Vector3(pos.x + 50, pos.y - i * 250 - 250, 0f);
            if (i > 1)
            {
                bounties[i].GetComponent<RectTransform>().position += new Vector3(220, 500);
            }
        }

    }
}
