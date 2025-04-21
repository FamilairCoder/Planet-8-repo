using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Collections.Specialized.BitVector32;

public class RecordTextManager : MonoBehaviour
{
    public List<RecordReference> records = new List<RecordReference>();
    public static int index = 0, recordAmount;
    public static bool active;
    // Start is called before the first frame update
    void Awake()
    {
        recordAmount = records.Count;
    }

    // Update is called once per frame
    void Update()
    {
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = records[index].firstText;
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = records[index].secondText;
        transform.GetChild(2).GetComponent<Image>().sprite = records[index].image;
        if (active && Input.GetKeyDown(KeyCode.Escape) || HUDmanage.on_map)
        {
            active = false;
            OpenMenu.opened = false;
            HUDmanage.DONT = .3f;
        }
        if (active) GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        else GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -5000f);
    }
}
