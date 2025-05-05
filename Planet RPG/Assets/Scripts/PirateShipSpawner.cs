using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateShipSpawner : MonoBehaviour
{
    public List<GameObject> initialShipsToSpawn = new List<GameObject>();
    public List<GameObject> initialShipsSpawned = new List<GameObject>();
    public float initialAmount;
    public List<GameObject> shipsToSpawn = new List<GameObject>();
    public List<GameObject> shipsSpawned = new List<GameObject>();

    private float timeleft = .7f, amountSpawned;
    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        timeleft -= Time.deltaTime;
        if (timeleft < 0)
        {
            shipsSpawned.RemoveAll(ship => ship == null);
            for (int i = 0; i < initialAmount; i++)
            {
                if (i < initialShipsSpawned.Count && initialShipsSpawned[i] == null)
                {
                    SaveManager.SetFloat(gameObject.name + "alive" + i, 0);
                    //initialShipsSpawned.Remove(initialShipsSpawned[i]);
                }
            }
            if (Vector2.Distance(transform.position, HUDmanage.playerReference.transform.position) > 400)
            {
                foreach (GameObject ship in shipsSpawned)
                {
                    ship.SetActive(false);
                }
                foreach (GameObject ship in initialShipsSpawned)
                {
                    if (ship != null) ship.SetActive(false);
                }
            }
            else
            {
                foreach (GameObject ship in shipsSpawned)
                {
                    ship.SetActive(true);
                }
                foreach (GameObject ship in initialShipsSpawned)
                {
                    if (ship != null) ship.SetActive(true);
                }
            }

            SaveManager.SetFloat(gameObject.name + "amountSpawned", shipsSpawned.Count);
            
            timeleft = Random.Range(.25f, .5f);
        }


        
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(.5f);
        for (int i = 0; i < SaveManager.GetFloat(gameObject.name + "amountSpawned", 0); i++)
        {
            var p = Instantiate(shipsToSpawn[Random.Range(0, shipsToSpawn.Count)], transform.position, Quaternion.identity);

            p.GetComponent<NPCmovement>().stay_around = gameObject;
            p.GetComponent<NPCmovement>().stay_radius = 100;
            p.GetComponent<NPCmovement>().dontRetreat = true;
            p.GetComponent<NPCmovement>().key = gameObject.name + "spawned" + i;
            shipsSpawned.Add(p);
        }


        for (int i = 0; i < initialAmount; i++)
        {
            //Debug.Log(i);
            if (SaveManager.GetFloat(gameObject.name + "alive" + i, 1) == 1)
            {
                var dist = Random.Range(0f, 50f);
                var vec = Random.Range(0f, 360f);
                var pos = new Vector2(transform.position.x + (dist * Mathf.Sin(Mathf.Deg2Rad * vec)), transform.position.y + (dist * Mathf.Cos(Mathf.Deg2Rad * vec)));
                var index = SaveManager.GetInt(gameObject.name + "index" + i, Random.Range(0, initialShipsToSpawn.Count));
                var p = Instantiate(initialShipsToSpawn[index], pos, Quaternion.identity);

                p.GetComponent<NPCmovement>().stay_around = gameObject;
                p.GetComponent<NPCmovement>().stay_radius = 70;
                p.GetComponent<NPCmovement>().dontRetreat = true;
                p.GetComponent<NPCmovement>().key = gameObject.name + "initialSpawned" + i;
                initialShipsSpawned.Add(p);
                SaveManager.SetInt(gameObject.name + "index" + i, Random.Range(0, initialShipsToSpawn.Count));
            }
            else
            {
                initialShipsSpawned.Add(null);
            }

        }


        while (true)
        {
            var amount = Random.Range(1, 4);
            var playerHit = Physics2D.OverlapCircle(transform.position, 100, LayerMask.GetMask("playerparts"));
            var patrolHit = Physics2D.OverlapCircle(transform.position, 100, LayerMask.GetMask("patrol"));
            if (playerHit != null || patrolHit != null)
            {
                //Debug.Log("spawning");
                for (int i = 0; i < amount; i++)
                {
                    var p = Instantiate(shipsToSpawn[Random.Range(0, shipsToSpawn.Count)], transform.position, Quaternion.identity);

                    p.GetComponent<NPCmovement>().stay_around = gameObject;
                    p.GetComponent<NPCmovement>().stay_radius = 200;
                    p.GetComponent<NPCmovement>().dontRetreat = true;
                    p.GetComponent<NPCmovement>().key = gameObject.name + "spawned" + i;
                    shipsSpawned.Add(p);
                }
            }
            yield return new WaitForSeconds(10f);
        }
    }
}
