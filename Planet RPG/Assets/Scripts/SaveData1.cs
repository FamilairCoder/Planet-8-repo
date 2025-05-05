using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData1 : MonoBehaviour
{
    private float timeleft = 60;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeleft -= Time.deltaTime;
        if (timeleft < 0)
        {
            PlayerPrefs.Save();
            timeleft = 60;
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
