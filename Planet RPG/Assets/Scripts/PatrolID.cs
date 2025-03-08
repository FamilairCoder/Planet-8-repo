using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PatrolID : MonoBehaviour
{
    public static float stayDist = 20f;
    public ParticleSystem particles;
    public AudioSource clip;
    public GameObject ind;
    public ParticleSystem boostParticles;
    [Header("Identification stuff---------------------")]
    public string id;
    public bool taken;
    public bool didTaken;
    public PatrolSpawner spawnCameFrom;
    // Start is called before the first frame update
    void Start()
    {
        stayDist = PlayerPrefs.GetFloat("stayDist", 20);
    }

    // Update is called once per frame
    void Update()
    {
/*        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (!taken && Input.GetMouseButtonDown(0) && GetComponent<Collider2D>().OverlapPoint(mousePos) && !HUDmanage.on_map)
        {

        }*/
    }

    public void Hire()
    {

        //Play vfx and sfx
        taken = true;
        Instantiate(ind, transform.position, Quaternion.identity).GetComponent<IndicatorPositioning>().tiedObj = gameObject;
        if (taken && !didTaken)
        {
            particles.Play();
            clip.Play();
            PatrolManager.patrols.Add(gameObject);
            didTaken = true;
        }
        PlayerPrefs.SetFloat("taken" + id, 1);
        PlayerPrefs.Save();
    }
}
