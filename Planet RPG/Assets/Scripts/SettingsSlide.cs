using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsSlide : MonoBehaviour
{
    public Transform linkedBar;
    private float origPos;
    private bool down;
    public bool isMasterVolume, isSongVolume, isSFXVolume, isBloomIntensity, isBloomThreshold;
    // Start is called before the first frame update
    void Start()
    {
        origPos = linkedBar.position.x;
        var x = 0f;
        if (isMasterVolume)
        {
            x = GetPrefs("MasterVolume", 1, .5f);
        }
        else if (isSongVolume)
        {
            x = GetPrefs("SongVolume", 1, .5f);
        }
        else if (isSFXVolume)
        {
            x = GetPrefs("SFXVolume", 1, 1f);
        }
        else if (isBloomIntensity)
        {
            x = GetPrefs("bloom intensity", 2, .5f);
        }
        else if (isBloomThreshold)
        {
            x = GetPrefs("bloom threshold", 1, .7f);
        }
        transform.position = new Vector2(origPos + x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (down)
        {
            var barPosX = linkedBar.position.x;
            var campos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(Mathf.Clamp(campos.x, barPosX - 7.5f, barPosX + 7.5f), transform.position.y, 0);

/*            if (isMasterVolume)
            {
                SetPrefs("MasterVolume", 1);
            }
            else if (isSongVolume)
            {
                SetPrefs("SognVolume", 1);
            }
            else if (isSFXVolume)
            {
                SetPrefs("SFXVolume", 1);
            }
            else if (isBloomIntensity)
            {
                SetPrefs("bloom intensity", 2);
            }
            else if (isBloomThreshold)
            {
                SetPrefs("bloom threshold", 1);
            }*/
        }

    }

    private void OnMouseDown()
    {
        down = true; 

    }

    private void OnMouseUp()
    {
        down = false;
    }


    float GetPrefs(string key, float max, float defaultValue)
    {

        return Mathf.Lerp(-7.5f, 7.5f, PlayerPrefs.GetFloat(key, defaultValue) / max);

    }
    void SetPrefs(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }
}
