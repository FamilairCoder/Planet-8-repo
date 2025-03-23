using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class StationWeapon : MonoBehaviour
{
    [Header("Set")]
    public bool isPirate;
    public List<GameObject> atk_points = new List<GameObject>();
    //public GameObject atk_point;
    public GameObject bullet;
    public float turning_spd, atk_spd, atk_spread, detection_radius, additionalSizeMult;
    public bool laser;
    public float laser_range, laser_dmg;
    public GameObject laser_explosion;
    public bool missile;
    public float turnSpdMultiplier, sizeMultiplier;
    [Header("Dont need to set---------")]
    public GameObject target;
    public float atk_time;
    public string target_tag;
    private Vector2 dir;
    private float dirtime, dmg_time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (GetComponent<Health>().hp > 0 && (transform.parent.GetComponent<Health>() == null || transform.parent.GetComponent<Health>().hp > 0))
        {
            float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, targetAngle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turning_spd * Time.deltaTime);
        }
        //Debug.Log(target_tag);

        if (target != null)
        {
            dir = (target.transform.position - transform.position).normalized;

            if (!laser)
            {
                atk_time -= Time.deltaTime;
                if (atk_time <= 0 && GetComponent<Health>().hp > 0 && (transform.parent.GetComponent<Health>() == null || transform.parent.GetComponent<Health>().hp > 0))
                {
                    if (!missile)
                    {
                        foreach (var ap in atk_points)
                        {
                            var rot = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + Random.Range(-atk_spread, atk_spread));
                            var b = Instantiate(bullet, ap.transform.position, rot);
                            b.transform.localScale *= (1 + additionalSizeMult);
                            b.GetComponent<Bullet>().target_tag = target_tag;
                            b.GetComponent<Bullet>().came_from = gameObject;
                        }
                    }
                    else
                    {
                        foreach (var ap in atk_points)
                        {
                            var b = Instantiate(bullet, ap.transform.position, ap.transform.rotation);
                            b.transform.localScale *= sizeMultiplier;
                            b.GetComponent<Bullet>().turn_spd *= turnSpdMultiplier;
                            b.GetComponent<Bullet>().target_tag = target_tag;
                            b.GetComponent<Bullet>().came_from = gameObject;
                            b.GetComponent<Bullet>().target = target;
                        }
                    }



                    atk_time = atk_spd;
                }
            }



            else 
            {

                var lr = GetComponent<LineRenderer>();
                lr.enabled = true;
                var cast = Physics2D.RaycastAll(atk_points[0].transform.position, transform.up, laser_range);
                Vector3 hit = new(0, 0, 0);
                GameObject obj = null;
                foreach (var c in cast)
                {
                    if (c.collider.gameObject.GetComponent<Health>() != null && c.collider.gameObject.GetComponent<Health>().hp > 0)
                    {
                        if (c.collider.CompareTag(target_tag) || (isPirate && HUDmanage.pirateTags.Contains(c.collider.tag)))
                        {
                            hit = c.point;
                            obj = c.collider.gameObject;
                            break;
                        }

                    }

                }


                lr.SetPosition(0, atk_points[0].transform.position);
                if (hit != new Vector3(0, 0, 0))
                {
                    lr.SetPosition(1, hit);

                    dmg_time -= Time.deltaTime;
                    if (dmg_time <= 0)
                    {
                        Instantiate(laser_explosion, hit, Quaternion.identity, obj.transform);
                        obj.GetComponent<Health>().hp -= laser_dmg;
                        dmg_time = .25f;
                    }
                }
                else
                {
                    lr.SetPosition(1, atk_points[0].transform.position + atk_points[0].transform.up * laser_range);
                }
            }
            if (Vector2.Distance(transform.position, target.transform.position) > detection_radius) target = null;

        }
        
        if (target == null)
        {
            if (!isPirate)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detection_radius, LayerMask.GetMask("pirates"));
                foreach (Collider2D a in colliders)
                {
                    target = a.gameObject;
                    break;
                }
            }
            else
            {
                Collider2D[] player = Physics2D.OverlapCircleAll(transform.position, detection_radius, LayerMask.GetMask("playerparts"));
                Collider2D[] patrols = Physics2D.OverlapCircleAll(transform.position, detection_radius, LayerMask.GetMask("patrol"));
                var combined = player.Concat(patrols).ToArray();
                var highest = combined.Length;
                if (highest > 0)
                {
                    var chosen = Random.Range(0, highest);
                    target = combined[chosen].gameObject;
                }

            }

            if (GetComponent<LineRenderer>() != null)
            {
                var lr = GetComponent<LineRenderer>();
                lr.enabled = false;

            }


            dirtime -= Time.deltaTime;
            if (dirtime < 0)
            {
                dir = (Random.insideUnitCircle + new Vector2(transform.position.x, transform.position.y) - new Vector2(transform.position.x, transform.position.y)).normalized;
                dirtime = Random.Range(0f, 10f);
            }
        }
    }
/*    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(target_tag) && target == null)
        {
            target = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(target_tag))
        {
            target = null;
        }
    }*/

}
