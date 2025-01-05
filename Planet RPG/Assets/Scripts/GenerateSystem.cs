using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateSystem : MonoBehaviour
{
    public GameObject planet;
    // Start is called before the first frame update
    void Start()
    {
        int planet_numb = Random.Range(5, 10);
        for (int i = 0; i < planet_numb; i++)
        {
            var dist = Random.Range(20f, 200f);
            var vec = Random.Range(0, 360);
            var pos = new Vector2(transform.position.x + (dist * Mathf.Sin(Mathf.Deg2Rad * vec)), transform.position.y + (dist * Mathf.Cos(Mathf.Deg2Rad * vec)));

            var rot = Quaternion.Euler(0, 0, Random.Range(-100f, 100f));
            var p = Instantiate(planet, pos, rot);
            var scal = Random.Range(1f, 4f);
            p.transform.localScale = new Vector2 (scal, scal);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
