using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingSpawner : MonoBehaviour
{
    public List<GameObject> pirateShips = new List<GameObject>();
    public List<GameObject> ruins = new List<GameObject>();
    public Transform ruinParent;
    public List<GameObject> randObjs = new List<GameObject>();
    public List<GameObject> randAsteroids = new List<GameObject>();
    public static float totalDeliveries;
    public string key;
    private float checkTime, pirateChance, pirateDelay = .2f, objIncrease, astIncrease;
    private Vector3 currentPos;
    private bool pirateDid;
    private int pirateNumb;

    // Start is called before the first frame update
    void Start()
    {
        currentPos = transform.position;
        totalDeliveries = PlayerPrefs.GetFloat(key + "totalD", 0);

/*        for (int i = 0; i < 000; i++)
        {
            var x = PlayerPrefs.GetFloat("bgruins x" + i, Random.Range(-8000f, 8000f));
            var y = PlayerPrefs.GetFloat("bgruins y" + i, Random.Range(-8000f, 8000f));
            var rot = PlayerPrefs.GetFloat("bgruins rot" + i, Random.Range(0f, 360f));
            var index = PlayerPrefs.GetInt("bgruins index" + i, Random.Range(0, ruins.Count));
            var pos = new Vector2(x, y);
            var r = Instantiate(ruins[index], pos, Quaternion.identity, ruinParent);
            r.transform.localRotation = Quaternion.Euler(0, 0, rot);
            PlayerPrefs.SetFloat("bgruins x" + i, x);
            PlayerPrefs.SetFloat("bgruins y" + i, y);
            PlayerPrefs.SetFloat("bgruins rot" + i, rot);
            PlayerPrefs.SetInt("bgruins index" + i, index);
        }*/
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        checkTime -= Time.deltaTime;
/*        pirateDelay -= Time.deltaTime;
        if (pirateDelay < 0 && !pirateDid)
        {
            for (int i = 0; i < totalDeliveries; i++)
            {
                if (PlayerPrefs.GetInt(key + "pirateAlive" + pirateNumb, 0) == 1)
                {
                    var type = PlayerPrefs.GetInt(key + "pirateType" + pirateNumb, Random.Range(0, pirateShips.Count));
                    PlayerPrefs.SetInt(key + "pirateType" + pirateNumb, type);

                    var p = Spawn(pirateShips[type], 150, 0);
                    p.GetComponent<NPCmovement>().key = "spawned pirate " + pirateNumb;
                    p.GetComponent<NPCmovement>().dir = (transform.position - p.transform.position).normalized;
                    p.GetComponent<NPCmovement>().rand_time = 10;
                    p.GetComponent<NPCmovement>().inhibit = true;
                    spawnedPirateShips.Insert(pirateNumb, p);
                }

                PlayerPrefs.Save();
                pirateNumb++;
            }
            pirateDid = true;
        }*/
        if (checkTime < 0)
        {
            PlayerPrefs.SetFloat(key + "totalD", totalDeliveries);
            PlayerPrefs.Save();

            if (Vector2.Distance(currentPos, transform.position) > 150)
            {
 
                var astHit = Physics2D.OverlapCircleAll(transform.position, 200, LayerMask.GetMask("asteroids"));
                var stationHit = Physics2D.OverlapCircleAll(transform.position, 200, LayerMask.GetMask("station"));
                if (stationHit.Length <= 0 && astHit.Length <= 0)
                {
                    if (totalDeliveries > 0)
                    {
                        var chance = Random.Range(0f, 1f);
                        pirateChance = Mathf.Lerp(0, .4f, totalDeliveries / 15);
                        pirateChance = Mathf.Clamp(pirateChance, 0, .4f);
                        if (chance < pirateChance)
                        {
                            var pirateAmount = Mathf.Lerp(1, 5, totalDeliveries / 10);
                            var amount = Random.Range(0, pirateAmount);

                            for (int i = 0; i < amount - 1; i++)
                            {
                                //var vec = Random.Range(0f, 360f);
                                //var dist = 100;
                                //var pos = new Vector2(transform.position.x + (dist * Mathf.Sin(Mathf.Deg2Rad * vec)), transform.position.y + (dist * Mathf.Cos(Mathf.Deg2Rad * vec)));
                                //var p = Instantiate(pirateShips[Random.Range(0, pirateShips.Count)], pos, Quaternion.identity);
                                var type = PlayerPrefs.GetInt(key + "pirateType" + pirateNumb, Random.Range(0, pirateShips.Count));
                                PlayerPrefs.SetInt(key + "pirateType" + pirateNumb, type);

                                var p = Spawn(pirateShips[Random.Range(0, pirateShips.Count)], Random.Range(180f, 220f), 15);
                                p.GetComponent<NPCmovement>().key = "spawned pirate " + pirateNumb;
                                p.GetComponent<NPCmovement>().dir = (transform.position - p.transform.position).normalized;
                                p.GetComponent<NPCmovement>().rand_time = 10;
                                var inhibitChance = Random.Range(0f, 1f);
                                if (inhibitChance < .3f)
                                {
                                    p.GetComponent<NPCmovement>().inhibit = true;
                                }                                
                                p.GetComponent<NPCmovement>().giveBounty = true;
                                p.AddComponent<Despawn>();
                                //p.GetComponent<NPCmovement>().d = (transform.position - p.transform.position).normalized;
                                pirateNumb++;
                                //p.GetComponent<NPCmovement>();
                            }
                        }

                    }
                    var objChance = Random.Range(0f, 1f);
                    var astChance = Random.Range(0f, 1f);
                    if (objChance < .1f + objIncrease)
                    {
                        var o = Spawn(randObjs[Random.Range(0, randObjs.Count)], 200, 30);
                        o.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
                        if (o.GetComponent<NPCmovement>() != null) { o.GetComponent<NPCmovement>().rand_time = 9999f; o.GetComponent<NPCmovement>().dir = o.transform.up; }
                        if (o.GetComponent<MiniAsteroidSpawner>() != null) { o.transform.position += transform.up * 100; }
                        objIncrease = 0;
                    }
                    else
                    {
                        objIncrease += .02f;
                    }
                    astHit = Physics2D.OverlapCircleAll(transform.position, 400, LayerMask.GetMask("asteroids"));
                    if (astChance < .1f + astIncrease && astHit.Length <= 0)
                    {
                        var o = Spawn(randObjs[Random.Range(0, randAsteroids.Count)], 200, 50);
                        o.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
                        if (o.GetComponent<MiniAsteroidSpawner>() != null) { o.transform.position += transform.up * 100; }
                        astIncrease = 0;
                    }
                    else if (astHit.Length <= 0)
                    {
                        astIncrease += .01f;
                    }
                }
                
                currentPos = transform.position;
            }
            checkTime = 1;
        }
    }


    GameObject Spawn(GameObject obj, float dist, float offsetRadius)
    {
        //var pos = transform.position + transform.up * dist;
        //pos += new Vector3(Random.Range(-offsetRadius, offsetRadius), Random.Range(-offsetRadius, offsetRadius), 0);
        //var vec = Random.Range(-offsetRadius, offsetRadius);
        //var pos = new Vector2(transform.position.x + (dist * Mathf.Sin(Mathf.Deg2Rad * vec)), transform.position.y + (dist * Mathf.Cos(Mathf.Deg2Rad * vec)));

        var playerDirection = transform.up;

        // Randomly offset the angle by a small amount to either side of the direction
        float randomAngle = Random.Range(-offsetRadius, offsetRadius);

        // Convert the random angle to radians
        float angleInRadians = Mathf.Atan2(playerDirection.y, playerDirection.x) + Mathf.Deg2Rad * randomAngle;

        // Calculate the spawn position based on the angle and distance
        var pos = new Vector2(
            transform.position.x + dist * Mathf.Cos(angleInRadians),
            transform.position.y + dist * Mathf.Sin(angleInRadians)
        );

        var o = Instantiate(obj, pos, Quaternion.identity);

        return o;
    }
}
