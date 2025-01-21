using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float hp, orig_hp;
    public GameObject death_explosion;
    public Sprite ruin;
    public Sprite orig_sprite;
    public bool gets_ruined;
    public GameObject trail, linked_diagnosis;
    private float saveTime;
    private SpriteRenderer spr;
    [Header("For test dummy stuf---------------------------")]
    public bool reportDmg;
    private float dmgTime = 1, startHP;
    // Start is called before the first frame update
    void Start()
    {
        if (!reportDmg && transform.parent.GetComponent<ShipStats>() != null && !transform.parent.GetComponent<ShipStats>().ignore_key)
        {
            if (transform.parent.GetComponent<ShipStats>().npc)
            {

                hp = PlayerPrefs.GetFloat(transform.parent.GetComponent<NPCmovement>().key + transform.GetSiblingIndex() + "hp", hp);
                orig_hp = PlayerPrefs.GetFloat(transform.parent.GetComponent<NPCmovement>().key + transform.GetSiblingIndex() + "orig_hp", hp);

            }
            else
            {
                hp = PlayerPrefs.GetFloat("player" + transform.GetSiblingIndex() + "hp", hp);
                orig_hp = PlayerPrefs.GetFloat("player" + transform.GetSiblingIndex() + "orig_hp", hp);

            }
            StartCoroutine(saveRoutine());
            StartCoroutine(healRoutine());
        }        
        else 
        {
            orig_hp = hp;
        }
        spr = GetComponent<SpriteRenderer>();
        orig_sprite = GetComponent<SpriteRenderer>().sprite;
        startHP = hp;
    }

    // Update is called once per frame
    void Update()
    {
        if (reportDmg)
        {
            dmgTime -= Time.deltaTime;
            if (dmgTime < 0)
            {
                Debug.Log(Mathf.Round(startHP - hp) + " every second");
                hp = startHP;
                dmgTime = 1;
            }
        }
        else
        {
            if (linked_diagnosis != null)
            {
                linked_diagnosis.GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, hp / orig_hp);

            }



            if (hp <= 0)
            {

                if (trail != null)
                {
                    trail.SetActive(false);
                }
                if (gets_ruined)
                {
                    if (ruin != null) spr.sprite = ruin;
                    else
                    {
                        spr.color = new Color(.5f, .5f, .5f);
                    }
                    GetComponent<Collider2D>().enabled = false;
                    if (GetComponent<Animator>() != null) GetComponent<Animator>().enabled = false;

                }


            }
            else
            {
                if (trail != null)
                {
                    trail.SetActive(true);
                }
                GetComponent<Collider2D>().enabled = true;
                if (GetComponent<Animator>() != null)
                    GetComponent<Animator>().enabled = true;

                spr.sprite = orig_sprite;
                spr.color = new Color(1f, 1f, 1f);


            }
        }
        
    }

    private IEnumerator saveRoutine()
    {

        while (true)
        {
            if (transform.parent.GetComponent<ShipStats>() != null && !transform.parent.GetComponent<ShipStats>().ignore_key)
            {
                if (transform.parent.GetComponent<ShipStats>().npc)
                {

                    PlayerPrefs.SetFloat(transform.parent.GetComponent<NPCmovement>().key + transform.GetSiblingIndex() + "hp", hp);
                    PlayerPrefs.SetFloat(transform.parent.GetComponent<NPCmovement>().key + transform.GetSiblingIndex() + "orig_hp", orig_hp);

                }
                else
                {
                    PlayerPrefs.SetFloat("player" + transform.GetSiblingIndex() + "hp", hp);
                    PlayerPrefs.SetFloat("player" + transform.GetSiblingIndex() + "orig_hp", orig_hp);
                }
            }
            yield return new WaitForSeconds(Random.Range(.5f, 1.5f));
        }
        
    }

    private IEnumerator healRoutine()
    {
        if (GetComponentInParent<NPCmovement>() == null || !GetComponentInParent<NPCmovement>().is_pirate)
        {
            yield break;
        }
        while (true)
        {

            if (hp < orig_hp && GetComponentInParent<NPCmovement>().target == null)
            {
                hp += Random.Range(0f, 1f);
                
            }
            yield return new WaitForSeconds(1);

        }

    }
}
