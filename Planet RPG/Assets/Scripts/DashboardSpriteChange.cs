using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DashboardSpriteChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite onSprite, offSprite;
    public bool concreteM, boost, danger, stop;
    public string onText, offText;
    [Header("Indicator specific-----------------------")]
    public Color lowDanger;
    public Color highDanger;
    public string dangerText;

    public Sprite inhibitedSprite, bashSprite;
    public string inhibitedText, bashText;

    private PlayerMovement moveScript;
    private Image img;
    private bool over;
    // Start is called before the first frame update
    void Start()
    {
        moveScript = HUDmanage.playerReference.GetComponent<PlayerMovement>();
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (concreteM)
        {
            if (moveScript.concreteMovement) img.sprite = onSprite;
            else img.sprite = offSprite;
        }
        else if (boost)
        {
            if (!moveScript.inhibited)
            {
                img.sprite = offSprite;
                if (Input.GetKey(KeyCode.LeftShift)) img.sprite = onSprite;
                if (Input.GetKeyUp(KeyCode.LeftShift)) img.sprite = offSprite;
            }
            else img.sprite = inhibitedSprite;
            if (PlayerBash.bash) img.sprite = bashSprite;
        }
        else if (danger)
        {
            img.color = Color.Lerp(lowDanger, highDanger, ThingSpawner.totalDeliveries / 15f);
        }
        else if (stop)
        {
            img.sprite = offSprite;
            if (Input.GetKey(KeyCode.X)) img.sprite = onSprite;
            if (Input.GetKeyUp(KeyCode.X)) img.sprite = offSprite;
        }
        if (over)
        {
            if (concreteM)
            {
                if (moveScript.concreteMovement) DashboardTextScript.dashText.text = onText;
                else DashboardTextScript.dashText.text = offText;
            }
            else if (boost)
            { 

                if (!moveScript.inhibited)
                {
                    DashboardTextScript.dashText.text = offText;
                    if (Input.GetKey(KeyCode.LeftShift)) DashboardTextScript.dashText.text = onText;
                    if (Input.GetKeyUp(KeyCode.LeftShift)) DashboardTextScript.dashText.text = offText;
                }
                else DashboardTextScript.dashText.text = inhibitedText;
                if (PlayerBash.bash) DashboardTextScript.dashText.text = bashText;
            }
            else if (danger)
            {
                DashboardTextScript.dashText.text = dangerText + ThingSpawner.totalDeliveries.ToString();
            }
            else if (stop)
            {
                DashboardTextScript.dashText.text = offText;
                if (Input.GetKey(KeyCode.X)) DashboardTextScript.dashText.text = onText;
                if (Input.GetKeyUp(KeyCode.X)) DashboardTextScript.dashText.text = offText;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        over = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        over = false;
        DashboardTextScript.dashText.text = "";
        

    }
}
