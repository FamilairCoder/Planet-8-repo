using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccumulateScript : MonoBehaviour
{
    public GameObject pelletObj, rodObj, piratePellet;
    [Header("For Laserbeam--------------")]
    public GameObject beamExplosion;
    public LineRenderer lr1, lr2, lr3;
    [Header("Dont set------------------")]
    public GameObject cameFrom;
    public ShipStats stats;
    public Material mat;
    public bool isPirate, pellet, rod, beam, isFiring;
    public static bool playerCharging;
    private float chargeTime, fireTime, shootTime, laserTime;
    private bool shotRod;
    private bool tookEnergy;
    public string targetTag;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<ParticleSystemRenderer>().material = mat;
        GetComponent<ParticleSystemRenderer>().trailMaterial = mat;

        lr1.material = mat;
        lr2.material = mat;
        lr3.material = mat;
    }

    // Update is called once per frame
    void Update()
    {
        if (cameFrom != null)
        {
            transform.position = cameFrom.transform.position;
            chargeTime += Time.deltaTime;
            var chargeRatio = chargeTime / 5f;
            if (!isPirate && chargeTime <= 5)
            {
                playerCharging = true;
                
                PlayerMovement.accumulateSlow = Mathf.Clamp01(Mathf.Lerp(1, .05f, chargeRatio));
                PlayerMovement.accumulateZoom = Mathf.Clamp01(Mathf.Lerp(1, .75f, chargeRatio));
                PostProcessManager.abberation = Mathf.Clamp01(Mathf.Lerp(0, .5f, chargeRatio));
                AccumulateBar.yScale = Mathf.Lerp(0, 2.083f, chargeRatio);
            }
            else if (isPirate && chargeTime <= 5)
            {
                cameFrom.GetComponent<NPCmovement>().chargeSlow = Mathf.Clamp01(Mathf.Lerp(1, .05f, chargeRatio));
            }
            if (chargeTime > 5)
            {
                //if (!isPirate) playerFiring = true;
                isFiring = true;
                GetComponent<ParticleSystem>().Stop();
                fireTime += Time.deltaTime;

                var ratio = fireTime / 3f;
                if (!isPirate)
                {
                    PlayerMovement.accumulateZoom = Mathf.Lerp(.75f, 1, ratio);
                    AccumulateBar.yScale = Mathf.Lerp(2.083f, 0, ratio);
                    PostProcessManager.abberation = Mathf.Lerp(.5f, 0, ratio);
                    if (!tookEnergy) { EnergyManagement.energy -= 50; tookEnergy = true; }
                }
                if (pellet)
                {
                    shootTime -= Time.deltaTime;
                    
                    if (shootTime < 0)
                    {
                        var rot = Mathf.Lerp(0f, 180f, ratio);

                        GameObject b = null;
                        if (!isPirate) b = Instantiate(pelletObj, transform.position, Quaternion.Euler(0, 0, cameFrom.transform.rotation.eulerAngles.z + Random.Range(-rot, rot)));
                        else b = Instantiate(piratePellet, transform.position, Quaternion.Euler(0, 0, cameFrom.transform.rotation.eulerAngles.z + Random.Range(-rot, rot)));
                        var script = b.GetComponent<Bullet>();
                        if (!isPirate) { script.target_tag = "enemy"; script.playerMade = true; }
                        else { script.target_tag = targetTag; }
                        if (isPirate) script.came_from = cameFrom.GetComponent<NPCmovement>().weapons[0];                        
                        else script.came_from = cameFrom;
                        script.dmg *= (1 + stats.dmg_bonus);
                        if (fireTime < 2) shootTime = .01f;
                        else shootTime = .05f;
                    }

                }
                if (rod)
                {
                    fireTime += Time.deltaTime;
                    if (!shotRod)
                    {
                        var b = Instantiate(rodObj, transform.position, Quaternion.Euler(0, 0, cameFrom.transform.rotation.eulerAngles.z));
                        b.GetComponent<RailBullet>().mat = mat;
                        b.GetComponent<RailBullet>().cameFrom = cameFrom;
                        b.GetComponent<RailBullet>().isPirate = isPirate;
                        b.GetComponent<RailBullet>().dmg = 10 * (1 + stats.dmg_bonus);
                        shotRod = true;
                    }
                }

                if (beam)
                {

                    FireLaser(cameFrom.transform.position + cameFrom.transform.right * 1, lr1);
                    FireLaser(cameFrom.transform.position, lr2);
                    FireLaser(cameFrom.transform.position - cameFrom.transform.right * 1, lr3);
                    if (!isPirate) PlayerMovement.accumulateSlow = .01f;
                    else cameFrom.GetComponent<NPCmovement>().chargeSlow = .01f;


                }
                if (fireTime >= 3)
                {
                    if (!isPirate)
                    {
                        AccumulateBar.yScale = 0;
                        PlayerMovement.accumulateSlow = 1;
                        PostProcessManager.abberation = 0;
                        playerCharging = false;
                        
                    }
                    else
                    {
                        cameFrom.GetComponent<NPCmovement>().chargeSlow = 1;
                    }
                    Destroy(gameObject);
                }
            }
        }
        else { Destroy(gameObject); if (!isPirate) { PlayerMovement.accumulateSlow = 1; AccumulateBar.yScale = 0; playerCharging = false; PostProcessManager.abberation = 0; } }
    }

    void FireLaser(Vector3 position, LineRenderer lr)
    {
        lr.enabled = true;
        var cast = Physics2D.RaycastAll(position, cameFrom.transform.up, 200);
        Vector3 hit = new(0, 0, 0);
        GameObject obj = null;
        foreach (var c in cast)
        {
            var col = c.collider;
            if (cameFrom != null && (cameFrom.transform.parent == null || !col.gameObject.transform.IsChildOf(cameFrom.transform.parent)) && c.collider.gameObject.GetComponent<Health>() != null && c.collider.gameObject.GetComponent<Health>().hp > 0)
            {
                if ((!isPirate && col.CompareTag("enemy")) || (isPirate && HUDmanage.pirateTags.Contains(c.collider.tag)))
                {
                    hit = c.point;
                    obj = c.collider.gameObject;
                    break;
                }

            }

        }


        lr.SetPosition(0, position);
        if (hit != new Vector3(0, 0, 0))
        {
            lr.SetPosition(1, hit);

            laserTime -= Time.deltaTime;
            if (laserTime <= 0)
            {
                Instantiate(beamExplosion, hit, Quaternion.identity, obj.transform);
                obj.GetComponent<Health>().hp -= 3 * (1 + stats.dmg_bonus);
                laserTime = .2f;
            }
        }
        else
        {
            lr.SetPosition(1, position + cameFrom.transform.up * 200);
        }
    }
}
