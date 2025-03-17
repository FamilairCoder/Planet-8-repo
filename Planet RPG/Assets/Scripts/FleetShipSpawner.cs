using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class FleetShipSpawner : MonoBehaviour
{
    public GameObject spawnPoint;
    public float range;
    public List<GameObject> lvl1Pirates = new List<GameObject>();
    public List<GameObject> lvl2Pirates = new List<GameObject>();
    public List<GameObject> lvl3Pirates = new List<GameObject>();

    public GameObject chosenLvl1, chosenLvl2, chosenLvl3;
    public GameObject nav_objLvl1, nav_objLvl2, nav_objLvl3;
    public GameObject createdLvl1Nav, createdLvl2Nav, createdLvl3Nav;
    private MapParralax map_obj;
    public string key;
    // Start is called before the first frame update
    void Start()
    {
        key = gameObject.name + "fleetshipkey";
        chosenLvl1 = SpawnShip(lvl1Pirates, 1);
        chosenLvl2 = SpawnShip(lvl2Pirates, 2);
        chosenLvl3 = SpawnShip(lvl3Pirates, 3);

    }

    // Update is called once per frame
    void Update()
    {
    }

    GameObject SpawnShip(List<GameObject> list, float lvl)
    {
        if (PlayerPrefs.GetFloat(key + "leader" + lvl + "alive", 1) == 1)
        {
            var listIndex = PlayerPrefs.GetInt(key + "list index" + lvl, Random.Range(0, list.Count));
            var xpos = PlayerPrefs.GetFloat(key + "positionx" + lvl, Random.Range(-range, range));
            var ypos = PlayerPrefs.GetFloat(key + "positiony" + lvl, Random.Range(-range, range));
            var pos = new Vector3(spawnPoint.transform.position.x + xpos, spawnPoint.transform.position.y + ypos);
            var a = Instantiate(list[listIndex], pos, Quaternion.identity);
            a.GetComponent<NPCmovement>().pirateLeader = true;
            a.GetComponent<NPCmovement>().stay_around = spawnPoint;
            a.GetComponent<NPCmovement>().stay_radius = range;
            a.GetComponent<NPCmovement>().giveBounty = true;
            a.AddComponent<SquadLocationSetter>();
            SquadLocationSetter sq = a.GetComponent<SquadLocationSetter>();

            var z = CreateLeaderNav(lvl, a);
            if (lvl == 1) createdLvl1Nav = z;
            if (lvl == 2) createdLvl2Nav = z;
            if (lvl == 3) createdLvl3Nav = z;

            var chance = 0f;
            if (lvl == 1)
            {
                for (int i = 0; i < 3; i++)
                {
                    var ind = PlayerPrefs.GetInt(key + "pirate1 index" + lvl + i, Random.Range(0, lvl1Pirates.Count));
                    PlayerPrefs.SetInt(key + "pirate1 index" + lvl + i, ind);
                    sq.shipsToSpawn.Add(lvl1Pirates[ind]);
                }

            }
            else if (lvl == 2)
            {
                for (int i = 0; i < 4; i++)
                {
                    chance = PlayerPrefs.GetFloat(key + "chance" + lvl + i, Random.Range(0f, 1f));
                    if (chance < .5f)
                    {
                        var ind = PlayerPrefs.GetInt(key + "pirate1 index" + lvl + i, Random.Range(0, lvl1Pirates.Count));
                        PlayerPrefs.SetInt(key + "pirate1 index" + lvl + i, ind);
                        sq.shipsToSpawn.Add(lvl1Pirates[ind]);
                    }
                    else
                    {
                        var ind = PlayerPrefs.GetInt(key + "pirate2 index" + lvl + i, Random.Range(0, lvl2Pirates.Count));
                        PlayerPrefs.SetInt(key + "pirate2 index" + lvl + i, ind);
                        Debug.Log(ind);
                        sq.shipsToSpawn.Add(lvl2Pirates[ind]);
                    }
                    PlayerPrefs.SetFloat(key + "chance" + lvl + i, chance);
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    chance = PlayerPrefs.GetFloat(key + "chance" + lvl + i, Random.Range(0f, 1f));
                    if (chance < .3f)
                    {
                        var ind = PlayerPrefs.GetInt(key + "pirate1 index" + lvl + i, Random.Range(0, lvl1Pirates.Count));
                        PlayerPrefs.SetInt(key + "pirate1 index" + lvl + i, ind);
                        sq.shipsToSpawn.Add(lvl1Pirates[ind]);
                    }
                    else if (chance < .7f)
                    {
                        var ind = PlayerPrefs.GetInt(key + "pirate2 index" + lvl + i, Random.Range(0, lvl2Pirates.Count));
                        PlayerPrefs.SetInt(key + "pirate2 index" + lvl + i, ind);
                        sq.shipsToSpawn.Add(lvl2Pirates[ind]);
                    }
                    else
                    {
                        var ind = PlayerPrefs.GetInt(key + "pirate3 index" + lvl + i, Random.Range(0, lvl3Pirates.Count));
                        PlayerPrefs.SetInt(key + "pirate3 index" + lvl + i, ind);
                        sq.shipsToSpawn.Add(lvl3Pirates[ind]);
                    }
                    PlayerPrefs.SetFloat(key + "chance" + lvl + i, chance);
                }
            }
            sq.amount = sq.shipsToSpawn.Count;
            sq.key = key + "leader" + lvl;

            PlayerPrefs.SetFloat(key + "list index" + lvl, listIndex);
            PlayerPrefs.SetFloat(key + "positionx" + lvl, xpos);
            PlayerPrefs.SetFloat(key + "positiony" + lvl, ypos);


            PlayerPrefs.Save();

            return a;
        }
        else
        {
            return null;
        }

    }

    GameObject CreateLeaderNav(float lvl, GameObject toLink)
    {
        GameObject nav = null;
        if (lvl == 1) nav = Instantiate(nav_objLvl1, transform.position, Quaternion.identity);
        if (lvl == 2) nav = Instantiate(nav_objLvl2, transform.position, Quaternion.identity);
        if (lvl == 3) nav = Instantiate(nav_objLvl3, transform.position, Quaternion.identity);

        map_obj = nav.GetComponent<MapParralax>();
        map_obj.linked_obj = toLink;
        map_obj.savekey = gameObject.name + "Fleetmapobj" + lvl;
        return nav;
    }
}
