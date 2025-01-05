using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsNumb : MonoBehaviour
{
    public Transform linkedSlide;
    public bool isMasterVolume, isSongVolume, isSFXVolume;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMasterVolume)
        {
            AudioScript.masterVolume = Mathf.Lerp(0, 1, (linkedSlide.position.x + 7.5f) / 15f);
            GetComponentInChildren<TextMeshPro>().text = Mathf.Round(AudioScript.masterVolume * 100).ToString() + "%";
        }
        else if (isSongVolume)
        {
            AudioScript.songVolume = Mathf.Lerp(0, 1, (linkedSlide.position.x + 7.5f) / 15f);
            GetComponentInChildren<TextMeshPro>().text = Mathf.Round(AudioScript.songVolume * 100).ToString() + "%";
        }
        else if (isSFXVolume)
        {
            AudioScript.SFXVolume = Mathf.Lerp(0, 1, (linkedSlide.position.x + 7.5f) / 15f);
            GetComponentInChildren<TextMeshPro>().text = Mathf.Round(AudioScript.SFXVolume * 100).ToString() + "%";
        }
    }
}
