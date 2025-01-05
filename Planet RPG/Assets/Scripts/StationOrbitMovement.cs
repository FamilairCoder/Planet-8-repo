using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationOrbitMovement : MonoBehaviour
{
    public GameObject orbit_around;
    public float distx, disty;
    private float spd, vec;
    // Start is called before the first frame update
    void Start()
    {
        spd = 5f;
        vec = Random.Range(0f, 360f);
    }

    // Update is called once per frame
    void Update()
    {
        vec += spd * Time.deltaTime;
        var pos = new Vector2(orbit_around.transform.position.x + (distx * Mathf.Sin(Mathf.Deg2Rad * vec)), orbit_around.transform.position.y + (disty * Mathf.Cos(Mathf.Deg2Rad * vec)));
        transform.position = pos;
    }
}
