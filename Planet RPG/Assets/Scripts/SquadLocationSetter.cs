using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadLocationSetter : MonoBehaviour
{
    private List<Vector3> positions = new List<Vector3>();
    public List<GameObject> shipsToSpawn = new List<GameObject>();
    public List<GameObject> shipsSpawned = new List<GameObject>();
    
    public float amount, min_dist, max_dist, spd;
    public string key;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<NPCmovement>().squadKey = key;
        for (int i = 0; i < amount; i++)
        {
            if (GetComponent<NPCmovement>() == null || GetComponent<NPCmovement>().is_npc)
            {
                var dist = Random.Range(min_dist, max_dist);
                var vec = Random.Range(0f, 360f);
                var pos = new Vector2(transform.position.x + (dist * Mathf.Sin(Mathf.Deg2Rad * vec)), transform.position.y + (dist * Mathf.Cos(Mathf.Deg2Rad * vec)));
                var s = Instantiate(shipsToSpawn[Random.Range(0, shipsToSpawn.Count)], pos, Quaternion.identity);
                var g = new GameObject("point");
                g.transform.position = pos;
                g.transform.parent = transform.parent;
                s.GetComponent<NPCmovement>().squadPoint = g;
                s.GetComponent<NPCmovement>().squadLeader = transform.parent.gameObject;
                //s.GetComponent<ShipStats>().spd = spd;
                s.AddComponent<Despawn>();
            }
            else if (PlayerPrefs.GetFloat(key + "ship" + i + "alive", 1) == 1)
            {
                var dist = Random.Range(0, 20);
                var vec = Random.Range(0f, 360f);
                var pos = new Vector2(transform.position.x + (dist * Mathf.Sin(Mathf.Deg2Rad * vec)), transform.position.y + (dist * Mathf.Cos(Mathf.Deg2Rad * vec)));
                var s = Instantiate(shipsToSpawn[i], pos, Quaternion.identity);
                s.GetComponent<NPCmovement>().inSquad = true;
                s.GetComponent<NPCmovement>().playerPos = gameObject.transform;
                s.GetComponent<NPCmovement>().squadKey = key + "ship" + i;

                shipsSpawned.Add(s);
            }

        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
