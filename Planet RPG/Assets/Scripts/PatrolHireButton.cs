using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PatrolHireButton : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI costText, hireText;
    private MenuScript menuScript;
    public bool hired;
    // Start is called before the first frame update
    void Start()
    {
        menuScript = transform.parent.GetComponent<MenuScript>();
    }

    // Update is called once per frame
    void Update()
    {
        var cost = menuScript.hireCost;
        if (HUDmanage.money < cost) { costText.color = Color.red; hireText.color = Color.red; }
        else { costText.color = new Color(0, 0.7264151f, 0.09843943f); hireText.color = new Color(0, 0.7264151f, 0.09843943f); }
        costText.text = cost.ToString() + "P";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
        if (HUDmanage.money >= menuScript.hireCost)
        {
            //Debug.Log("clicked");
            transform.parent.GetComponent<PatrolHireVisual>().patrolObj.GetComponent<PatrolID>().Hire();
            HUDmanage.money -= menuScript.hireCost;
            hired = true;
            //Destroy(transform.parent);
        }

        //Debug.Log(PlayerPrefs.GetInt(key + "taken"));
    }
}
