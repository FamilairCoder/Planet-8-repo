using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public static float songVolume = 1, masterVolume = .5f, SFXVolume = .5f;
    public bool isManager, forMenu, dontPitchShift;
    
    private float changeTimeleft, checkTime, fade = 1, saveTime, distVolumeTime;
    private Collider2D[] stationNumb, pirateNumb, pirateTrainNumb, bigWrecks;
    public GameObject player;
    private Transform playerPos;
    
    private AudioClip playNext;
    private AudioSource AudioSource;

    private bool changing, traveling, isLaser, inMenu, nearPirateStation;
    public AudioClip civilSpace, travel, fighting, asteroidField, shipGraveyard, delivery, pirateStation, pirateTrain, bigWreckSong;
    private float time, civilSpaceTime, travelTime, fightingTime, asteroidFieldTime, shipGraveyardTime, deliveryTime, pirateStationTime, pirateTrainTime, bigWreckTime;
    // Start is called before the first frame update
    private void Awake()
    {
        var player = HUDmanage.playerReference;


        //if (gameObject.name == "PirateTrain") Debug.Log(player);
        if (isManager || forMenu)
        {
            GetComponent<AudioSource>().volume = songVolume * masterVolume * fade;
            songVolume = PlayerPrefs.GetFloat("SongVolume", 1);
            masterVolume = PlayerPrefs.GetFloat("MasterVolume", .5f);
            SFXVolume = PlayerPrefs.GetFloat("SFXVolume", .5f);
            
        }
        else if (player != null)
        {
            
            playerPos = player.gameObject.transform;
            GetComponent<AudioSource>().volume = Mathf.Lerp(SFXVolume * masterVolume, 0, Vector2.Distance(transform.position, playerPos.position) / 100);
            if (!dontPitchShift) GetComponent<AudioSource>().pitch = Random.Range(.5f, 1.5f);
        }
        else if (player == null)
        {
            GetComponent<AudioSource>().volume = SFXVolume * masterVolume;
            if (!dontPitchShift) GetComponent<AudioSource>().pitch = Random.Range(.5f, 1.5f);
            if (gameObject.name != "PirateTrain" && gameObject.name != "PirateTrain (1)")inMenu = true;
        }


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
        distVolumeTime -= Time.deltaTime;
        if (saveTime < 0 && forMenu)
        {
            PlayerPrefs.SetFloat("SongVolume", songVolume);
            PlayerPrefs.SetFloat("MasterVolume", masterVolume);
            PlayerPrefs.SetFloat("SFXVolume", SFXVolume);
            

            saveTime = .1f;
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
                    nearPirateStation = false;
                    var prev = playNext;
                    stationNumb = Physics2D.OverlapCircleAll(player.transform.position, 100, LayerMask.GetMask("station"));
                    pirateNumb = Physics2D.OverlapCircleAll(player.transform.position, 100, LayerMask.GetMask("pirates"));
                    pirateTrainNumb = Physics2D.OverlapCircleAll(player.transform.position, 100, LayerMask.GetMask("pirateTrain"));
                    bigWrecks = Physics2D.OverlapCircleAll(player.transform.position, .01f, LayerMask.GetMask("bigWreck"));
                    foreach (var item in pirateNumb)
                    {
                        if (item.GetComponent<PirateShipSpawner>() != null)
                        {
                            nearPirateStation = true;
                            break;
                        }
                    }

                    if (pirateTrainNumb.Length > 0)
                    {
                        playNext = pirateTrain;
                        time = pirateTrainTime;
                    }
                    else if (bigWrecks.Length > 0)
                    {
                        playNext = bigWreckSong;
                        time = bigWreckTime;
                    }
                    else if (nearPirateStation)
                    {
                        playNext = pirateStation;
                        time = pirateStationTime;
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
                    else if (pirateNumb.Length > 0)
                    {
                        playNext = fighting;
                        time = fightingTime;
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
                if (clip == pirateStation) pirateStationTime = AudioSource.time;
                if (clip == pirateTrain) pirateTrainTime = AudioSource.time;
                if (clip == bigWreckSong) bigWreckTime = AudioSource.time;
            }


        }
        else if (!forMenu)
        {
            if (distVolumeTime < 0)
            {
                if (!inMenu) { GetComponent<AudioSource>().volume = Mathf.Lerp(SFXVolume * masterVolume, 0, Vector2.Distance(transform.position, HUDmanage.playerReference.gameObject.transform.position) / 100);  }
                else GetComponent<AudioSource>().volume = SFXVolume * masterVolume;
                distVolumeTime = Random.Range(0f, .2f);
            }

        }
        else if (forMenu)
        {
            GetComponent<AudioSource>().volume = songVolume * masterVolume;
        }
    }
}
