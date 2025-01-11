using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSceneChange : MonoBehaviour
{
    public GameObject camera, darkOverlay;
    public bool forMainScene;
    private bool toMainScene;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (toMainScene)
        {
            camera.GetComponent<Camera>().orthographicSize -= 2 * Time.deltaTime;
            darkOverlay.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, .8f) * Time.deltaTime;
            if (darkOverlay.GetComponent<SpriteRenderer>().color.a >= 1f) SceneManager.LoadScene(1);
        }
    }
    private void OnMouseDown()
    {
        if (forMainScene)
        {
            toMainScene = true;
        }
    }
}
