using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutpostSetBody : MonoBehaviour
{
    public GameObject leg;
    private string key;
    // Start is called before the first frame update
    void Start()
    {
        key = gameObject.name; 
        var amount = PlayerPrefs.GetFloat(key + "amount", Random.Range(1, 10));
        var radius = PlayerPrefs.GetFloat(key + "radius", Random.Range(1f, 3f));
        //var radius = Random.Range(transform.localScale.x * .1f, transform.localScale.x * .9f);
        //var radius = Random.Range(1f, 3f);
        for (float i = 0; i < amount; i++)
        {
            var currentpos = transform.position;
            var vec = Mathf.Lerp(0, 360, i / amount);
            var pos = new Vector3(currentpos.x + (radius * Mathf.Sin(Mathf.Deg2Rad * vec)), currentpos.y + (radius * Mathf.Cos(Mathf.Deg2Rad * vec)));
            
            var dir = (pos - transform.position).normalized;
            float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, targetAngle));

            var l = Instantiate(leg, pos, targetRotation, transform);
            
        }
        PlayerPrefs.SetFloat(key + "amount", amount);
        PlayerPrefs.SetFloat(key + "radius", radius);
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
