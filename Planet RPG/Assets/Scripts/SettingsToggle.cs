using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SettingsToggle : MonoBehaviour
{
    public Sprite on, off;
    private GameObject indicator;
    public bool forConcreteMovement;
    private bool concreteMovement;
    // Start is called before the first frame update
    void Start()
    {
        indicator = transform.GetChild(0).gameObject;
        if (SaveManager.GetInt("ConcreteMovement", 0) == 1)
        {
            concreteMovement = true;
            indicator.GetComponent<SpriteRenderer>().sprite = on;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && GetComponent<Collider2D>().OverlapPoint(mousePos))
        {
            if (forConcreteMovement)
            {
                if (concreteMovement)
                {
                    concreteMovement = false;
                    indicator.GetComponent<SpriteRenderer>().sprite = off;
                    SaveManager.SetInt("ConcreteMovement", 0);
                    
                }
                else
                {
                    concreteMovement = true;
                    indicator.GetComponent<SpriteRenderer>().sprite = on;
                    SaveManager.SetInt("ConcreteMovement", 1);
                    
                }
            }

        }
    }
}
