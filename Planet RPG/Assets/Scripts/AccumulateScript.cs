using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccumulateScript : MonoBehaviour
{
    public GameObject pelletObj, rodObj, piratePellet;
    [Header("Dont set------------------")]
    public GameObject cameFrom;
    public ShipStats stats;
    public Material mat;
    public bool isPirate, pellet, rod, beam, isFiring;
    private float chargeTime, fireTime, shootTime;
    private bool shotRod;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<ParticleSystemRenderer>().material = mat;
        GetComponent<ParticleSystemRenderer>().trailMaterial = mat;
    }

    // Update is called once per frame
    void Update()
    {
        if (cameFrom != null)
        {
            transform.position = cameFrom.transform.position;
            chargeTime += Time.deltaTime;
            if (!isPirate)
            {
                var chargeRatio = chargeTime / 5f;
                PlayerMovement.accumulateSlow = Mathf.Clamp01(Mathf.Lerp(1, .05f, chargeRatio));
                PlayerMovement.accumulateZoom = Mathf.Clamp01(Mathf.Lerp(1, .75f, chargeRatio));
                AccumulateBar.yScale = Mathf.Lerp(0, 2.083f, chargeRatio);
            }
            if (chargeTime > 5)
            {
                isFiring = true;
                GetComponent<ParticleSystem>().Stop();
                fireTime += Time.deltaTime;

                var ratio = fireTime / 3f;
                PlayerMovement.accumulateZoom = 1;
                AccumulateBar.yScale = Mathf.Lerp(2.083f, 0, ratio);
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
                        script.came_from = cameFrom;                        
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
                        shotRod = true;
                    }
                }
                if (fireTime >= 3)
                {
                    AccumulateBar.yScale = 0;
                    PlayerMovement.accumulateSlow = 1;
                    Destroy(gameObject);
                }
            }
        }
        else { Destroy(gameObject); PlayerMovement.accumulateSlow = 1; AccumulateBar.yScale = 0; }
    }
}
