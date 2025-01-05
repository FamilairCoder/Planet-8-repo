using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawn : MonoBehaviour
{
    private Transform playerPos;
    // Start is called before the first frame update
    void Start()
    {
        playerPos = FindObjectOfType<PlayerMovement>().transform;

        StartCoroutine(check());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator check()
    {
       
        if (Vector2.Distance(playerPos.position, transform.position) > 500)
        {
            Destroy(gameObject);
        }
        yield return new WaitForSeconds(5);
        StartCoroutine(check());
    }
}
