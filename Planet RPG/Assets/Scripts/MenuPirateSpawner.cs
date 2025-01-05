using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPirateSpawner : MonoBehaviour
{
    public List<GameObject> lvl1 = new List<GameObject>();
    public List<GameObject> lvl2 = new List<GameObject>();
    public List<GameObject> lvl3 = new List<GameObject>();
    public float amount;
    private float timeleft;
    // Start is called before the first frame update
    void Start()
    {
        Spawn(amount);
    }

    // Update is called once per frame
    void Update()
    {
        timeleft -= Time.deltaTime;
        if (timeleft < 0)
        {
            var am = Random.Range(1, 4);
            Spawn(am);
            timeleft = Random.Range(0f, 3f);
        }
    }


    void Spawn(float amo)
    {
        for (int i = 0; i < amo; i++)
        {
            var chance = Random.Range(0f, 1f);
            var posx = 0f;
            var posy = 0f;
            if (chance < .25f)
            {
                posx = -52f;
                posy = Random.Range(-30f, 30f);
            }
            else if (chance < .5f)
            {
                posx = 52;
                posy = Random.Range(-30f, 30f);
            }
            else if (chance < .75f)
            {
                posx = Random.Range(-52f, 52f);
                posy = 30f;
            }
            else
            {
                posx = Random.Range(-52f, 52f);
                posy = -30f;
            }
            var pos = new Vector2(posx, posy);
            List<GameObject> toSpawn;
            chance = Random.Range(0f, 1f);
            if (chance < .6f)
            {
                toSpawn = lvl1;
            }
            else if (chance < .85f)
            {
                toSpawn = lvl2;
            }
            else
            {
                toSpawn = lvl3;
            }
            var p = Instantiate(toSpawn[Random.Range(0, toSpawn.Count)], pos, Quaternion.identity);
        }
    }
}
