using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColor : MonoBehaviour
{
    public List<Color> colors = new List<Color>();
    public bool pickRandom;
    public RandomColor sameas;
    // Start is called before the first frame update
    void Start()
    {
        if (!pickRandom) GetComponent<SpriteRenderer>().color = colors[Random.Range(0, colors.Count)];
        else GetComponent<SpriteRenderer>().color = Random.ColorHSV(0f, 1f, 1, 1, 1, 1);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (sameas != null) GetComponent<SpriteRenderer>().color = sameas.GetComponent<SpriteRenderer>().color;
    }
}
