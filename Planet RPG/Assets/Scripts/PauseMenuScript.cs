using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour
{
    //private Image imgComponent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (HUDmanage.pauseMenu)
        {
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
        else
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(-5000, 2345);
        }
    }
}
