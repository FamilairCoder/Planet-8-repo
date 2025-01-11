using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class NPCmovement : MonoBehaviour
{
    public bool is_pirate, is_npc, for_menu;
    public float lvl;
    //public GameObject basic_laser_bullet;
    [Header("Unique Variables")]
    public List<GameObject> weapons = new List<GameObject>();
    public float bounty_cost, detect_radius;
    [Header("Dont have to set-------")]
    public float stay_radius;
    public bool has_bounty, inhibit, giveBounty, attackedByPlayer;
    private bool basic_laser = true;    
    public GameObject stay_around, squadPoint, squadLeader;
    public GameObject target;
    private float turning_spd, spd, delay_time = .1f, beam_slow, saveTime, origSpd;
    private bool did, found_danger;
    public float rand_time;
    public Vector3 dir;
    private Vector3 offset, target_point;
    public string key;
    private ShipStats ship;
    private Rigidbody2D rb;
   
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<ParticleSystem>() != null && inhibit) { GetComponent<ParticleSystem>().Play(); Debug.Log(inhibit); }
        //GetComponent<ParticleSystem>().Play();
        ship = GetComponent<ShipStats>();
        rb = GetComponent<Rigidbody2D>();
        origSpd = ship.spd * (1 + ship.thrust_bonus);
        /*        var weapon_chance = Random.Range(0f, 1f);
                if (weapon_chance < 1)
                {
                    basic_laser = true;
                    atk_spd = Random.Range(.2f, .4f);
                }*/
    }

    // Update is called once per frame
    void FixedUpdate()
    {                
        if (target == null)
        {
            
            if (squadLeader == null) rb.AddForce(transform.up * spd / 3 * Time.fixedDeltaTime, ForceMode2D.Impulse);
            else rb.AddForce((squadPoint.transform.position - transform.position).normalized * spd / 3 * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
        else if (target != null)
        {
            rb.AddForce(transform.up * spd * beam_slow * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }

        

    }

    private void Update()
    {
        if (!is_npc && key != "" && !for_menu)
        {
            delay_time -= Time.deltaTime;
            saveTime -= Time.deltaTime;

            if (delay_time < 0 && !did)
            {
                transform.position = new Vector2(FloatSaveKey("positionx", transform.position.x), FloatSaveKey("positiony", transform.position.y));
                did = true;
            }
            if (did && saveTime < 0)
            {
                PlayerPrefs.SetFloat(key + "positionx", transform.position.x);
                PlayerPrefs.SetFloat(key + "positiony", transform.position.y);
                saveTime = 3f;
            }
        }
        
        
        

        spd = ship.spd * (1 + ship.thrust_bonus);
        turning_spd = ship.turning_spd * (1 + ship.turnspd_bonus);
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, spd * beam_slow);

        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, targetAngle));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turning_spd * beam_slow * Time.deltaTime);


        if (target == null)
        {
            beam_slow = 1f;

            rand_time -= Time.deltaTime;
            if (rand_time < 0)
            {
                dir = (Random.insideUnitCircle + new Vector2(transform.position.x, transform.position.y) - new Vector2(transform.position.x, transform.position.y)).normalized;
                rand_time = Random.Range(0f, 30f);
                target_point = new(0, 0);
                beam_slow = 1f;
                if (is_npc && stay_around != null && Random.Range(0f, 1f) < .1f)
                {
                    target_point = new Vector2(stay_around.transform.position.x + Random.Range(-10f, 10f), stay_around.transform.position.y + Random.Range(-10f, 10f));
                    rand_time = Random.Range(30f, 60f);
                }
            }

            if (target_point.x != 0 && !found_danger)
            {
                dir = (target_point - new Vector3(transform.position.x, transform.position.y)).normalized;
                if (Vector2.Distance(transform.position, target_point) < 10)
                {
                    dir = (Random.insideUnitCircle + new Vector2(transform.position.x, transform.position.y) - new Vector2(transform.position.x, transform.position.y)).normalized;
                    beam_slow = .1f;
                }
            }

            if (squadPoint != null)
            {
                var dist = Vector2.Distance(squadPoint.transform.position, transform.position);
                dir = squadLeader.transform.up;

                spd = Mathf.Lerp(0, origSpd, dist / 2);
            }


            if (is_pirate)
            {
                FindTarget("player_station", detect_radius);
                if (target == null)
                {
                    FindTarget("player", detect_radius);
                }
            }

            if (!is_npc && stay_around != null && Vector2.Distance(transform.position, stay_around.transform.position) > stay_radius)
            {
                dir = (stay_around.transform.position - transform.position).normalized;
                rand_time = 10;
            }
            else if (!is_npc && stay_around != null && Vector2.Distance(transform.position, stay_around.transform.position) < stay_radius / 4)
            {
                dir = (transform.position - stay_around.transform.position).normalized;
                rand_time = 10;
            }

        }
        else if (target != null)
        {


            rand_time -= Time.deltaTime;
            if (rand_time < 0)
            {
                var targetScal = target.GetComponent<Collider2D>().bounds.size.x;
                offset = new Vector3(Random.Range(-targetScal, targetScal), Random.Range(-targetScal, targetScal));
                rand_time = 1f;
            }


            dir = ((target.transform.position + offset) - transform.position).normalized;

            if (Vector2.Distance(target.transform.position, transform.position) < detect_radius * .8f)
            {

                foreach (GameObject w in weapons)
                {
                    if (!w.GetComponent<NPCweapon>().laser_beam)
                    {
                        w.GetComponent<NPCweapon>().Attack();
                        w.GetComponent<NPCweapon>().target_tag = target.tag;
                        w.GetComponent<NPCweapon>().dmg_bonus = GetComponent<ShipStats>().dmg_bonus;
                        w.GetComponent<NPCweapon>().firerate_bonus = GetComponent<ShipStats>().firerate_bonus;
                    }
                    else
                    {
                        w.GetComponent<NPCweapon>().target_tag = target.tag;
                        w.GetComponent<NPCweapon>().is_firing = true;
                        beam_slow = .5f;
                        w.GetComponent<NPCweapon>().dist = detect_radius;
                    }
                }



            }
            else
            {
                foreach (GameObject w in weapons)
                {
                    if (w.GetComponent<NPCweapon>().laser_beam)
                    {
                        w.GetComponent<NPCweapon>().is_firing = false;
                        beam_slow = 1f;
                    }
                }
            }




            if (target != null && target.GetComponent<Health>() != null && (Vector2.Distance(target.transform.position, transform.position) > detect_radius || target.GetComponent<Health>().hp <= 0))
            {
                target = null;
                rand_time = 0;
            }
        }
    }


    void FindTarget(string tag, float dist)
    {
        var hit = Physics2D.OverlapCircleAll(transform.position, dist);
        var possibleTargets = new List<GameObject>();
        foreach (var b in hit)
        {

            var a = b.gameObject;
            if (a.CompareTag(tag) && a != gameObject && !a.transform.IsChildOf(transform))
            {

                possibleTargets.Add(a);
            }
            
        }
        if (possibleTargets.Count > 0)
        {
            var index = Random.Range(0, possibleTargets.Count);
            target = possibleTargets[index];
            var tpos = target.transform.position;
            dir = (Random.insideUnitCircle + new Vector2(tpos.x, tpos.y) - new Vector2(transform.position.x, transform.position.y)).normalized;
            rand_time = 2;
        }
    }

    float FloatSaveKey(string name, float default_value)
    {
        var f = PlayerPrefs.GetFloat(key + name, default_value);
        PlayerPrefs.SetFloat(key + name, f);
        return f;

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        
        if (collision.CompareTag("enemy") && is_npc)
        {
            found_danger = true;
            dir = -(collision.transform.position - transform.position).normalized;
            rand_time = 10f;
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            found_danger = false;
        }
        
    }

}
