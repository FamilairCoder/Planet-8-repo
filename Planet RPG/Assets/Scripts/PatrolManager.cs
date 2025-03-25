using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolManager : MonoBehaviour
{
    public GameObject focusIndicator, holdIndicator;
    public List<GameObject> spawnPatrols = new List<GameObject>();
    public static List<GameObject> patrols = new List<GameObject>();
    private float numb;
    public static bool focusFire, holdFire, holdPosition;
    public static GameObject focusTarget, createdIndicator, createdHoldIndicator;
    public static bool createdFocus = false, createdHold = false;
    // Start is called before the first frame update
    void Start()
    {
        numb = PlayerPrefs.GetFloat("numb of patrols", 0);
        for (int i = 0; i < numb; i++)
        {
            //Debug.Log("looping");
            var id = PlayerPrefs.GetString("id" + i);
            //Debug.Log(PlayerPrefs.GetFloat(key + "alive" + i));
            if (PlayerPrefs.GetFloat("alive" + id, 1) == 1)
            {
                //Debug.Log("spawning");
                //patrols.Remove(patrols[i]);
                SpawnPatrol(id, i);

            }
        }
        StartCoroutine(SaveFleet());
    }

    // Update is called once per frame
    void Update()
    {
        if (focusFire && !createdFocus)
        {
            createdIndicator = Instantiate(focusIndicator, transform.position, Quaternion.identity);
            createdFocus = true;
        }
        if (holdPosition && !createdHold)
        {
            createdHoldIndicator = Instantiate(holdIndicator, transform.position, Quaternion.identity);
            createdHold = true;
        }
        else if (!holdPosition && createdHold)
        {
            Destroy(createdHoldIndicator);
            createdHold = false;
        }
    }


    void SpawnPatrol(string id, float num)
    {
        var index = PlayerPrefs.GetInt("index" + id, Random.Range(0, patrols.Count));
        //Debug.Log(index);
        var dist = Random.Range(10, 20);
        var vec = Random.Range(0f, 360f);
        var pos = new Vector2(transform.position.x + (dist * Mathf.Sin(Mathf.Deg2Rad * vec)), transform.position.y + (dist * Mathf.Cos(Mathf.Deg2Rad * vec)));
        GameObject p = Instantiate(spawnPatrols[index], pos, Quaternion.identity);
        p.GetComponent<NPCmovement>().stay_around = gameObject;
        p.GetComponent<NPCmovement>().stay_radius = PatrolID.stayDist;
        p.GetComponent<PatrolID>().id = id;
        p.GetComponent<PatrolID>().taken = true;
        p.GetComponent<PatrolID>().didTaken = true;
        p.GetComponent<PatrolID>().Hire();
        //p.GetComponent<PatrolID>().spawnCameFrom = GetComponent<PatrolSpawner>();


        while (patrols.Count <= num)
        {
            patrols.Add(null); // Fill with default values to create space
        }
        patrols[(int)num] = p;
    }



    IEnumerator SaveFleet()
    {
        while (true)
        {
            PlayerPrefs.SetFloat("numb of patrols", patrols.Count);
            //Debug.Log(patrols.Count);
            for(int i = 0; i < patrols.Count; i++)
            {
                var p = patrols[i];
                if (p != null)
                {
                    var pID = p.GetComponent<PatrolID>();
                    if (PlayerPrefs.GetFloat("alive" + pID.id) == 1)
                    {
                        PlayerPrefs.SetString("id" + i, pID.id);
                    }
                }

            }
            PlayerPrefs.Save();
            yield return new WaitForSeconds(1f);
        }

    }
}
