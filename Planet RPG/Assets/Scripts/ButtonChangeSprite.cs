using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChangeSprite : MonoBehaviour
{
    public Sprite onSprite, offSprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        GetComponent<SpriteRenderer>().sprite = onSprite;
    }
    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().sprite = offSprite;
    }
}
