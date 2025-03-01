using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxMovement : MonoBehaviour
{
    public float parallaxFactor, min, max;
    public bool randomize, setPlayer;

    public Transform playerTransform;
    private Vector3 previousCameraPosition;

    void Start()
    {
        if (setPlayer) playerTransform = HUDmanage.playerReference.transform;
        if (randomize)
        {
            parallaxFactor = Random.Range(min, max);
        }
        
        previousCameraPosition = playerTransform.position;
    }

    void Update()
    {
        Vector3 deltaMovement = playerTransform.position - previousCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxFactor, deltaMovement.y * parallaxFactor, 0);
        previousCameraPosition = playerTransform.position;
    }
}
