using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject asteroid, ast_bg;
    public List<GameObject> ore_asteroids = new List<GameObject>();
    private List<GameObject> asteroids = new List<GameObject>();
    private List<GameObject> spawnedAsteroids = new List<GameObject>();
    private GameObject player;
    public float min_dist, max_dist;
    private bool close, far;
    public string savekey;
    private float checkTime = 1f;
    private bool started;
    public static bool nearPlayer;
    // Start is called before the first frame update
    void Start()
    {
        player = HUDmanage.playerReference;

        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance < max_dist + 200)
        {
            nearPlayer = true;
            //far = false;
            //if (GetComponent<ParticleSystem>() != null && !GetComponent<ParticleSystem>().isPlaying)
            //{
            //    GetComponent<ParticleSystem>().Play();
            //}
        }
        else
        {
            nearPlayer = false;
            //far = true;
            //if (GetComponent<ParticleSystem>() != null)
            //{
            //    GetComponent<ParticleSystem>().Stop();
            //}
        }

        StartCoroutine(SpawnAsteroids());
    }

    // Update is called once per frame
    void Update()
    {
        checkTime -= Time.deltaTime;
        if (checkTime <= 0 )
        {
            HandleDistanceChecks();
            checkTime = 1f;
        }

    }
    float FloatSaveKey(string name, float numb, float default_value)
    {
        var f = PlayerPrefs.GetFloat(savekey + "" + name + "" + numb, default_value);
        PlayerPrefs.SetFloat(savekey + "" + name + "" + numb, f);
        return f;
    }


    void HandleDistanceChecks()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (!far && distance > max_dist + 200)// && spawnedAsteroids.Count > 0)
        {
            ChangeActive(spawnedAsteroids, false);
            if (GetComponent<ParticleSystem>() != null) GetComponent<ParticleSystem>().Stop();
            far = true;
            nearPlayer = false;
        }
        else if (far && distance < max_dist + 200)// && spawnedAsteroids.Count > 0)
        {
            ChangeActive(spawnedAsteroids, true);
            far = false;
            if (GetComponent<ParticleSystem>() != null && !GetComponent<ParticleSystem>().isPlaying) GetComponent<ParticleSystem>().Play();
            nearPlayer = true;
        }
    }
    void ChangeActive(List<GameObject> list, bool boolean)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            var n = list[i];
            if (n != null)
            {
                n.SetActive(boolean);
            }
            else
            {
                list.RemoveAt(i); // Safely remove from the list
            }
        }
    }

    IEnumerator SpawnAsteroids()
    {
        for (int i = 0; i < max_dist / 4; i++)
        {
            if (PlayerPrefs.GetInt(savekey + i + "alive", 1) == 0) 
            { 
                PlayerPrefs.DeleteKey(savekey + i + "x"); 
                PlayerPrefs.DeleteKey(savekey + i + "y");

                PlayerPrefs.DeleteKey(savekey + "ore_dist" + i);
                PlayerPrefs.DeleteKey(savekey + "ore_distReal" + i);
                PlayerPrefs.SetInt(savekey + i + "alive", 1);

                PlayerPrefs.Save();
            }
            var ore_chance = FloatSaveKey("orechance", i, Random.Range(0f, 1f));

            if (ore_chance < .75f)
            {
                var scalx = FloatSaveKey("ore_scalx", i, Random.Range(1f, 10f));
                var scaly = scalx + FloatSaveKey("ore_scalx_offset", i, Random.Range(0f, 2f));

                var chance = FloatSaveKey("ore_big_chance", i, Random.Range(0f, 1f));
                if (chance < .1f)
                {
                    scalx = FloatSaveKey("ore_big_scalx", i, Random.Range(15f, 30f));
                    scaly = scalx + FloatSaveKey("ore_big_scalx_offset", i, Random.Range(0f, 8f));
                }

                //var pos = new Vector3(transform.position.x + Random.Range(-max_dist, max_dist), transform.position.y + Random.Range(-max_dist, max_dist));
                var dist = Mathf.Sqrt(Random.Range(min_dist * min_dist, max_dist * max_dist));
                var vec = FloatSaveKey("ore_vec", i, Random.Range(0f, 360f));
                var pos = new Vector2(transform.position.x + (FloatSaveKey("ore_dist", i, dist) * Mathf.Sin(Mathf.Deg2Rad * vec)), transform.position.y + (FloatSaveKey("ore_dist", i, dist) * Mathf.Cos(Mathf.Deg2Rad * vec)));
                var a = Instantiate(asteroid, pos, Quaternion.Euler(0, 0, FloatSaveKey("ore_rot", i, Random.Range(0f, 360f))), transform);
                a.transform.localScale = new Vector3(scalx, scaly);
                a.GetComponent<AsteroidInfo>().key = savekey + i;

                chance = FloatSaveKey("what_ore_chance", i, Random.Range(0f, 1f));
                if (chance < .5f) a.GetComponent<AsteroidInfo>().copper = true;
                else if (chance < .8f) a.GetComponent<AsteroidInfo>().iron = true;
                else if (chance < 1f) a.GetComponent<AsteroidInfo>().gold = true;
                spawnedAsteroids.Add(a);

            }
            else
            {
                var scalx = FloatSaveKey("orechanceReal", i, Random.Range(.5f, 5f));


                //var pos = new Vector3(transform.position.x + Random.Range(-max_dist, max_dist), transform.position.y + Random.Range(-max_dist, max_dist));
                var dist = Mathf.Sqrt(Random.Range(min_dist * min_dist, max_dist * max_dist));
                var vec = FloatSaveKey("ore_vecReal", i, Random.Range(0f, 360f));
                var pos = new Vector2(transform.position.x + (FloatSaveKey("ore_distReal", i, dist) * Mathf.Sin(Mathf.Deg2Rad * vec)), transform.position.y + (FloatSaveKey("ore_distReal", i, dist) * Mathf.Cos(Mathf.Deg2Rad * vec)));

                var chance = FloatSaveKey("what_ore_chanceReal", i, Random.Range(0f, 1f));
                GameObject a = null;
                if (chance < .5f) a = Instantiate(ore_asteroids[0], pos, Quaternion.Euler(0, 0, FloatSaveKey("ore_rotReal", i, Random.Range(0f, 360f))), transform);
                else if (chance < .8f) a = Instantiate(ore_asteroids[1], pos, Quaternion.Euler(0, 0, FloatSaveKey("ore_rotReal", i, Random.Range(0f, 360f))), transform);
                else if (chance < 1f) a = Instantiate(ore_asteroids[2], pos, Quaternion.Euler(0, 0, FloatSaveKey("ore_rotReal", i, Random.Range(0f, 360f))), transform);
                a.GetComponent<AsteroidInfo>().key = savekey + i;
                //var a = Instantiate(ore_asteroids[Random.Range(0, ore_asteroids.Count)], pos, Quaternion.Euler(0, 0, Random.Range(0f, 360f)), transform);
                a.GetComponent<AsteroidInfo>().has_ore = true;
                a.transform.localScale = new Vector3(PlayerPrefs.GetFloat(a.GetComponent<AsteroidInfo>().key + "x", scalx), PlayerPrefs.GetFloat(a.GetComponent<AsteroidInfo>().key + "y", scalx));
                spawnedAsteroids.Add(a);
            }
            //yield return new WaitForSeconds(.005f);

        }
        yield return null;
        StopCoroutine(SpawnAsteroids());
    }

}
