using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAsteroidSpawner : MonoBehaviour
{
    public GameObject asteroid;
    public float min_dist, max_dist, amount;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < amount; i++)
        {



            var scalx = Random.Range(0f, .5f);

            var dist = Mathf.Sqrt(Random.Range(min_dist * min_dist, max_dist * max_dist));
            var vec = Random.Range(0f, 360f);
            var pos = new Vector2((transform.position.x + dist) * Mathf.Sin(Mathf.Deg2Rad * vec), transform.position.y + (dist * Mathf.Cos(Mathf.Deg2Rad * vec)));
            var a = Instantiate(asteroid, pos, Quaternion.Euler(0, 0, Random.Range(0f, 360f)), transform);
            a.transform.localScale = new Vector3(scalx, scalx);

        
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
