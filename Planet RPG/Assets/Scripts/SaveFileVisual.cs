using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveFileVisual : MonoBehaviour
{

    public bool isShip, isMoney, isSecondary, isWeapon;
    private string saveID;
    [Header("Visual specific stuf----------")]
    public Sprite lvl1Img;
    public Sprite lvl2Img, lvl3Img;
    private int index;
    public bool basicLaser, laserRod, laserBeam;
    // Start is called before the first frame update
    void Start()
    {
        saveID = "Save" + GetComponentInParent<SaveSelect>().saveId;
        if (isShip)
        {
            if (PlayerPrefs.GetFloat(saveID + "lvl") == 1)
            {
                GetComponent<SpriteRenderer>().sprite = lvl1Img;
            }
            else if (PlayerPrefs.GetFloat(saveID + "lvl") == 2)
            {
                GetComponent<SpriteRenderer>().sprite = lvl2Img;
            }
            else if (PlayerPrefs.GetFloat(saveID + "lvl") == 3)
            {
                GetComponent<SpriteRenderer>().sprite = lvl3Img;
            }
        }
        else if (isMoney)
        {
            GetComponent<TextMeshPro>().text = PlayerPrefs.GetFloat(saveID + "money").ToString() + "P";
        }
        else if (isSecondary)
        {
            var list = SaveSelect.secondaries;
            for(int i = 0; i < list.Count; i++)
            {
                if (list[i].name == gameObject.name) index = i;
            }
            if (PlayerPrefs.GetInt(saveID + "part " + index + " bought") == 1) GetComponent<SpriteRenderer>().color = Color.white;
            else GetComponent<SpriteRenderer>().color = Color.black;
        }
        else if (isWeapon)
        {
            if (basicLaser)
            {
                if (PlayerPrefs.GetFloat(saveID + "basic laser") > 0) GetComponent<SpriteRenderer>().color = Color.white;
                else GetComponent<SpriteRenderer>().color = Color.black;
            }
            else if (laserRod)
            {
                if (PlayerPrefs.GetFloat(saveID + "laser rod") > 0) GetComponent<SpriteRenderer>().color = Color.white;
                else GetComponent<SpriteRenderer>().color = Color.black;
            }
            else if (laserBeam)
            {
                if (PlayerPrefs.GetFloat(saveID + "laser beam") > 0) GetComponent<SpriteRenderer>().color = Color.white;
                else GetComponent<SpriteRenderer>().color = Color.black;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
