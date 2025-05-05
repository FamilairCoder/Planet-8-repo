using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneLoadScreen : MonoBehaviour
{
    private float timeleft = 1;
    public Image stationImg, shipImg;
    private RectTransform stationRect;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        GetComponent<Image>().sprite = LoadingSceneManager.spr;
        stationImg.sprite = LoadingSceneManager.stationSpr;
        shipImg.sprite = LoadingSceneManager.shipSpr;

        stationImg.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, stationImg.gameObject.GetComponent<RectTransform>().anchoredPosition.y);
        stationRect = stationImg.gameObject.GetComponent<RectTransform>();
        stationRect.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
    }

    // Update is called once per frame
    void Update()
    {
        timeleft -= Time.deltaTime;
        if (timeleft < 0)
        {
            GetComponent<Image>().color -= new Color(0, 0, 0, .5f) * Time.deltaTime;
            stationImg.color -= new Color(0, 0, 0, .5f) * Time.deltaTime;
            shipImg.color -= new Color(0, 0, 0, .5f) * Time.deltaTime;
            if (GetComponent<Image>().color.a <= 0) { Destroy(gameObject); Destroy(stationRect.gameObject); }
        }
    }

    void FixedUpdate()
    {
        stationRect.transform.rotation *= Quaternion.Euler(0, 0, -40 * Time.deltaTime);
    }
}
