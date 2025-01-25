using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewOrbitMovement : MonoBehaviour
{
    public float dist, spd;
    private float vec;
    // Start is called before the first frame update
    void Start()
    {
        vec = Random.Range(0f, 360f);
    }

    // Update is called once per frame
    void Update()
    {
        vec += spd * Time.deltaTime;
        transform.localPosition = new Vector2(dist * Mathf.Sin(Mathf.Deg2Rad * vec), dist * Mathf.Cos(Mathf.Deg2Rad * vec));
    }
}
