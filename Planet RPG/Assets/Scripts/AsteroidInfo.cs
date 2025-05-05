using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidInfo : MonoBehaviour
{
    public bool has_ore, copper, iron, gold, has_detected, has_probes;
    public List<Transform> probeRotations = new List<Transform>();
    public string key;
    private float setTime;
    public AsteroidSpawner spawner;
    private bool isQuitting = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        setTime -= Time.deltaTime;
        if (setTime < 0 && has_ore)
        {
            SaveManager.SetFloat(key + "x", transform.localScale.x);
            SaveManager.SetFloat(key + "y", transform.localScale.y);
            
            setTime = 1f;
        }
    }
    void OnApplicationQuit()
    {
        // This will be set to true when the application is closing
        isQuitting = true;
    }

    void OnDestroy()
    {
        // This will only run if the object is destroyed during gameplay, not on application quit
        if (!isQuitting)
        {
            SaveManager.SetInt(key + "alive", 0);
        }
    }

}
