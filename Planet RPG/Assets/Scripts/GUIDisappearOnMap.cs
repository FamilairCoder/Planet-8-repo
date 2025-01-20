using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIDisappearOnMap : MonoBehaviour
{
    private bool a;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (HUDmanage.on_map && a)
        {
            if (GetComponent<Image>() != null) GetComponent<Image>().enabled = false;
            else if (GetComponent<TextMeshProUGUI>() != null) GetComponent<TextMeshProUGUI>().enabled = false;
            a = false;
        }
        else if (!HUDmanage.on_map && !a)
        {
            if (GetComponent<Image>() != null) GetComponent<Image>().enabled = true;
            else if (GetComponent<TextMeshProUGUI>() != null) GetComponent<TextMeshProUGUI>().enabled = true;
            a = true;
        }
    }
}
