using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTow : MonoBehaviour
{
    public static List<GameObject> towObjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var i = 0;
        foreach (GameObject obj in towObjects)
        {
            if (obj == null) { towObjects.Remove(obj); continue; }
            if (i == 0) 
            {
                obj.transform.position = Vector2.Lerp(obj.transform.position, transform.position + -transform.up * 2, .2f);

                var dir = (transform.position - obj.transform.position).normalized;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                obj.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
            }
            else
            {
                obj.transform.position = Vector2.Lerp(obj.transform.position, towObjects[i - 1].transform.position + -(towObjects[i - 1].transform.up * 1.5f), .2f);

                var dir = (towObjects[i - 1].transform.position - obj.transform.position).normalized;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                obj.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
            } 
                
                
            i++;
        }
    }
}
