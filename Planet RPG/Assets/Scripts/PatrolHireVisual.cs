using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatrolHireVisual : MonoBehaviour
{
    public GameObject imageObj, patrolObj, imageObjToCreate;
    // Start is called before the first frame update
    void Start()
    {
        //var children = patrolObj.transform.
        for (int i = 0; i < patrolObj.transform.childCount; i++)
        {
            if (patrolObj.transform.GetChild(i).GetComponent<SpriteRenderer>() != null)
            {
                var childPos = patrolObj.transform.GetChild(i).transform.localPosition;
                var xMult = 19.83873032f;
                var yMult = 39.67492161f;
                var a = Instantiate(imageObjToCreate, GetComponent<RectTransform>().transform.position, Quaternion.identity, imageObj.transform);
                a.GetComponent<RectTransform>().anchoredPosition = new Vector2(childPos.x * xMult, childPos.y * yMult);
                if (patrolObj.transform.GetChild(i).transform.localScale.x < 1) a.GetComponent<RectTransform>().transform.localScale *= new Vector2(-1, 1);
                a.GetComponent<Image>().sprite = patrolObj.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
