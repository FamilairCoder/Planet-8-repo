using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public static float songVolume = 1, masterVolume = .5f, SFXVolume = .5f;
    public bool isManager, forMenu, dontPitchShift;
    private bool changing, traveling, isLaser;
    private float changeTimeleft, checkTime, fade = 1, saveTime;
    private Collider2D[] stationNumb, pirateNumb;
    public GameObject player;
    public AudioClip civilSpace, travel, fighting, asteroidField, shipGraveyard, delivery;
    private AudioClip playNext;
    private AudioSource AudioSource;    
    // Start is called before the first frame update
    private void Awake()
    {
        if (isManager || forMenu)
        {
            GetComponent<AudioSource>().volume = songVolume * masterVolume * fade;
        }
        else
        {
            GetComponent<AudioSource>().volume = SFXVolume * masterVolume;
            if (!dontPitchShift) GetComponent<AudioSource>().pitch = Random.Range(.5f, 1.5f);
        }

        songVolume = PlayerPrefs.GetFloat("SongVolume", 1);
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", .5f);
        SFXVolume = PlayerPrefs.GetFloat("SFXVolume", .5f);
    }
    void Start()
    {
        AudioSource = GetComponent<AudioSource>();

        AudioSource.maxDistance = 200;
        //if (soundToPlay != null ) {AudioSource.clip = soundToPlay;}
    }

    // Update is called once per frame
    void Update()
    {
        saveTime -= Time.deltaTime;
        if (saveTime < 0)
        {
            PlayerPrefs.SetFloat("SongVolume", songVolume);
            PlayerPrefs.SetFloat("MasterVolume", masterVolume);
            PlayerPrefs.SetFloat("SFXVolume", SFXVolume);
            PlayerPrefs.Save();

            saveTime = 1f;
        }


        if (isManager)
        {
            GetComponent<AudioSource>().volume = songVolume * masterVolume * fade;
            if (!changing)
            {
                if (changeTimeleft < 1) changeTimeleft += Time.deltaTime;
                fade = Mathf.Lerp(0, 1, changeTimeleft / 2);
                checkTime -= Time.deltaTime;
                if (checkTime < 0)
                {
                    var prev = playNext;
                    stationNumb = Physics2D.OverlapCircleAll(player.transform.position, 100, LayerMask.GetMask("station"));
                    pirateNumb = Physics2D.OverlapCircleAll(player.transform.position, 100, LayerMask.GetMask("pirates"));

                    if (pirateNumb.Length > 0)
                    {
                        playNext = fighting;
                    }
                    else if (AsteroidSpawner.nearPlayer)
                    {
                        playNext = asteroidField;
                    }
                    else if (GraveyardRuinSpawner.nearPlayer)
                    {
                        playNext = shipGraveyard;
                    }
                    else if (stationNumb.Length > 0)
                    {
                        playNext = civilSpace;
                    }
                    else if (ThingSpawner.totalDeliveries > 0)
                    {
                        playNext = delivery;
                    } 
                    else
                    {
                        playNext = travel;
                    }

                    if (prev != playNext && !changing)
                    {
                        changing = true;
                        changeTimeleft = 1f;
                    }
                    checkTime = .5f;
                }
            }
            else
            {
                fade = Mathf.Lerp(0, 1, changeTimeleft / 2);
                
                changeTimeleft -= Time.deltaTime;
                if (changeTimeleft < 0)
                {
                    AudioSource.clip = playNext;
                    AudioSource.Play();
                    changing = false;
                }
            }

        }
        else if (!forMenu)
        {
            GetComponent<AudioSource>().volume = SFXVolume * masterVolume;
        }
        else if (forMenu)
        {
            GetComponent<AudioSource>().volume = songVolume * masterVolume;
        }
    }
}
