using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PatrolSpawner : MonoBehaviour
{
    public string key;
    public float numb, maxDist;


    public List<GameObject> patrols = new List<GameObject>();
    public List<GameObject> spawnedPatrols = new List<GameObject>();

    private Transform player;
    private bool far;
    private float createTimeleft = 600;
    // Start is called before the first frame update
    void Start()
    {
        player = HUDmanage.playerReference.transform;
        if (key == "") key = gameObject.name + "patrolspot";

        for (int i = 0; i < numb; i++)
        {
            if (PlayerPrefs.GetFloat("alive" + i + key, 1) == 1)
            {
                SpawnPatrol(i);
            }
            else
            {
                spawnedPatrols.Add(null);
            }
            
        }
        createTimeleft = PlayerPrefs.GetFloat(key + "timeleft", 600);
        StartCoroutine(CheckSpawned());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnPatrol(float id)
    {
        var index = PlayerPrefs.GetInt("index" + id, Random.Range(0, patrols.Count));

        var dist = Random.Range(0, maxDist);
        var vec = Random.Range(0f, 360f);
        var pos = new Vector2(transform.position.x + (dist * Mathf.Sin(Mathf.Deg2Rad * vec)), transform.position.y + (dist * Mathf.Cos(Mathf.Deg2Rad * vec)));
        GameObject p = Instantiate(patrols[index], pos, Quaternion.identity);
        p.GetComponent<NPCmovement>().stay_around = gameObject;
        p.GetComponent<NPCmovement>().stay_radius = maxDist;
        p.GetComponent<PatrolID>().id = id + key;
        p.GetComponent<PatrolID>().spawnCameFrom = GetComponent<PatrolSpawner>();

        PlayerPrefs.SetFloat("alive" + id + key, 1);
        PlayerPrefs.SetInt("index" + id, index);
        PlayerPrefs.Save();
        //key += 1;
        spawnedPatrols.Insert((int)id, p);

        if (PlayerPrefs.GetFloat("taken" + id + key, 0) == 1)
        {
            p.SetActive(false);
        }

    }


        private IEnumerator CheckSpawned()
    {

        while (true)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);

            if (!far && distance > 500)// && npc_spawned_ships.Count > 0)
            {
                ChangeActive(spawnedPatrols, false);

                far = true;
            }
            else if (far && distance < 500)// && npc_spawned_ships.Count > 0)
            {


                ChangeActive(spawnedPatrols, true);
                far = false;
            }

            for (int i = 0; i < numb; i++)
            {
                
                if (spawnedPatrols[i] == null)
                {
                    PlayerPrefs.SetFloat("alive" + i + key, 0);
                    PlayerPrefs.DeleteKey("index" + i + key);
                    
                }

                
            }


            if (createTimeleft <= 0)
            {
                for (int i = 0; i < numb; i++)
                {
                    //Debug.Log(PlayerPrefs.GetFloat(key + "alive" + i));
                    if (PlayerPrefs.GetFloat("alive" + i + key, 1) == 0)
                    {
                        spawnedPatrols.Remove(spawnedPatrols[i]);
                        SpawnPatrol(i);

                    }
                }
                createTimeleft = 600;
            }
            PlayerPrefs.SetFloat(key + "timeleft", createTimeleft);
            PlayerPrefs.Save();
            //Debug.Log(createTimeleft);
            createTimeleft -= 1;

            yield return new WaitForSeconds(1);
        }
    }

    void ChangeActive(List<GameObject> list, bool boolean)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            var n = list[i];
            if (n != null && !n.GetComponent<NPCmovement>().attackedByPlayer && !n.GetComponent<PatrolID>().taken)
            {
                n.SetActive(boolean);
            }
        }
    }
}
