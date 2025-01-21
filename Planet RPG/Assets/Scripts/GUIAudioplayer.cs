using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIAudioplayer : MonoBehaviour
{
    public static AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (clip != null)
        {
            GetComponent<AudioSource>().clip = clip;
            GetComponent<AudioSource>().Play();
            
            clip = null;
        }
    }
}
