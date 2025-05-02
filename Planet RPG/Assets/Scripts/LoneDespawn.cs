using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LoneDespawn : MonoBehaviour
{
    public GameObject linkedObj;
    private Transform playerPos;
    public float dist;
    private float distTime;
    private bool far;
    // Start is called before the first frame update
    void Start()
    {
        playerPos = HUDmanage.playerReference.transform;
    }

    // Update is called once per frame
    void Update()
    {
        distTime -= Time.deltaTime;
        if (distTime <= 0)
        {
            if (far && Vector2.Distance(transform.position, playerPos.position) < dist)
            {
                linkedObj.SetActive(true);
                far = false;
            }
            else if (!far && Vector2.Distance(transform.position, playerPos.position) > dist)
            {
                linkedObj.SetActive(false);
                far = true;
            }
            distTime = Random.Range(.5f, 1f);
        }
    }
}
