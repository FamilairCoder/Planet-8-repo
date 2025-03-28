using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMenuOpen : MonoBehaviour
{
    public static bool active;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PatrolManager.patrols.Count == 0)
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(2111, 35);
        }
        else
        {
            if (!active)
            {
                GetComponent<RectTransform>().anchoredPosition = new Vector2(1111, 35);
            }
            else
            {
                GetComponent<RectTransform>().anchoredPosition = new Vector2(808, 35);
            }
        }
        if (Input.GetKeyDown(KeyCode.V) && !TextInputScript.typing)
        {
            if (active) active = false;
            else active = true;
        }
    }
}
