using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCweapon : MonoBehaviour
{
    public GameObject atk_point, bullet, beam_explosion;
    public float atk_spd, atk_spread, atk_dmg_add, beam_dmg;
    public bool laser_beam, randomWeapon;
    [Header("Dont have to set---------")]
    public GameObject laserbullet, rod;
    public bool is_firing, laserBullet, laserRod;
    public float dmg_bonus, firerate_bonus, dist;
    private float atk_time, dmg_time = .25f;
    public string target_tag;
    [Header("For second atk_point---------")]
    public GameObject atk_point2;
    // Start is called before the first frame update
    void Start()
    {
        if (randomWeapon)
        {
            var npcM = GetComponentInParent<NPCmovement>();
            var chance = Random.Range(0f, 1f);
            if (chance < .3f)
            {
                laser_beam = true;
                npcM.attackDistance = 10;
            }
            else if (chance < .6f)
            {
                laserBullet = true;
                bullet = laserbullet;

                atk_spd = .2f;
                atk_spread = 5;
                npcM.attackDistance = 3;
            }
            else
            {
                laserRod = true;
                bullet = rod;

                atk_spd = .2f;
                atk_spread = 1;
                npcM.attackDistance = 5;
            }




            if (GetComponentInParent<PatrolID>() != null)
            {
                
                var id = GetComponentInParent<PatrolID>().id.ToString();
                if (PlayerPrefs.GetFloat("alive" + id, 1) == 1)
                {
                    chance = PlayerPrefs.GetFloat(id + "weaponChance" + transform.GetSiblingIndex(), Random.Range(0f, 1f));
                }
                else
                {
                    chance = Random.Range(0f, 1f);
                }



                if (chance < .3f)
                {
                    laser_beam = true;
                }
                else if (chance < .6f)
                {
                    laserBullet = true;
                    bullet = laserbullet;

                    atk_spd = .2f;
                    atk_spread = 5;
                }
                else
                {
                    laserRod = true;
                    bullet = rod;

                    atk_spd = .2f;
                    atk_spread = 1;
                }



                PlayerPrefs.SetFloat(id + "weaponChance" + transform.GetSiblingIndex(), chance);
                PlayerPrefs.Save();
            }




        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Health>().hp <= 0) { is_firing = false; }
        if (is_firing)
        {
            
            var lr = GetComponent<LineRenderer>();
            lr.enabled = true;
            var cast = Physics2D.RaycastAll(atk_point.transform.position, transform.up, dist);
            
            Vector3 hit = new(0, 0, 0);
            GameObject obj = null;
            foreach (var c in cast)
            {
                //Debug.Log(c);
                if (c.collider.CompareTag(target_tag) && c.collider.gameObject != gameObject && !c.transform.IsChildOf(transform) && c.collider.gameObject.GetComponent<Health>() != null && c.collider.gameObject.GetComponent<Health>().hp > 0)
                {
                    hit = c.point;
                    obj = c.collider.gameObject;
                    break;
                }

            }


            lr.SetPosition(0, atk_point.transform.position);
            if (hit != new Vector3(0, 0, 0)) 
            { 
                lr.SetPosition(1, hit);

                dmg_time -= Time.deltaTime;
                if (dmg_time <= 0)
                {
                    Instantiate(beam_explosion, hit, Quaternion.identity, obj.transform);
                    obj.GetComponent<Health>().hp -= beam_dmg;
                    dmg_time = .25f;
                }
            }
            else
            {
                lr.SetPosition(1, atk_point.transform.position + atk_point.transform.up * dist);
            }
        }
        else if (GetComponent<LineRenderer>() != null)
        {
            var lr = GetComponent<LineRenderer>();
            lr.enabled = false;

        }
    }

    public void Attack()
    {
        //if (GetComponentInParent<NPCmovement>().is_patrol) Debug.Log("attacking");
        atk_time -= Time.deltaTime;
        if (atk_time <= 0 && GetComponent<Health>().hp > 0)
        {
            var rot = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + Random.Range(-atk_spread, atk_spread));
            var b = Instantiate(bullet, atk_point.transform.position, rot);
            b.GetComponent<Bullet>().target_tag = target_tag;
            b.GetComponent<Bullet>().came_from = gameObject;
            b.GetComponent<Bullet>().dmg *= (1 + dmg_bonus);
            b.GetComponent<Bullet>().dmg += atk_dmg_add;
            if (GetComponentInParent<PatrolID>() != null && GetComponentInParent<PatrolID>().taken) b.GetComponent<Bullet>().patrolMade = true;
            if (atk_point2 != null)
            {
                rot = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + Random.Range(-atk_spread, atk_spread));
                b = Instantiate(bullet, atk_point.transform.position, rot);
                b.GetComponent<Bullet>().target_tag = target_tag;
                b.GetComponent<Bullet>().came_from = gameObject;
                b.GetComponent<Bullet>().dmg *= (1 + dmg_bonus);
                b.GetComponent<Bullet>().dmg += atk_dmg_add;
                if (GetComponentInParent<PatrolID>() != null && GetComponentInParent<PatrolID>().taken) b.GetComponent<Bullet>().patrolMade = true;
            }
            
            atk_time = atk_spd * (1 + firerate_bonus);
        }
    }
}
