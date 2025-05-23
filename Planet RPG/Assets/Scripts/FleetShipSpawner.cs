using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.PlayerSettings;

public class FleetShipSpawner : MonoBehaviour
{
    public List<GameObject> linkedOutposts = new List<GameObject>();
    public GameObject linkedStation;
   
    public GameObject spawnPoint;
    public float range;
    public List<GameObject> lvl1Pirates = new List<GameObject>();
    public List<GameObject> lvl2Pirates = new List<GameObject>();
    public List<GameObject> lvl3Pirates = new List<GameObject>();

    public GameObject chosenLvl1, chosenLvl2, chosenLvl3;
    public GameObject nav_objLvl1, nav_objLvl2, nav_objLvl3, outpostNavObj;
    public GameObject createdLvl1Nav, createdLvl2Nav, createdLvl3Nav;
    public List<GameObject> createdOutpostNavObjs = new List<GameObject>();
    private MapParralax map_obj;
    public string key;
    private float distTime;
    private bool far;
    // Start is called before the first frame update
    void Start()
    {
        key = gameObject.name + "fleetshipkey";
        chosenLvl1 = SpawnShip(lvl1Pirates, 1);
        chosenLvl2 = SpawnShip(lvl2Pirates, 2);
        chosenLvl3 = SpawnShip(lvl3Pirates, 3);

        for (int i = 0; i < linkedOutposts.Count; i++) 
        {
            var nav = Instantiate(outpostNavObj, transform.position, Quaternion.identity);

            map_obj = nav.GetComponent<MapParralax>();
            map_obj.linked_obj = linkedOutposts[i];
            map_obj.savekey = gameObject.name + "Outpostmapobj" + i;
            createdOutpostNavObjs.Add(nav);
        }


    }

    // Update is called once per frame
    void Update()
    {
        distTime -= Time.deltaTime;
        if (distTime <= 0)
        {
            if (far && Vector2.Distance(transform.position, HUDmanage.playerReference.transform.position) < 2000)
            {
                DeactivateShips(chosenLvl1, true);
                DeactivateShips(chosenLvl2, true);
                DeactivateShips(chosenLvl3, true);
                far = false;
            }
            else if (!far && Vector2.Distance(transform.position, HUDmanage.playerReference.transform.position) > 2000)
            {
                DeactivateShips(chosenLvl1, false);
                DeactivateShips(chosenLvl2, false);
                DeactivateShips(chosenLvl3, false);
                far = true;
            }
            distTime = 1f;
        }
    }

    GameObject SpawnShip(List<GameObject> list, float lvl)
    {
        if (SaveManager.GetFloat(key + "leader" + lvl + "alive", 1) == 1)
        {
            var listIndex = SaveManager.GetInt(key + "list index" + lvl, Random.Range(0, list.Count));
            var xpos = SaveManager.GetFloat(key + "positionx" + lvl, Random.Range(-range, range));
            var ypos = SaveManager.GetFloat(key + "positiony" + lvl, Random.Range(-range, range));
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
                    var ind = SaveManager.GetInt(key + "pirate1 index" + lvl + i, Random.Range(0, lvl1Pirates.Count));
                    SaveManager.SetInt(key + "pirate1 index" + lvl + i, ind);
                    sq.shipsToSpawn.Add(lvl1Pirates[ind]);
                }

            }
            else if (lvl == 2)
            {
                for (int i = 0; i < 4; i++)
                {
                    chance = SaveManager.GetFloat(key + "chance" + lvl + i, Random.Range(0f, 1f));
                    if (chance < .5f)
                    {
                        var ind = SaveManager.GetInt(key + "pirate1 index" + lvl + i, Random.Range(0, lvl1Pirates.Count));
                        SaveManager.SetInt(key + "pirate1 index" + lvl + i, ind);
                        sq.shipsToSpawn.Add(lvl1Pirates[ind]);
                    }
                    else
                    {
                        var ind = SaveManager.GetInt(key + "pirate2 index" + lvl + i, Random.Range(0, lvl2Pirates.Count));
                        SaveManager.SetInt(key + "pirate2 index" + lvl + i, ind);
                        //Debug.Log(ind);
                        sq.shipsToSpawn.Add(lvl2Pirates[ind]);
                    }
                    SaveManager.SetFloat(key + "chance" + lvl + i, chance);
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    chance = SaveManager.GetFloat(key + "chance" + lvl + i, Random.Range(0f, 1f));
                    if (chance < .3f)
                    {
                        var ind = SaveManager.GetInt(key + "pirate1 index" + lvl + i, Random.Range(0, lvl1Pirates.Count));
                        SaveManager.SetInt(key + "pirate1 index" + lvl + i, ind);
                        sq.shipsToSpawn.Add(lvl1Pirates[ind]);
                    }
                    else if (chance < .7f)
                    {
                        var ind = SaveManager.GetInt(key + "pirate2 index" + lvl + i, Random.Range(0, lvl2Pirates.Count));
                        SaveManager.SetInt(key + "pirate2 index" + lvl + i, ind);
                        sq.shipsToSpawn.Add(lvl2Pirates[ind]);
                    }
                    else
                    {
                        var ind = SaveManager.GetInt(key + "pirate3 index" + lvl + i, Random.Range(0, lvl3Pirates.Count));
                        SaveManager.SetInt(key + "pirate3 index" + lvl + i, ind);
                        sq.shipsToSpawn.Add(lvl3Pirates[ind]);
                    }
                    SaveManager.SetFloat(key + "chance" + lvl + i, chance);
                }
            }
            sq.amount = sq.shipsToSpawn.Count;
            sq.key = key + "leader" + lvl;

            SaveManager.SetFloat(key + "list index" + lvl, listIndex);
            SaveManager.SetFloat(key + "positionx" + lvl, xpos);
            SaveManager.SetFloat(key + "positiony" + lvl, ypos);


            

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

    void DeactivateShips(GameObject ship, bool boolean)
    {
        var squadScript = ship.GetComponent<SquadLocationSetter>();
        for (int i = 0; i < squadScript.shipsSpawned.Count; i++)
        {
            squadScript.shipsSpawned[i].gameObject.SetActive(boolean);
        }
        ship.SetActive(boolean);

    }
}
