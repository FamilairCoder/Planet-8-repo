using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomText : MonoBehaviour
{
    public List<string> texts = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<TextMeshProUGUI>() != null) GetComponent<TextMeshProUGUI>().text = texts[Random.Range(0, texts.Count)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
