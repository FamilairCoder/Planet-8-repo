using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool shown;
    public List<GameObject> show = new List<GameObject>();
    public List<GameObject> related_buttons = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!shown) Hide();
        //if (GetComponent<SetBounty>() != null) related_buttons = GetComponent<SetBounty>().bounties;''
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!shown)
        {
            shown = true;
            Show();            
        }


    }

    public void Show()
    {
        foreach (GameObject s in show)
        {
            if (s != null)
            {
                s.SetActive(true);
            }
            
        }
        foreach (GameObject rb in related_buttons)
        {
            //rb.GetComponent<MenuButton>().Hide();
            rb.GetComponent<MenuButton>().shown = false;
        }
    }
    public void Hide()
    {
        foreach (GameObject s in show)
        {
            if (s != null)
            {
                s.SetActive(false);
            }
            else
            {
                show.Remove(s);
            }
            
        }
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
