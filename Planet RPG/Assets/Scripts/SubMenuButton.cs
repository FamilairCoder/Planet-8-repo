using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SubMenuButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool shown;
    private bool active;
    public GameObject mainButton;
    public List<GameObject> show = new List<GameObject>();
    public List<GameObject> related_buttons = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (!shown) Hide();
        if (!mainButton.GetComponent<MenuButton>().shown)
        {
            Hide();
            GetComponent<Image>().enabled = false;
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = false;
        }
        else
        {
            if (shown) Show();
            GetComponent<Image>().enabled = true;
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = true;
        }
        //if (GetComponent<SetBounty>() != null) related_buttons = GetComponent<SetBounty>().bounties;''
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (mainButton.GetComponent<MenuButton>().shown)
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
            rb.GetComponent<SubMenuButton>().shown = false;
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
