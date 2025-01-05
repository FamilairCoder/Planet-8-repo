using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class AppearTextOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (var t in texts)
        {
            t.enabled = true;
        }

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (var t in texts)
        {
            t.enabled = false;
        }

    }


}
