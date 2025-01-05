using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundStarSpawn : MonoBehaviour
{
    public GameObject bg_star, player;
    public float amount;
    public bool isMenu;
    // Start is called before the first frame update
    void Start()
    {
        if (!isMenu)
        {
            for (int i = 0; i < amount; i++)
            {
                var pos = new Vector3(Random.Range(-160f, 160f), Random.Range(-90f, 90f), 0);
                var a = Instantiate(bg_star, pos, Quaternion.identity, gameObject.transform);
                a.GetComponent<ParallaxMovement>().playerTransform = player.transform;
            }
        }
        else
        {
            for (int i = 0; i < amount; i++)
            {
                var pos = new Vector3(Random.Range(-50f, 50f), Random.Range(-30f, 30f), 0);
                Instantiate(bg_star, pos, Quaternion.identity, gameObject.transform);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
