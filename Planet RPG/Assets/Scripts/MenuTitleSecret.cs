using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class MenuTitleSecret : MonoBehaviour
{
    public GameObject bullet;
    public List<GameObject> pirates = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && HitTitle())
        {

            var amount = Random.Range(0f, 20f);
            for (int i = 0; i < amount; i++)
            {
                var b = Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
                var scal = Random.Range(1f, 4f);
                b.transform.localScale = new Vector3(scal, scal);
            }
        }
        if (Input.GetMouseButtonDown(1) && HitTitle())
        {
            var amount = Random.Range(1f, 10f);
            for (int i = 0; i < amount; i++)
            {
                var offset = new Vector3(Random.Range(-30f, 30f), Random.Range(-2f, 2f));
                var b = Instantiate(pirates[Random.Range(0, pirates.Count)], transform.position + offset, Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
            }
        }
    }

    bool HitTitle()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return GetComponent<Collider2D>().OverlapPoint(mousePos);
    }


}
