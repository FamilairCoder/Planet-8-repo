using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMenuOpen : MonoBehaviour
{
    private bool active;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PatrolManager.patrols.Count == 0)
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(2111, 0);
        }
        else
        {
            if (!active)
            {
                GetComponent<RectTransform>().anchoredPosition = new Vector2(1111, 0);
            }
            else
            {
                GetComponent<RectTransform>().anchoredPosition = new Vector2(808, 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (active) active = false;
            else active = true;
        }
    }
}
