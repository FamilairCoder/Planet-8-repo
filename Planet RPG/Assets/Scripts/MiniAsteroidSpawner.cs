using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniAsteroidSpawner : MonoBehaviour
{
    public GameObject asteroid, copperAsteroid, ironAsteroid;
    public float minDist, maxDist, amount, oreAmount;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            var dist = Mathf.Sqrt(Random.Range(minDist * minDist, maxDist * maxDist));
            var vec = Random.Range(0f, 360f);
            var pos = new Vector2(transform.position.x + dist * Mathf.Sin(Mathf.Deg2Rad * vec), transform.position.y + dist * Mathf.Cos(Mathf.Deg2Rad * vec));
            var a = Instantiate(asteroid, pos, Quaternion.Euler(0, 0, Random.Range(0f, 360f)), transform);
            var scalx = Random.Range(1f, 5f);
            a.transform.localScale = new Vector2(scalx, scalx + Random.Range(0f, 2f));
        }

        for (int i = 0; i < oreAmount; i++)
        {
            var dist = Mathf.Sqrt(Random.Range(minDist * minDist, maxDist * maxDist));
            var vec = Random.Range(0f, 360f);
            var pos = new Vector2(transform.position.x + dist * Mathf.Sin(Mathf.Deg2Rad * vec), transform.position.y + dist * Mathf.Cos(Mathf.Deg2Rad * vec));

            var chance = Random.Range(0f, 1f);
            GameObject a = null;
            if (chance < .6f) a = Instantiate(copperAsteroid, pos, Quaternion.Euler(0, 0, Random.Range(0f, 360f)), transform);
            else a = Instantiate(ironAsteroid, pos, Quaternion.Euler(0, 0, Random.Range(0f, 360f)), transform);

            var scalx = Random.Range(1f, 2f);
            a.transform.localScale = new Vector2(scalx, scalx + Random.Range(.5f, 1f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
