using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UnlockParts : MonoBehaviour, IPointerClickHandler
{
    public GameObject text, lockImg, BG;
    public Color buyColor, cantColor;
    public float cost;
    private bool bought, looped;
    public string key;
    public List<CircuitSlotScript> slots = new List<CircuitSlotScript>();
    // Start is called before the first frame update
    void Start()
    {
        if (SaveManager.GetInt(key + transform.GetSiblingIndex() + "bought", 0) == 1)
        {
            HUDmanage.lvl++;
            bought = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!bought)
        {
            if (HUDmanage.money < cost) text.GetComponent<TextMeshProUGUI>().color = cantColor;
            else text.GetComponent<TextMeshProUGUI>().color = buyColor;

            if (SaveManager.GetInt(key + transform.GetSiblingIndex() + "bought", 0) == 1)
            {
                HUDmanage.lvl++;
                bought = true;
            }
        }
        else
        {
            text.GetComponent<TextMeshProUGUI>().enabled = false;
            lockImg.GetComponent<Image>().enabled = false;
            BG.GetComponent<Image>().enabled = false;
            if (!looped)
            {
                foreach (var slot in slots)
                {
                    slot.enabled = true;
                }
                looped = true;
            }
        }
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!bought && HUDmanage.money >= cost)
        {
            HUDmanage.lvl++;
            HUDmanage.money -= cost;
            bought = true;
            SaveManager.SetInt(key + transform.GetSiblingIndex() + "bought", 1);
            
        }
    }


}
