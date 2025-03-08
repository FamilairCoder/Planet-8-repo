using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomUIImage : MonoBehaviour
{
    public List<Sprite> images = new List<Sprite>();
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().sprite = images[Random.Range(0, images.Count)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
