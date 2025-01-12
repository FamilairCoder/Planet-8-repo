using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SettingsToggle : MonoBehaviour
{
    public Color on, off;
    private GameObject indicator;
    public bool forConcreteMovement;
    private bool concreteMovement;
    // Start is called before the first frame update
    void Start()
    {
        indicator = transform.GetChild(0).gameObject;
        if (PlayerPrefs.GetInt("ConcreteMovement", 0) == 1)
        {
            concreteMovement = true;
            indicator.GetComponent<SpriteRenderer>().color = on;
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
                    indicator.GetComponent<SpriteRenderer>().color = off;
                    PlayerPrefs.SetInt("ConcreteMovement", 0);
                    PlayerPrefs.Save();
                }
                else
                {
                    concreteMovement = true;
                    indicator.GetComponent<SpriteRenderer>().color = on;
                    PlayerPrefs.SetInt("ConcreteMovement", 1);
                    PlayerPrefs.Save();
                }
            }

        }
    }
}
