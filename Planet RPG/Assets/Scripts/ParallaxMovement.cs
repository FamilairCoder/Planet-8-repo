using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxMovement : MonoBehaviour
{
    public float parallaxFactor, min, max;
    public bool randomize, setPlayer;

    public Transform playerTransform;
    private Vector3 previousCameraPosition;
    private Vector3 cameraVelocity;
    void Start()
    {
        if (setPlayer) playerTransform = HUDmanage.playerReference.transform;
        if (randomize)
        {
            parallaxFactor = Random.Range(min, max);
        }
        
        previousCameraPosition = playerTransform.position;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = playerTransform.position - previousCameraPosition;
        //deltaMovement = Camera.main.velocity;
        cameraVelocity = (playerTransform.position - previousCameraPosition) / Time.deltaTime;

        transform.position += new Vector3(cameraVelocity.x * parallaxFactor, cameraVelocity.y * parallaxFactor, 0) * Time.deltaTime;
        previousCameraPosition = playerTransform.position;
        //Debug.Log(Camera.main.velocity);
    }
}
