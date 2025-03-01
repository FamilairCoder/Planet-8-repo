using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class FMButtonScript : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image bg;
    public string name;
    public bool focusFire, holdFire, holdPosition, stayDistance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (holdFire && !PatrolManager.holdFire) bg.enabled = false;
        if (focusFire && !PatrolManager.focusFire) bg.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (focusFire)
        {
            if (PatrolManager.focusFire)
            {
                Destroy(PatrolManager.createdIndicator);
                PatrolManager.focusFire = false;
                PatrolManager.createdFocus = false;
                PatrolManager.focusTarget = null;
                bg.enabled = false;
            }
            else
            {
                PatrolManager.focusFire = true;
                bg.enabled = true;
            }
            
        }
        else if (holdFire)
        {
            SwitchSetting(ref PatrolManager.holdFire);
            PatrolManager.focusFire = false;
        }
        else if (holdPosition)
        {
            SwitchSetting(ref PatrolManager.holdPosition);
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        FMNameTextScript.name = name;

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        FMNameTextScript.name = "";

    }

    void SwitchSetting(ref bool value)
    {

        if (value)
        {
            value = false;
            bg.enabled = false;
        }
        else
        {
            value = true;
            bg.enabled = true;
        }
        
    }
}
