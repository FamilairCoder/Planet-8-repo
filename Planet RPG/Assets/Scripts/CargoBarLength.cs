using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CargoBarLength : MonoBehaviour
{
    public Color green, red;
    private Color target_col;
    private float target_y, y;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerMining.cargo_amount / PlayerMining.cargo_capacity > .6f)
        {
            target_col = Color.Lerp(green, red, ((PlayerMining.cargo_amount / PlayerMining.cargo_capacity) - .6f) / .4f);        }
        else
        {
            target_col = green;
        }
        target_y = Mathf.Lerp(0, .89f, PlayerMining.cargo_amount / PlayerMining.cargo_capacity);
        

        transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(transform.GetChild(0).GetComponent<Image>().color, target_col, .2f);
        y = Mathf.Lerp(y, target_y, .2f);
        transform.localScale = new Vector3(.75f, y);
    }
}
