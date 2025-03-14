using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonSceneChange : MonoBehaviour, IPointerClickHandler
{
    public GameObject camera, darkOverlay;
    public int toSceneIndex;
    //private bool toMainScene;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
/*        if (toMainScene)
        {
            camera.GetComponent<Camera>().orthographicSize -= 2 * Time.deltaTime;
            darkOverlay.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, .8f) * Time.deltaTime;
            if (darkOverlay.GetComponent<SpriteRenderer>().color.a >= 1f) 
        }*/
    }
    private void OnMouseDown()
    {
        if (GetComponent<Image>() == null)
        {
            LoadingSceneManager.sceneToLoad = toSceneIndex;
            SceneManager.LoadScene(2);
        }

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (GetComponent<Image>() != null)
        {
            HUDmanage.pauseMenu = false;
            LoadingSceneManager.sceneToLoad = toSceneIndex;
            SceneManager.LoadScene(2);
        }
    }

}
