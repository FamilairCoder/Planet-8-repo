using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public static float songVolume = 1, masterVolume = .5f, SFXVolume = .5f;
    public bool isManager, forMenu, dontPitchShift;
    private bool changing, traveling, isLaser, inMenu;
    private float changeTimeleft, checkTime, fade = 1, saveTime;
    private Collider2D[] stationNumb, pirateNumb;
    public GameObject player;
    private Transform playerPos;
    public AudioClip civilSpace, travel, fighting, asteroidField, shipGraveyard, delivery;
    private AudioClip playNext;
    private AudioSource AudioSource;    

    private float time, civilSpaceTime, travelTime, fightingTime, asteroidFieldTime, shipGraveyardTime, deliveryTime;
    // Start is called before the first frame update
    private void Awake()
    {
        if (isManager || forMenu)
        {
            GetComponent<AudioSource>().volume = songVolume * masterVolume * fade;
        }
        else if (FindObjectOfType<PlayerMovement>() != null)
        {
            playerPos = FindObjectOfType<PlayerMovement>().gameObject.transform;
            GetComponent<AudioSource>().volume = Mathf.Lerp(SFXVolume * masterVolume, 0, Vector2.Distance(transform.position, playerPos.position) / 100);
            if (!dontPitchShift) GetComponent<AudioSource>().pitch = Random.Range(.5f, 1.5f);
        }
        else if(FindObjectOfType<PlayerMovement>() == null)
        {
            GetComponent<AudioSource>().volume = SFXVolume * masterVolume;
            if (!dontPitchShift) GetComponent<AudioSource>().pitch = Random.Range(.5f, 1.5f);
            inMenu = true;
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
                        time = asteroidFieldTime;
                    }
                    else if (GraveyardRuinSpawner.nearPlayer)
                    {
                        playNext = shipGraveyard;
                        time = shipGraveyardTime;
                    }
                    else if (stationNumb.Length > 0)
                    {
                        playNext = civilSpace;
                        time = civilSpaceTime;
                    }
                    else if (ThingSpawner.totalDeliveries > 0)
                    {
                        playNext = delivery;
                        time = deliveryTime;
                    } 
                    else
                    {
                        playNext = travel;
                        time = travelTime;
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
                    AudioSource.time = time;
                    AudioSource.Play();
                    changing = false;
                }
            }
            if (AudioSource.clip != null)
            {
                var clip = AudioSource.clip;
                if (clip == fighting) fightingTime = AudioSource.time;
                if (clip == asteroidField) asteroidFieldTime = AudioSource.time;
                if (clip == shipGraveyard) shipGraveyardTime = AudioSource.time;
                if (clip == civilSpace) civilSpaceTime = AudioSource.time;
                if (clip == delivery) deliveryTime = AudioSource.time;
                if (clip == travel) travelTime = AudioSource.time;
            }


        }
        else if (!forMenu)
        {
            if (!inMenu) GetComponent<AudioSource>().volume = Mathf.Lerp(SFXVolume * masterVolume, 0, Vector2.Distance(transform.position, playerPos.position) / 100);
            else GetComponent<AudioSource>().volume = SFXVolume * masterVolume;
        }
        else if (forMenu)
        {
            GetComponent<AudioSource>().volume = songVolume * masterVolume;
        }
    }
}
