using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorStayOn : MonoBehaviour
{
    public bool isFocusFire, isHoldPosition;
    private bool choseTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFocusFire)
        {
            if (PatrolManager.focusTarget == null)
            {
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = new Vector3(mousePos.x, mousePos.y, 0);
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    PatrolManager.focusFire = false;
                    PatrolManager.createdFocus = false;
                    Destroy(gameObject);
                }
            }
            else
            {
                choseTarget = true;
                var pos = PatrolManager.focusTarget.transform.position;
                transform.position = pos;
            }

            if (PatrolManager.focusTarget == null && choseTarget)
            {
                PatrolManager.focusFire = false;
                PatrolManager.createdFocus = false;
                Destroy(gameObject);
            }
        }


    }
}
