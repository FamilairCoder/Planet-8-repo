using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsSlide : MonoBehaviour
{
    private bool down;
    public bool isMasterVolume, isSongVolume, isSFXVolume;
    // Start is called before the first frame update
    void Start()
    {
        var x = 0f;
        if (isMasterVolume)
        {
            x = Mathf.Lerp(-7.5f, 7.5f, PlayerPrefs.GetFloat("MasterVolume", .5f) / 1);
        }
        else if (isSongVolume)
        {
            x = Mathf.Lerp(-7.5f, 7.5f, PlayerPrefs.GetFloat("SongVolume", .5f) / 1);
        }
        else if (isSFXVolume)
        {
            x = Mathf.Lerp(-7.5f, 7.5f, PlayerPrefs.GetFloat("SFXVolume", .5f) / 1);
        }
        transform.position = new Vector2(x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (down)
        {
            var campos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(Mathf.Clamp(campos.x, -7.5f, 7.5f), transform.position.y, 0);

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
}
