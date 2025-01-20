using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    //public GameObject ship;
    public bool spawn_npcs;
    public float npc_amount;
    public string savekey;
    public List<GameObject> ships = new List<GameObject>();
    public List<GameObject> lvl2_ships = new List<GameObject>();
    public List<GameObject> lvl3_ships = new List<GameObject>();
    public List<GameObject> npc_ships = new List<GameObject>();

    public float max_dist, min_dist, max_numb, spawn_time;
    private float time_left, delay = .1f, numb, numblvl2, numblvl3, check_dist_time = .2f, check_ship_time = .5f;
    public List<int> ship_types = new List<int>();
    public List<float> armor_bonuses = new List<float>();
    public List<GameObject> spawned_ships = new List<GameObject>();
    public List<GameObject> spawned_shipsLvl2 = new List<GameObject>();
    public List<GameObject> spawned_shipsLvl3 = new List<GameObject>();
    public List<GameObject> npc_spawned_ships = new List<GameObject>();
    private bool did, far;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMining>().gameObject;
        if (savekey == "") savekey = gameObject.name;

        if (spawn_npcs)
        {
            for (int i = 0; i < npc_amount; i++)
            {
                SpawnNPC();
            }
        }

        time_left = spawn_time;
    }

    // Update is called once per frame
    void Update()
    {


        delay -= Time.deltaTime;
        if (delay <= 0 && !did)
        {
            SpawnInitialShips();
            delay = .1f;
        }

        if (check_ship_time <= 0)
        {
            CleanupDestroyedShips();
            check_ship_time = .5f; // Reset the timer
        }

        time_left -= Time.deltaTime;
        if (time_left <= 0 && did)
        {
            SpawnMoreShips();
            time_left = spawn_time; // Reset the timer
        }
        check_dist_time -= Time.deltaTime;
        //Debug.Log(check_dist_time);
        if (check_dist_time <= 0)
        {
            //Debug.Log("checking dist");
            HandleDistanceChecks();
            check_dist_time = 1f; // Reset the timer
        }
    }

    void HandleDistanceChecks()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (!far && distance > max_dist)// && npc_spawned_ships.Count > 0)
        {
            ChangeActive(npc_spawned_ships, false);
            ChangeActive(spawned_ships, false);
            ChangeActive(spawned_shipsLvl2, false);
            ChangeActive(spawned_shipsLvl3, false);
            far = true;
        }
        else if (far && distance < max_dist)// && npc_spawned_ships.Count > 0)
        {

            ChangeActive(npc_spawned_ships, true);
            ChangeActive(spawned_ships, true);
            ChangeActive(spawned_shipsLvl2, true);
            ChangeActive(spawned_shipsLvl3, true);
            far = false;
        }

    }

    void SpawnInitialShips()
    {
        for (int i = 0; i < max_numb;)
        {
            if (PlayerPrefs.GetFloat(savekey + numb + "alivelvl1", 1) == 1)
            {
                PlayerPrefs.SetFloat(savekey + numb + "alivelvl1", 1);
                SpawnShip(ships, 1, numb);
                i++;
            }
            numb++;
        }

        for (int i = 0; i < max_numb;)
        {
            if (PlayerPrefs.GetFloat(savekey + numblvl2 + "alivelvl2", 1) == 1)
            {
                PlayerPrefs.SetFloat(savekey + numblvl2 + "alivelvl2", 1);
                SpawnShip(lvl2_ships, 2, numblvl2);
                i++;
            }
            numblvl2++;
        }
        for (int i = 0; i < max_numb;)
        {
            if (PlayerPrefs.GetFloat(savekey + numblvl3 + "alivelvl3", 1) == 1)
            {
                PlayerPrefs.SetFloat(savekey + numblvl3 + "alivelvl3", 1);
                SpawnShip(lvl3_ships, 3, numblvl3);
                i++;
            }
            numblvl3++;
        }

        numb = 0;
        numblvl2 = 0;
        numblvl3 = 0;
        did = true;
    }

    void SpawnMoreShips()
    {
        for (int i = 0; i < max_numb;)
        {
            if (spawned_ships.Count < max_numb)
            {
                if (PlayerPrefs.GetFloat(savekey + numb + "alive", 1) == 1)
                {
                    PlayerPrefs.SetFloat(savekey + numb + "alive", 1);
                    SpawnShip(ships, 1, numb);
                    i++;
                }
                numb++;
            }
            else
            {
                i++;
            }
        }

        for (int i = 0; i < max_numb;)
        {
            if (spawned_shipsLvl2.Count < max_numb)
            {
                if (PlayerPrefs.GetFloat(savekey + numblvl2 + "alive", 1) == 1)
                {
                    PlayerPrefs.SetFloat(savekey + numblvl2 + "alive", 1);
                    SpawnShip(lvl2_ships, 2, numblvl2);
                    i++;
                }
                numblvl2++;
            }
            else
            {
                i++;
            }
        }
        for (int i = 0; i < max_numb;)
        {
            if (spawned_shipsLvl3.Count < max_numb)
            {
                if (PlayerPrefs.GetFloat(savekey + numblvl3 + "alive", 1) == 1)
                {
                    PlayerPrefs.SetFloat(savekey + numblvl3 + "alive", 1);
                    SpawnShip(lvl3_ships, 3, numblvl3);
                    i++;
                }
                numblvl3++;
            }
            else
            {
                i++;
            }
        }
    }

    void CleanupDestroyedShips()
    {
        spawned_ships.RemoveAll(ship => ship == null);
        spawned_shipsLvl2.RemoveAll(ship => ship == null);
        spawned_shipsLvl3.RemoveAll(ship => ship == null);
    }

    void SpawnShip(List<GameObject> ship_list, float lvl, float numb)
    {
        var type = Random.Range(0, ship_list.Count);
        var ship_to_spawn = ship_list[IntSaveKey("ship type" + lvl, type, numb)];
        ship_types.Add(type);

        var dist = FloatSaveKey("dist" + lvl, Random.Range(min_dist, max_dist), numb);
        var vec = FloatSaveKey("vec" + lvl, Random.Range(0f, 360f), numb);
        var pos = new Vector2(transform.position.x + (dist * Mathf.Sin(Mathf.Deg2Rad * vec)), transform.position.y + (dist * Mathf.Cos(Mathf.Deg2Rad * vec)));
        var rot = Quaternion.Euler(0, 0, FloatSaveKey("rotation" + lvl, Random.Range(0f, 360f), numb));

        var a = Instantiate(ship_to_spawn, pos, rot);
        a.GetComponent<NPCmovement>().stay_radius = max_dist;
        a.GetComponent<NPCmovement>().stay_around = gameObject;
        a.GetComponent<NPCmovement>().key = numb.ToString() + lvl.ToString();

        AssignBonuses(a, lvl, numb);

        if (lvl == 1) spawned_ships.Add(a);
        if (lvl == 2) spawned_shipsLvl2.Add(a);
        if (lvl == 3) spawned_shipsLvl3.Add(a);
    }

    void AssignBonuses(GameObject a, float lvl, float numb)
    {
        var chance = Random.Range(0f, 1f);
        if (chance > .5f)
        {
            var stats = a.GetComponent<ShipStats>();
            var armor_bonus = FloatSaveKey("armor_bonus" + lvl, Random.Range(0, 5), numb);
            stats.dmg_bonus = FloatSaveKey("dmg_bonus" + lvl, Random.Range(0f, 1f), numb);
            stats.firerate_bonus = FloatSaveKey("firerate_bonus" + lvl, Random.Range(.5f, 1f), numb);
            stats.thrust_bonus = FloatSaveKey("thrust_bonus" + lvl, Random.Range(0f, 1f), numb);
            stats.turnspd_bonus = FloatSaveKey("turnspd_bonus" + lvl, Random.Range(0f, 1.5f), numb);

            foreach (Transform child in stats.transform)
            {
                var health = child.GetComponent<Health>();
                if (health != null)
                {
                    health.hp += armor_bonus;
                    health.orig_hp = health.hp; 
                }
            }
            stats.bounty_bonus = Mathf.Round((stats.dmg_bonus * 80 + Mathf.Lerp(160, 0, stats.firerate_bonus) + stats.thrust_bonus * 40 + stats.turnspd_bonus * 25 + armor_bonus * 15 - 40) / 10) * 10;
        }
    }

    void SpawnNPC()
    {
        var dist = Random.Range(0, max_dist / 2);
        var vec = Random.Range(0f, 360f);
        var pos = new Vector2(transform.position.x + (dist * Mathf.Sin(Mathf.Deg2Rad * vec)), transform.position.y + (dist * Mathf.Cos(Mathf.Deg2Rad * vec)));
        var npc = Instantiate(npc_ships[Random.Range(0, npc_ships.Count)], pos, Quaternion.identity);
        npc.GetComponent<NPCmovement>().stay_around = gameObject;
        npc_spawned_ships.Add(npc);
    }

    float FloatSaveKey(string name, float default_value, float numb)
    {
        var f = PlayerPrefs.GetFloat(savekey + name + numb, default_value);
        PlayerPrefs.SetFloat(savekey + name + numb, f);
        return f;
    }

    int IntSaveKey(string name, int default_value, float numb)
    {
        var i = PlayerPrefs.GetInt(savekey + name + numb, default_value);
        PlayerPrefs.SetInt(savekey + name + numb, i);
        return i;
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

}
