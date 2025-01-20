using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCweapon : MonoBehaviour
{
    public GameObject atk_point, bullet, beam_explosion;
    public float atk_spd, atk_spread, atk_dmg_add, beam_dmg;
    public bool laser_beam;
    [Header("Dont have to set---------")]
    public bool is_firing;
    public float dmg_bonus, firerate_bonus, dist;
    private float atk_time, dmg_time = .25f;
    public string target_tag;
    [Header("For second atk_point---------")]
    public GameObject atk_point2;
    // Start is called before the first frame update
    void Start()
    {
        
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
                if (c.collider.CompareTag(target_tag) && c.collider.gameObject.GetComponent<Health>() != null && c.collider.gameObject.GetComponent<Health>().hp > 0)
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
        atk_time -= Time.deltaTime;
        if (atk_time <= 0 && GetComponent<Health>().hp > 0)
        {
            var rot = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + Random.Range(-atk_spread, atk_spread));
            var b = Instantiate(bullet, atk_point.transform.position, rot);
            b.GetComponent<Bullet>().target_tag = target_tag;
            b.GetComponent<Bullet>().came_from = gameObject;
            b.GetComponent<Bullet>().dmg *= (1 + dmg_bonus);
            b.GetComponent<Bullet>().dmg += atk_dmg_add;

            if (atk_point2 != null)
            {
                rot = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + Random.Range(-atk_spread, atk_spread));
                b = Instantiate(bullet, atk_point.transform.position, rot);
                b.GetComponent<Bullet>().target_tag = target_tag;
                b.GetComponent<Bullet>().came_from = gameObject;
                b.GetComponent<Bullet>().dmg *= (1 + dmg_bonus);
                b.GetComponent<Bullet>().dmg += atk_dmg_add;
            }
            
            atk_time = atk_spd * (1 + firerate_bonus);
        }
    }
}
