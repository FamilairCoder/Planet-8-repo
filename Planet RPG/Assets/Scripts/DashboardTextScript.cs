using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DashboardTextScript : MonoBehaviour
{
    public static TextMeshProUGUI dashText;
    private RectTransform canvasRect;
    // Start is called before the first frame update
    void Start()
    {
        dashText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 localPoint;
        

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, null, out localPoint))
        {
            GetComponent<RectTransform>().anchoredPosition = localPoint;
        }

    }

}
