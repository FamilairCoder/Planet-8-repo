using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GraveyardRuinSpawner : MonoBehaviour
{
    public List<GameObject> ruins = new List<GameObject>();
    private List<GameObject> spawned_ruins = new List<GameObject>();
    public float amount, min_dist, max_dist;
    private float check_dist_time = .2f;
    private bool far;
    public static bool nearPlayer;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMining>().gameObject;
        for (int i = 0; i < amount; i++)
        {
            var dist = Mathf.Sqrt(Random.Range(min_dist * min_dist, max_dist * max_dist));
            var vec = Random.Range(0f, 360f);
            var pos = new Vector2(transform.position.x + (dist * Mathf.Sin(Mathf.Deg2Rad * vec)), transform.position.y + (dist * Mathf.Cos(Mathf.Deg2Rad * vec)));
            var r = Instantiate(ruins[Random.Range(0, ruins.Count)], pos, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            if (r.GetComponent<Despawn>() != null) Destroy(r.GetComponent<Despawn>());

            spawned_ruins.Add(r);
        }
    }

    // Update is called once per frame
    void Update()
    {
        check_dist_time -= Time.deltaTime;
        if (check_dist_time <= 0)
        {
            HandleDistanceChecks();
            check_dist_time = .75f; // Reset the timer
        }
    }


    void HandleDistanceChecks()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (!far && distance > max_dist + 200 && spawned_ruins.Count > 0)
        {
            ChangeActive(spawned_ruins, false);
            far = true;
            
            nearPlayer = false;
        }
        else if (far && distance < max_dist + 200 && spawned_ruins.Count > 0)
        {
            ChangeActive(spawned_ruins, true);
            far = false;
            
            nearPlayer = true;
        }
    }

    void ChangeActive(List<GameObject> list, bool boolean)
    {
        foreach (var n in list)
        {
            n.SetActive(boolean);
        }
    }
}
