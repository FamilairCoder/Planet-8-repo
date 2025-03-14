using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
//using static UnityEditor.PlayerSettings;

public class NPCmovement : MonoBehaviour
{
    public bool is_pirate, is_npc, is_patrol, for_menu;
    public float lvl;
    public ParticleSystem boostParticles;
    //public GameObject basic_laser_bullet;
    [Header("Unique Variables")]
    public List<GameObject> weapons = new List<GameObject>();
    public float bounty_cost, detect_radius, attackDistance;
   // public 
    [Header("Dont have to set-------")]
    public float stay_radius;
    public bool has_bounty, inhibit, giveBounty, attackedByPlayer, retreat, found_danger, inSquad, pirateLeader;
    private bool basic_laser = true, choseFocus, held;    
    public GameObject stay_around, squadPoint, squadLeader;
    public GameObject target;
    private float turning_spd, spd, delay_time = .1f, beam_slow, saveTime, origSpd, origHealth, retreatTime, retreatChance = 1f, weaponsBroken, retreatThreshold;
    private bool did, boost = false;
    public float rand_time;
    public Vector3 dir;
    private Vector3 movedir;
    private Vector3 offset, target_point;
    public string key, squadKey;
    private ShipStats ship;
    private Rigidbody2D rb;
    public Transform playerPos;
    private PatrolID patrolID;

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
        retreatThreshold = Random.Range(.1f, .5f);
        if (retreatThreshold < .25f) retreatThreshold = 0f;

        if (is_pirate || is_patrol) StartCoroutine(FindTarget());
        if (is_patrol)
        {
            playerPos = HUDmanage.playerReference.transform;
            patrolID = GetComponent<PatrolID>();
        }
        
            
    }

    // Update is called once per frame
    void FixedUpdate()
    {                
        if (target == null)
        {
            
            if (squadLeader == null)
            {
                if (is_npc)
                {
                    rb.AddForce(transform.up * spd / 3 * Time.fixedDeltaTime, ForceMode2D.Impulse);
                }


                else if (is_patrol)
                {
                    if (patrolID.taken)
                    {
                        rb.AddForce(beam_slow * spd * Time.fixedDeltaTime * transform.up, ForceMode2D.Impulse);

                    }
                    else if (!patrolID.taken)
                    {
                        rb.AddForce(transform.up * spd / 6 * Time.fixedDeltaTime, ForceMode2D.Impulse);
                    }
                    if (GetComponent<OpenMenu>().createdMenu != null)
                    {
                        rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(0, 0), .3f);
                        dir = (playerPos.position - transform.position).normalized;
                    }
                }

                else if (is_pirate)
                {
                    if (!inSquad && !pirateLeader)
                    {
                        rb.AddForce(beam_slow * spd * Time.fixedDeltaTime * transform.up, ForceMode2D.Impulse);
                    }
                    else if (!pirateLeader)
                    {
                        rb.AddForce(beam_slow * spd * Time.fixedDeltaTime * transform.up, ForceMode2D.Impulse);
                    }
                    else
                    {
                        rb.AddForce(beam_slow * .3f * spd * Time.fixedDeltaTime * transform.up, ForceMode2D.Impulse);
                    }
                }
            }
                
                
            else rb.AddForce((squadPoint.transform.position - transform.position).normalized * spd / 3 * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
        else if (target != null)
        {
            rb.AddForce(movedir * beam_slow * spd * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }

        

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && is_pirate && PatrolManager.focusFire && !EventSystem.current.IsPointerOverGameObject() && GetComponent<Collider2D>().OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)) && !HUDmanage.on_map)
        {
            PatrolManager.holdFire = false;
            PatrolManager.focusTarget = GetComponent<ShipStats>().core;
        }
        if (is_patrol && GetComponent<PatrolID>().taken)
        {
            if (PatrolManager.focusTarget != null)
            {
                choseFocus = true;
                target = PatrolManager.focusTarget;
            }
            else if (choseFocus)
            {

                choseFocus = false;
                target = null;
            }
            if (PatrolManager.holdPosition)
            {
                held = true;
                playerPos = PatrolManager.createdHoldIndicator.transform;
            }
            else if (held)
            {
                held = false;
                playerPos = HUDmanage.playerReference.transform;
            }

        }
        if (!is_npc && key != "" && !for_menu)
        {
            delay_time -= Time.deltaTime;
            saveTime -= Time.deltaTime;

            if (delay_time < 0 && !did)
            {
                transform.position = new Vector2(FloatSaveKey("positionx", transform.position.x), FloatSaveKey("positiony", transform.position.y));
                did = true;
                //Debug.Log(ship.total_hp);
                origHealth = ship.total_hp;
            }
            if (did && saveTime < 0)
            {
                PlayerPrefs.SetFloat(key + "positionx", transform.position.x);
                PlayerPrefs.SetFloat(key + "positiony", transform.position.y);
                saveTime = 3f;
            }
        }
        
        
        
        //if (is_pirate)
        spd = ship.spd * (1 + ship.thrust_bonus);
        turning_spd = ship.turning_spd * (1 + ship.turnspd_bonus);
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, spd * beam_slow);

        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, targetAngle));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turning_spd * beam_slow * Time.deltaTime);


        //fleet movement
        if (((is_patrol && patrolID.taken) || (is_pirate && inSquad)) && playerPos != null)
        {
            //Debug.Log("using fleet motion");
            if (target == null)
            {
                
                var dist = Vector2.Distance(transform.position, playerPos.position);
                //Debug.Log(dist);
                beam_slow = Mathf.Lerp(beam_slow, .5f, .2f);
                rand_time -= Time.deltaTime;
                if (rand_time < 0)
                {
                    //beam_slow = 1f;

                    /*                if (dist > PatrolID.stayDist)
                                    {

                                    }*/



                    dir = (Random.insideUnitCircle + new Vector2(transform.position.x, transform.position.y) - new Vector2(transform.position.x, transform.position.y)).normalized;
                    rand_time = Random.Range(0f, 5f);
                }

                if ((inSquad && dist > 10) || (!inSquad && dist > PatrolID.stayDist))
                {
                    if (!boost)
                    {
                        if (patrolID != null) patrolID.boostParticles.Play();
                        else if (boostParticles != null) boostParticles.Play();
                        boost = true;
                    }
                    beam_slow = Mathf.Lerp(beam_slow, dist / 5, .2f);
                    //Debug.Log("beam slow is " + beam_slow);
                    //beam_slow = dist / 5;
                    //beam_slow = 100;
                    //Debug.Log(beam_slow);
                    dir = (playerPos.position - transform.position).normalized;
                    rand_time = Random.Range(0f, 2f);
                }
                else
                {

                    if (patrolID != null) patrolID.boostParticles.Stop();
                    else if (boostParticles != null) boostParticles.Stop();
                    boost = false;
                }
                StopLaserbeams();

            }
            else if (target != null)
            {
                boost = false;
                if (patrolID != null) patrolID.boostParticles.Stop();
                else if (boostParticles != null) boostParticles.Stop();
            }
        }
        





        if (!retreat)
        {

            ship.boosting = false;
            if (boostParticles != null) boostParticles.Stop();
           

            if (target == null && (!is_patrol || !patrolID.taken) && !inSquad)
            {
                beam_slow = 1f;

                rand_time -= Time.deltaTime;
                if (rand_time < 0)
                {
                    found_danger = false;
                    dir = (Random.insideUnitCircle + new Vector2(transform.position.x, transform.position.y) - new Vector2(transform.position.x, transform.position.y)).normalized;
                    rand_time = Random.Range(0f, 30f);
                    if (is_patrol) rand_time = Random.Range(0f, 10f);
                    target_point = new(0, 0);
                    beam_slow = 1f;
                    if ((is_patrol || is_npc) && stay_around != null && (Random.Range(0f, 1f) < .1f || (is_patrol && Random.Range(0f, 1f) < .3f)))                    {
                        target_point = new Vector2(stay_around.transform.position.x + Random.Range(-10f, 10f), stay_around.transform.position.y + Random.Range(-10f, 10f));
                        rand_time = Random.Range(30f, 60f);
                        if (is_patrol) rand_time = Random.Range(0f, 30f);
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



                if (!is_npc && stay_around != null && Vector2.Distance(transform.position, stay_around.transform.position) > stay_radius)
                {
                    dir = (stay_around.transform.position - transform.position).normalized;
                    rand_time = 10;
                }
                else if (!pirateLeader && !is_npc && stay_around != null && Vector2.Distance(transform.position, stay_around.transform.position) < stay_radius / 4)
                {
                    dir = (transform.position - stay_around.transform.position).normalized;
                    rand_time = 10;
                }


                StopLaserbeams();




            }
            else if (target != null)
            {


                rand_time -= Time.deltaTime;
                retreatTime -= Time.deltaTime;
                if (rand_time < 0)
                {
                    
                    var targetScal = target.GetComponent<Collider2D>().bounds.size.x;
                    offset = new Vector3(Random.Range(-targetScal, targetScal), Random.Range(-targetScal, targetScal));
                    rand_time = 1f;
                }


                dir = ((target.transform.position + offset) - transform.position).normalized;
                //var movePos = (transform.position - target.transform.position).normalized * attackDistance;
                //movedir = (movePos - transform.position).normalized;

                Vector3 movePos = target.transform.position + (transform.position - target.transform.position).normalized * attackDistance;
                movedir = (movePos - transform.position).normalized;



                weaponsBroken = 0;
                var count = 0;
                foreach (var w in weapons)
                {
                    if (w.activeSelf)
                    {
                        count++;
                        if (w.GetComponent<Health>() != null && w.GetComponent<Health>().hp <= 0)
                        {
                            weaponsBroken++;
                        }
                    }

                }
                if (lvl > 1 && !for_menu && !inSquad && !pirateLeader)// && attackedByPlayer)
                {
                    if (retreatTime < 0)
                    {
                        retreatChance = Random.Range(0f, 1f);
                        retreatTime = 2f;
                    }
                    if ((ship.total_hp < ship.origHp * retreatThreshold || (ship.amount_broken > 0 && retreatChance < retreatThreshold) || (weaponsBroken == count && retreatChance < .5f)))
                    {
                        retreat = true;
                        rand_time = 10f;

                        movedir = (transform.position - target.transform.position).normalized;
                        dir = (transform.position - target.transform.position).normalized;


                        if (lvl > 1)
                        {
                            boost = true;
                        }
                    }
                }



                if (Vector2.Distance(target.transform.position, transform.position) < detect_radius * .8f)
                {
                    FireWeapons();
                }
                else
                {
                    StopLaserbeams();
                }




/*                if (is_patrol)
                {
                    if (target != null) Debug.Log("target is not null");
                    if (target.GetComponent<Health>() != null) Debug.Log("target has health");
                    if (Vector2.Distance(target.transform.position, transform.position) > detect_radius) Debug.Log("target is greater than detect radius");
                    if (target.GetComponent<Health>().hp <= 0) Debug.Log("target has less than or equal to 0 health");
                }*/
                if ((is_patrol && GetComponent<PatrolID>().taken && PatrolManager.holdFire) || (target != null && ((target.GetComponent<Health>() != null && (Vector2.Distance(target.transform.position, transform.position) > detect_radius || target.GetComponent<Health>().hp <= 0)) || Vector2.Distance(target.transform.position, transform.position) > detect_radius)))
                {
                    //if (is_patrol) Debug.Log("AAAAAAAAA");
                    target = null;
                    rand_time = 0;
                    //Debug.Log("AAAAAAA");
                    StopLaserbeams();
                    StartCoroutine(FindTarget());
                }
            }
        }
        else
        {
            if (boost)
            {
                boostParticles.Play();
                ship.boosting = true;
                boost = false;
            }

            if (Vector2.Distance(target.transform.position, transform.position) < 150f) PlayerBash.bash = true;
            else PlayerBash.bash = false;


            rand_time -= Time.deltaTime;
            if (rand_time < 0)
            {
                boostParticles.Stop();
                PlayerBash.bash = false;
                retreat = false;
                target = null;
                StartCoroutine(FindTarget());
            }

        }
    }


    void FindTargets(string tag, float dist)
    {
        //Debug.Log("Finding tag: " + tag);
        var hit = Physics2D.OverlapCircleAll(transform.position, dist);
        var possibleTargets = new List<GameObject>();
        foreach (var b in hit)
        {

            var a = b.gameObject;
            if (a.CompareTag(tag) && a.GetComponent<Collider2D>() != null && a.GetComponent<Health>() != null && a != gameObject && !a.transform.IsChildOf(transform))
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

/*    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            found_danger = false;
            Debug.Log("not found danger");
        }
        
    }*/

    private void FireWeapons()
    {
        foreach (GameObject w in weapons)
        {
            var weapon = w.GetComponent<NPCweapon>();
            if (!weapon.laser_beam)
            {
                weapon.Attack();
                weapon.target_tag = target.tag;
                weapon.dmg_bonus = GetComponent<ShipStats>().dmg_bonus;
                weapon.firerate_bonus = GetComponent<ShipStats>().firerate_bonus;
            }
            else
            {
                weapon.target_tag = target.tag;
                weapon.is_firing = true;
                beam_slow = .75f;
                weapon.dist = detect_radius;
            }
        }
    }
    private void StopLaserbeams()
    {
        foreach (GameObject w in weapons)
        {
            if (w.GetComponent<NPCweapon>().laser_beam)
            {
                w.GetComponent<NPCweapon>().is_firing = false;
                //beam_slow = 1f;
            }
        }
    }

    private IEnumerator FindTarget()
    {
        yield return new WaitForSeconds(.2f);
        
        while (true)
        {
            if (target == null && (!is_patrol || (!GetComponent<PatrolID>().taken || !PatrolManager.holdFire)))
            {

                if (is_pirate)
                {
                    
                    FindTargets("patrol", detect_radius);
                    if (target == null)
                    {
                        FindTargets("player_station", detect_radius);
                    }
                    if (target == null)
                    {
                        FindTargets("player", detect_radius);
                    }
                }
                else if (is_patrol)
                {
                    FindTargets("enemy", detect_radius);
                    //Debug.Log("AAAAAA");
                }
            }
            yield return new WaitForSeconds(Random.Range(0f, 1f));
        }

    }
}
