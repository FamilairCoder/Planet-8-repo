using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
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
    [Header("For Stations and Outposts----------------")]
    public bool isStation;
    public GameObject ruinObj, explosion;
    public SpriteRenderer cracks;
    public ParticleSystem deathParticles;
    public float bounty;
    private bool started, isPatrol, isPirate;
    public bool largeDeath;
    public GameObject ripple;
    //private GameObject linkedCore;
    [Header("For test dummy stuf---------------------------")]
    public bool reportDmg;
    private float dmgTime = 1, startHP;
    // Start is called before the first frame update
    void Start()
    {
        //linkedCore = transform.parent.GetComponent<ShipStats>().core;
        if (GetComponentInParent<PatrolID>() != null) isPatrol = true;
        else if (GetComponentInParent<NPCmovement>() != null && GetComponentInParent<NPCmovement>().is_pirate) isPirate = true;
        spr = GetComponent<SpriteRenderer>();
        if (!isStation && !reportDmg && transform.parent.GetComponent<ShipStats>() != null && !transform.parent.GetComponent<ShipStats>().ignore_key)
        {
            if (transform.parent.GetComponent<ShipStats>().npc)
            {
                if (isPatrol)// && PlayerPrefs.GetFloat(GetComponentInParent<PatrolID>().id + "alive", 1) == 1)
                {
                    hp = PlayerPrefs.GetFloat(GetComponentInParent<PatrolID>().id + transform.GetSiblingIndex() + "hp", hp);
                    orig_hp = PlayerPrefs.GetFloat(GetComponentInParent<PatrolID>().id + transform.GetSiblingIndex() + "orig_hp", hp);
                }
                else if (isPirate)// && PlayerPrefs.GetFloat(transform.parent.GetComponent<NPCmovement>().key + "alive", 1) == 1)
                {
                    hp = PlayerPrefs.GetFloat(transform.parent.GetComponent<NPCmovement>().key + transform.GetSiblingIndex() + "hp", hp);
                    orig_hp = PlayerPrefs.GetFloat(transform.parent.GetComponent<NPCmovement>().key + transform.GetSiblingIndex() + "orig_hp", hp);
                }
/*                else
                {
                    if (isPatrol) PlayerPrefs.SetFloat(GetComponentInParent<PatrolID>().id + "alive", 1);
                    else if (isPirate) PlayerPrefs.GetFloat(transform.parent.GetComponent<NPCmovement>().key + "alive", 1);
                }*/


            }
            else
            {
                hp = PlayerPrefs.GetFloat("player" + transform.GetSiblingIndex() + "hp", hp);
                orig_hp = PlayerPrefs.GetFloat("player" + transform.GetSiblingIndex() + "orig_hp", hp);

            }
            StartCoroutine(saveRoutine());
            StartCoroutine(healRoutine());
        }        
        else if (!isStation)
        {
            orig_hp = hp;
        }
        else if (isStation)
        {
            hp = PlayerPrefs.GetFloat(gameObject.name + "hp", hp);
            orig_hp = PlayerPrefs.GetFloat(gameObject.name + "orig_hp", hp);
            if (hp <= 0)
            {
                var a = Instantiate(ruinObj, transform.position, transform.rotation);
                a.transform.localScale = transform.localScale;
                Destroy(a.GetComponent<DespawnTimer>());
                a.GetComponent<SpriteRenderer>().sprite = ruin;

                Destroy(gameObject);
            }
        }

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
                if (!isStation)
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
                else if (!started)
                {
                    StartCoroutine(DeathSequence());
                    started = true;
 
                    


                }


            }
            else
            {
                if (trail != null)
                {
                    trail.SetActive(true);
                }
                if (GetComponent<Collider2D>() != null) GetComponent<Collider2D>().enabled = true;
                if (GetComponent<Animator>() != null)
                    GetComponent<Animator>().enabled = true;

                if (orig_sprite != null && spr.sprite != null) spr.sprite = orig_sprite;
                if (spr != null) spr.color = new Color(1f, 1f, 1f);

                if (isStation)
                {
                    
                    cracks.color = Color.Lerp(new Color(1, 1, 1, .5f), new Color(0, 0, 0, 0), hp / orig_hp);
                }

            }
        }
        
    }


    private IEnumerator DeathSequence()
    {
        var timeleft = 0;
        float size = GetComponent<Collider2D>().bounds.size.x * 1.3f;
        deathParticles.Play();
        while (true)
        {
            if (!largeDeath && timeleft < 30)
            {

                Instantiate(explosion, transform.position + new Vector3(Random.Range(-size, size), Random.Range(-size, size)), Quaternion.identity).transform.localScale = new Vector3(2, 2);

                timeleft++;
                yield return new WaitForSeconds(.1f);
            }
            else if (largeDeath && timeleft <= 60)
            {
                Instantiate(explosion, transform.position + new Vector3(Random.Range(-size, size), Random.Range(-size, size)), Quaternion.identity).transform.localScale = new Vector3(3, 3);
                if (timeleft == 0 || timeleft == 20 || timeleft == 40 || timeleft == 60)
                {
                    Instantiate(explosion, transform.position, Quaternion.identity).transform.localScale = new Vector3(5, 5);
                    Instantiate(ripple, transform.position, Quaternion.identity);
                }
                timeleft++;
                yield return new WaitForSeconds(.1f);
            }
            else
            {
                var a = Instantiate(ruinObj, transform.position, transform.rotation);
                a.transform.localScale = transform.localScale;
                Destroy(a.GetComponent<DespawnTimer>());
                a.GetComponent<SpriteRenderer>().sprite = ruin;

                PlayerPrefs.SetFloat(gameObject.name + "alive", 0);

                if (bounty > 0)
                {
                    HUDmanage.money += bounty;

                }
                if (!largeDeath) Instantiate(explosion, transform.position, Quaternion.identity).transform.localScale = new Vector3(5, 5);
                else Instantiate(explosion, transform.position, Quaternion.identity).transform.localScale = new Vector3(7, 7);
                PlayerPrefs.Save();
                Destroy(gameObject);


                yield return null;
            }
        }
    }


    private IEnumerator saveRoutine()
    {

        yield return new WaitForSeconds(.1f);
        spr = GetComponent<SpriteRenderer>();
        orig_sprite = GetComponent<SpriteRenderer>().sprite;
        while (true)
        {

            if (!isStation && transform.parent.GetComponent<ShipStats>() != null && !transform.parent.GetComponent<ShipStats>().ignore_key)
            {
                if (transform.parent.GetComponent<ShipStats>().npc)
                {

                    if (isPatrol)
                    {
                        PlayerPrefs.SetFloat(GetComponentInParent<PatrolID>().id + transform.GetSiblingIndex() + "hp", hp);
                        PlayerPrefs.SetFloat(GetComponentInParent<PatrolID>().id + transform.GetSiblingIndex() + "orig_hp", orig_hp);
                    }
                    else if (isPirate)
                    {
                        PlayerPrefs.SetFloat(transform.parent.GetComponent<NPCmovement>().key + transform.GetSiblingIndex() + "hp", hp);
                        PlayerPrefs.SetFloat(transform.parent.GetComponent<NPCmovement>().key + transform.GetSiblingIndex() + "orig_hp", orig_hp);
                    }

                }
                else
                {
                    PlayerPrefs.SetFloat("player" + transform.GetSiblingIndex() + "hp", hp);
                    PlayerPrefs.SetFloat("player" + transform.GetSiblingIndex() + "orig_hp", orig_hp);
                }
            }
            else if (isStation)
            {
                PlayerPrefs.SetFloat(gameObject.name + "hp", hp);
                PlayerPrefs.SetFloat(gameObject.name + "orig_hp", orig_hp);
            }
            yield return new WaitForSeconds(Random.Range(4f, 6f));
        }
        
    }

    private IEnumerator healRoutine()
    {
        if (GetComponentInParent<NPCmovement>() == null || GetComponentInParent<NPCmovement>().is_npc)
        {
            yield break;
        }
        while (true)
        {

            if (hp < orig_hp && GetComponentInParent<NPCmovement>().target == null)
            {
                hp += Random.Range(0f, 1f);
                Instantiate(HUDmanage.playerReference.GetComponent<PlayerMovement>().healingParticle, transform.position, Quaternion.identity).GetComponent<ObjectDisappear>().offsetPos = transform;
            }
            yield return new WaitForSeconds(Random.Range(0f, 3f));

        }

    }

    private void OnEnable()
    {
        StartCoroutine(saveRoutine());
        StartCoroutine(healRoutine());
    }

}
