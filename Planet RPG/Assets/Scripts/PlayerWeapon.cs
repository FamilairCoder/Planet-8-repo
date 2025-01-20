using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayerWeapon : MonoBehaviour
{
    public GameObject current_ship;
    [Header("Weapons stuf-------------------")]
    public GameObject basic_laser_icon;
    public GameObject basic_laser_bullet, laser_rod_icon, laser_rod_bullet, laser_beam_icon, laser_beam_bullet;
    private static List<GameObject> Weapons = new List<GameObject>();
    public static bool using_weapon, basic_laser, laser_rod, laser_beam;
    public static float basic_lasers, laser_beams, laser_rods;
    private float basic_laser_cooldown, laser_rod_cooldown, laser_beam_cooldown;// = .15f;
    [Header("Laser beam stuf-------------------")]
    public GameObject atk_point;
    public GameObject beam_explosion, lineRenderer;
    public float laser_dist, laser_dmg;
    //[Header("Bonuses")]
    //public float armor_bonus, dmg_bonus, firerate_bonus, thrust_bonus, turnspd_bonus;
    // Start is called before the first frame update
    void Start()
    {
        var ship = current_ship.GetComponent<ShipStats>();


        
    }

    // Update is called once per frame
    void Update()
    {

        if (basic_lasers > 0 && !Weapons.Contains(basic_laser_icon))
        {
            basic_laser_icon.GetComponent<Image>().color = new Color(1, 1, 1);
        }
        if (laser_beams > 0 && !Weapons.Contains(laser_beam_icon))
        {
            laser_beam_icon.GetComponent<Image>().color = new Color(1, 1, 1);
        }
        if (laser_rods > 0 && !Weapons.Contains(laser_rod_icon))
        {
            laser_rod_icon.GetComponent<Image>().color = new Color(1, 1, 1);
        }
        /*        if (transform.childCount == 0)
                {
                    var s = Instantiate(ship_prefab, transform.position, transform.rotation, transform);
                    current_ship = s;
                    GetComponent<PlayerMovement>().current_ship = s;
                }*/
        basic_laser_cooldown -= Time.deltaTime;
        laser_rod_cooldown -= Time.deltaTime;
        ActivateAttack();
        
        if (Input.GetMouseButton(0) && !PlayerMovement.dead)
        {
            if (basic_laser && basic_laser_cooldown < 0)
            {
                var ship = current_ship.GetComponent<ShipStats>();
                var rot = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + Random.Range(-3f, 3f));
                CreateBullet(basic_laser_bullet, rot);
                basic_laser_cooldown = Mathf.Clamp(.2f * (1 - ship.firerate_bonus), .015f, 99);
            }
            if (laser_rod && laser_rod_cooldown < 0)
            {
                var ship = current_ship.GetComponent<ShipStats>();
                var rot = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + Random.Range(-3f, 3f));
                CreateBullet(laser_rod_bullet, rot);
                laser_rod_cooldown = Mathf.Clamp(.15f * (1 - ship.firerate_bonus), .025f, 99);
            }
            if (laser_beam)
            {
                PlayerMovement.beam_slow = .5f;
                var lr = lineRenderer.GetComponent<LineRenderer>();
                lr.enabled = true;
                var cast = Physics2D.RaycastAll(atk_point.transform.position, transform.up, laser_dist);
                Vector3 hit = new(0, 0, 0);
                GameObject obj = null;
                foreach (var c in cast)
                {
                    if (c.collider.CompareTag("enemy") && c.collider.gameObject.GetComponent<Health>() != null && c.collider.gameObject.GetComponent<Health>().hp > 0)
                    {
                        hit = c.point;
                        obj = c.collider.gameObject;
                    }

                }


                lr.SetPosition(0, atk_point.transform.position);
                if (hit != new Vector3(0, 0, 0))
                {
                    lr.SetPosition(1, hit);

                    laser_beam_cooldown -= Time.deltaTime;
                    if (laser_beam_cooldown <= 0)
                    {
                        Instantiate(beam_explosion, hit, Quaternion.identity, obj.transform);
                        obj.GetComponent<Health>().hp -= laser_dmg;
                        laser_beam_cooldown = .25f;
                    }
                }
                else
                {
                    lr.SetPosition(1, atk_point.transform.position + atk_point.transform.up * laser_dist);
                }
                
            }

        }
        else if (lineRenderer.GetComponent<LineRenderer>() != null)
        {
            PlayerMovement.beam_slow = 1f;
            var lr = lineRenderer.GetComponent<LineRenderer>();
            lr.enabled = false;

        }
    }

    void ActivateAttack()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && basic_lasers > 0)
        {
            if (!basic_laser)
            {
                basic_laser = true;
                using_weapon = true;
            }
                
            else
            {
                basic_laser = false;
                using_weapon = false;
            }
                
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && laser_rods > 0)
        {
            if (!laser_rod)
            {
                laser_rod = true;
                using_weapon = true;
            }

            else
            {
                laser_rod = false;
                using_weapon = false;
            }

        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && laser_beams > 0)
        {
            if (!laser_beam)
            {
                laser_beam = true;
                using_weapon = true;
            }

            else
            {
                laser_beam = false;
                using_weapon = false;
            }

        }
    }

    void CreateBullet(GameObject bullet, Quaternion rotation)
    {
        var ship = current_ship.GetComponent<ShipStats>();
        var b = Instantiate(bullet, transform.position, rotation);
        b.GetComponent<Bullet>().target_tag = "enemy";
        b.GetComponent<Bullet>().came_from = gameObject;
        b.GetComponent<Bullet>().playerMade = true;
        b.GetComponent<Bullet>().dmg *= (1 + ship.dmg_bonus);
    }
}
