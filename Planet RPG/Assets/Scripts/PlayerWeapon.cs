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
    [Header("Secondary Weapon stuff-----------------------")]
    public GameObject accumulateObj;
    public GameObject empBullet, torpedo, shield, gravityWellBullet, energyText, oreText;
    public Material accumulateMat, empMat;
    public Color shieldColor;
    private GameObject createdAccumulateObj, createdShield;
    public static float shieldTime;
    private float shieldEnergyTime;
    private float fireEmpTime;
    public AudioSource shieldDeactivateSound;

    [Header("Laser beam stuf-------------------")]
    public GameObject atk_point;
    public GameObject beam_explosion, lineRenderer;
    public float laser_dist, laser_dmg;
    private HUDmanage HUD;
    public float empTime;
    public GameObject empParticle;
    //[Header("Bonuses")]
    //public float armor_bonus, dmg_bonus, firerate_bonus, thrust_bonus, turnspd_bonus;
    // Start is called before the first frame update
    void Start()
    {
        var ship = current_ship.GetComponent<ShipStats>();

        HUD = FindObjectOfType<HUDmanage>();
        
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
        if (!TextInputScript.typing) ActivateAttack();
        if (empTime > 0)
        {
            empTime -= Time.deltaTime;
            if (createdShield != null) Destroy(createdShield);

        }
        if (Input.GetMouseButton(0) && !PlayerMovement.dead && !AccumulateScript.playerCharging && empTime <= 0)
        {
            if (basic_laser && basic_laser_cooldown < 0)
            {
                var ship = current_ship.GetComponent<ShipStats>();
                var rot = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + Random.Range(-3f, 3f));
                CreateBullet(basic_laser_bullet, rot, false, false);
                basic_laser_cooldown = Mathf.Clamp(.2f * (1 - ship.firerate_bonus), .015f, 99);
            }
            if (laser_rod && laser_rod_cooldown < 0)
            {
                var ship = current_ship.GetComponent<ShipStats>();
                var rot = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + Random.Range(-1f, 1f));
                CreateBullet(laser_rod_bullet, rot, false, false);
                laser_rod_cooldown = Mathf.Clamp(.3f * (1 - ship.firerate_bonus), .025f, 99);
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
                        break;
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
        if (!laser_beam)
        {
            PlayerMovement.beam_slow = 1f;
            var lr = lineRenderer.GetComponent<LineRenderer>();
            lr.enabled = false;
        }


        if (HUD.code_has_secondary.Count > 0)
        {
            if (Input.GetMouseButton(1))
            {
                HoldSecondaryAttack();
            }
            if (Input.GetMouseButtonDown(1))
            {
                TapSecondaryAttack();
            }
            if (Input.GetMouseButtonUp(1) || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
            {
                StopSecondary();
            }

        }
            
    }

    void HoldSecondaryAttack()
    {
        var name = HUD.code_has_secondary[HUD.index].name;
        if (name == "AccumulatorIcon")
        {
            shieldEnergyTime -= Time.deltaTime;
            if (createdAccumulateObj == null && (basic_laser || laser_rod || laser_beam))
            {         
                if (EnergyManagement.energy >= 50)
                {
                    createdAccumulateObj = Instantiate(accumulateObj, transform.position, Quaternion.identity);
                    var script = createdAccumulateObj.GetComponent<AccumulateScript>();
                    script.mat = accumulateMat;
                    script.cameFrom = gameObject;
                    script.stats = current_ship.GetComponent<ShipStats>();
                    if (basic_laser) script.pellet = true;
                    else if (laser_rod) script.rod = true;
                    else if (laser_beam) script.beam = true;
                }
                    
                else if (shieldEnergyTime < 0)
                {
                    Instantiate(energyText, transform.position, Quaternion.identity);
                    shieldEnergyTime = 1f;
                }
            }
        }

        else if (name == "EmpIcon")
        {
            fireEmpTime -= Time.deltaTime;
            if (fireEmpTime < 0)
            {
                if (EnergyManagement.energy >= 10)
                {
                    var ship = current_ship.GetComponent<ShipStats>();
                    var rot = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
                    CreateBullet(empBullet, rot, true, false);
                    
                    EnergyManagement.energy -= 10;
                }


                else
                {
                    Instantiate(energyText, transform.position, Quaternion.identity);
                }
                fireEmpTime = 1f;
            }
        }

        else if (name == "ShieldIcon")
        {
            shieldTime -= Time.deltaTime;
            shieldEnergyTime -= Time.deltaTime;
            if (createdShield == null && shieldTime < 0)
            {
                
                if (EnergyManagement.energy >= 4)
                {
                    createdShield = Instantiate(shield, transform.position, Quaternion.identity);
                    var script = createdShield.GetComponent<ShieldStayOn>();
                    script.stayOn = transform.GetChild(0).transform.GetChild(0).gameObject;
                    createdShield.tag = transform.GetChild(0).transform.GetChild(0).tag;
                    createdShield.layer = transform.GetChild(0).transform.GetChild(0).gameObject.layer;
                    createdShield.GetComponent<SpriteRenderer>().color = shieldColor;
                    createdShield.GetComponent<Health>().playerShield = true;
                }


                else if (shieldEnergyTime < 0)
                {
                    Instantiate(energyText, transform.position, Quaternion.identity);
                    shieldEnergyTime = 1f;
                }
                
            }

        }

    }
    void TapSecondaryAttack()
    {
        var name = HUD.code_has_secondary[HUD.index].name;

        if (name == "GravityWellIcon")
        {
            if (EnergyManagement.energy >= 20)
            {
                var b = Instantiate(gravityWellBullet, transform.position, Quaternion.identity);
                b.GetComponent<GravityWellBullet>().dir = transform.up;
                EnergyManagement.energy -= 20;
            }
            else
            {
                Instantiate(energyText, transform.position, Quaternion.identity);
            }
        }
        
        else if (name == "TorpedoIcon")
        {
            if (EnergyManagement.energy >= 5 && PlayerMining.cargo_amount >= 3)
            {
                var rot = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
                CreateBullet(torpedo, rot, false, true);
                EnergyManagement.energy -= 5;
                PlayerMining.cargo_amount -= 3;
                var copper = PlayerMining.cargo_copper;
                var iron = PlayerMining.cargo_iron;
                var gold = PlayerMining.cargo_gold;
                
                if (copper >= 3)
                {
                    PlayerMining.cargo_copper -= 3;
                }
                else if (iron >= 3 - copper)
                {
                    PlayerMining.cargo_copper = 0;
                    PlayerMining.cargo_iron -= 3 - copper;
                }
                else if (gold >= 3 - copper - iron)
                {
                    PlayerMining.cargo_copper = 0;
                    PlayerMining.cargo_iron = 0;
                    PlayerMining.cargo_gold -= 3 - copper - iron;
                }

            }
            else
            {
                if (EnergyManagement.energy < 5) Instantiate(energyText, transform.position, Quaternion.identity);
                else Instantiate(oreText, transform.position, Quaternion.identity);
            }
        }
    }
    void StopSecondary()
    {
        if (createdAccumulateObj != null && !createdAccumulateObj.GetComponent<AccumulateScript>().isFiring)
        {
            Destroy(createdAccumulateObj);
            PlayerMovement.accumulateSlow = 1;
            PlayerMovement.accumulateZoom = 1;
            AccumulateBar.yScale = 0;
            AccumulateScript.playerCharging = false;
            PostProcessManager.abberation = 0;
        }
        if (createdShield != null)
        {
            if (!shieldDeactivateSound.isPlaying) shieldDeactivateSound.Play();
            Destroy(createdShield);
            shieldTime = 3f;
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

                laser_rod = false;
                laser_beam = false;
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

                basic_laser = false;
                laser_beam = false;
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

                basic_laser = false;
                laser_rod = false;
            }

            else
            {
                laser_beam = false;
                using_weapon = false;
            }

        }
    }


    void CreateBullet(GameObject bullet, Quaternion rotation, bool emp, bool isTorpedo)
    {
        var ship = current_ship.GetComponent<ShipStats>();
        var b = Instantiate(bullet, transform.position, rotation);
        b.GetComponent<Bullet>().target_tag = "enemy";
        b.GetComponent<Bullet>().came_from = gameObject;
        b.GetComponent<Bullet>().playerMade = true;
        b.GetComponent<Bullet>().dmg *= (1 + ship.dmg_bonus);
        if (emp) b.GetComponent<Bullet>().empMat = empMat;
        b.GetComponent<Bullet>().torpedo = isTorpedo;
    }
}
