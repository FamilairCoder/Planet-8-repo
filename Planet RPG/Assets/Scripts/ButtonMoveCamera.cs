using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMoveCamera : MonoBehaviour
{
    public Transform camera;
    public Vector3 toPos;
    public static bool moving;
    private bool pressed = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var campos = new Vector3(camera.position.x, camera.position.y, 0);
        if (pressed && Vector2.Distance(campos, toPos) > .1f)
        {
            var dir = (toPos - campos).normalized;
            //dir = new Vector3(dir.x, dir.y, campos.z);
            //camera.position += dir * 60 * Time.deltaTime;
            camera.position = Vector3.Lerp(camera.position, new Vector3(toPos.x, toPos.y, -10), .15f);
        }
        else if (pressed)
        {
            camera.position = new Vector3(toPos.x, toPos.y, -10);
            pressed = false;
            moving = false;
        }
    }
    private void OnMouseDown()
    {
        if (!moving)
        {
            moving = true;
            pressed = true;
        }
    }
}
