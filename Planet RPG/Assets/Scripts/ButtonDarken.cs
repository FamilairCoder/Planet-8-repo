using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonDarken : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
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
        
        if (GetComponent<Image>() != null) GetComponent<Image>().color = new Color(.5f, .5f, .5f);

    }
    public void OnPointerExit(PointerEventData eventData)
    {


        if (GetComponent<Image>() != null) GetComponent<Image>().color = new Color(1f, 1f, 1f);

    }
}
