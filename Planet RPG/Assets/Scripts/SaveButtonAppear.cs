using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveButtonAppear : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SaveSelect.selected) { GetComponent<SpriteRenderer>().enabled = true; GetComponent<BoxCollider2D>().enabled = true; GetComponentInChildren<TextMeshPro>().enabled = true; }
        else { GetComponent<SpriteRenderer>().enabled = false; GetComponent<BoxCollider2D>().enabled = false; GetComponentInChildren<TextMeshPro>().enabled = false; }
    }
}
