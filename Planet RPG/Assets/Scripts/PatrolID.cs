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
    public float id;
    public bool taken;
    public PatrolSpawner spawnCameFrom;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (!taken && Input.GetMouseButtonDown(0) && GetComponent<Collider2D>().OverlapPoint(mousePos) && !HUDmanage.on_map)
        {
            particles.Play();
            clip.Play();
            //Play vfx and sfx
            taken = true;
            Instantiate(ind, transform.position, Quaternion.identity).GetComponent<IndicatorPositioning>().tiedObj = gameObject;
        }
    }
}
