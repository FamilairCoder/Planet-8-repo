using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CVSpriteChange : MonoBehaviour
{
    public Sprite lvl2, lvl3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (HUDmanage.lvl == 2) GetComponent<Image>().sprite = lvl2;
        if (HUDmanage.lvl == 3) GetComponent<Image>().sprite = lvl3;
    }
}
