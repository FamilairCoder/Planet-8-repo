using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveOutline : MonoBehaviour
{
    public int wantedID;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("Save File ID") == wantedID) GetComponent<SpriteRenderer>().enabled = true;
        else GetComponent<SpriteRenderer>().enabled = false;
    }
}
