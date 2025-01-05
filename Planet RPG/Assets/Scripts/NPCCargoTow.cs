using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCargoTow : MonoBehaviour
{
    public Transform followObj;
    private Vector3 targetPos;
    private float offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = Vector2.Distance(transform.position, targetPos);
    }

    // Update is called once per frame
    void Update()
    {
        if (followObj != null)
        {
            targetPos = followObj.position + followObj.up * -5;
            transform.position = Vector3.Lerp(transform.position, targetPos, .4f);

            var dir = (followObj.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);

            transform.position = new Vector3(transform.position.x, transform.position.y, .3f);
        }
        else { Destroy(gameObject); }
    }
}
