using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecordReference : MonoBehaviour
{
    public string firstText, secondText;
    public Sprite image;
    // Start is called before the first frame update
    void Start()
    {
        firstText = transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        secondText = transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;
        image = transform.GetChild(2).GetComponent<Image>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
