using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsNumb : MonoBehaviour
{
    public Transform linkedSlide, linkedBar;
    private float linkedSlidePosx, leftPosx;
    public bool isMasterVolume, isSongVolume, isSFXVolume, isBloomIntensity, isBloomThreshold;
    // Start is called before the first frame update
    void Start()
    {
        linkedSlidePosx = linkedSlide.position.x;
        leftPosx = linkedSlide.position.x + 15;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMasterVolume)
        {
            AudioScript.masterVolume = CalculateLerp(0, 1);
            GetComponentInChildren<TextMeshPro>().text = Mathf.Round(AudioScript.masterVolume * 100).ToString() + "%";
        }
        else if (isSongVolume)
        {
            AudioScript.songVolume = CalculateLerp(0, 1);
            GetComponentInChildren<TextMeshPro>().text = Mathf.Round(AudioScript.songVolume * 100).ToString() + "%";
        }
        else if (isSFXVolume)
        {
            AudioScript.SFXVolume = CalculateLerp(0, 1);
            GetComponentInChildren<TextMeshPro>().text = Mathf.Round(AudioScript.SFXVolume * 100).ToString() + "%";
        }


        else if (isBloomIntensity)
        {
            PlayerPrefs.SetFloat("bloom intensity", CalculateLerp(0, 2));
            GetComponentInChildren<TextMeshPro>().text = Mathf.Round(PlayerPrefs.GetFloat("bloom intensity") * 50).ToString() + "%";
        }   
        else if (isBloomThreshold)
        {
            PlayerPrefs.SetFloat("bloom threshold", CalculateLerp(0, 1));
            GetComponentInChildren<TextMeshPro>().text = Mathf.Round(PlayerPrefs.GetFloat("bloom threshold") * 100).ToString() + "%";
        }

        PlayerPrefs.Save();
    }



    float CalculateLerp(float min, float max)
    {
        var relativePos = linkedSlide.position.x - linkedBar.position.x;
        //if (relativePos > 0) Debug.Log(relativePos);
        return Mathf.Lerp(min, max, (relativePos + 7.5f) / 15);
    }
}
