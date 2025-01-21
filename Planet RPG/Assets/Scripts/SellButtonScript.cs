using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SellButtonScript : MonoBehaviour, IPointerClickHandler
{
    public Image highlight;
    public TextMeshProUGUI text;
    public static bool sell;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        highlight.gameObject.SetActive(sell);
        text.gameObject.SetActive(sell);
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (!sell) { sell = true; }
        else { sell = false; }

    }
}
