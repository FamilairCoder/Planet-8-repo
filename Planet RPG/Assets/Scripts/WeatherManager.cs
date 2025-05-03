using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    public ParticleSystem meteorShower;
    private float weatherTime;
    // Start is called before the first frame update
    void Start()
    {
        weatherTime = PlayerPrefs.GetFloat("meteor time", 0);
    }

    // Update is called once per frame
    void Update()
    {
        weatherTime -= Time.deltaTime;
        if (weatherTime < 0)
        {
            var chance = Random.Range(0f, 1f);
            if (chance < .2f)
            {
                meteorShower.Play();
            }
            weatherTime = 300;
        }
        PlayerPrefs.SetFloat("meteor time", weatherTime);
    }
}
