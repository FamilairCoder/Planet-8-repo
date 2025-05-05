using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LoadingSceneManager : MonoBehaviour
{
    public static int sceneToLoad;
    public static Sprite spr, stationSpr, shipSpr;
    public SpriteRenderer stationObj, shipObj;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SceneLoad());
    }

    // Update is called once per frame
    void Update()
    {
        spr = GetComponent<SpriteRenderer>().sprite;
        stationSpr = stationObj.sprite;
        shipSpr = shipObj.sprite;
    }

    IEnumerator SceneLoad()
    {
        //loadingScreen.SetActive(true); // Show loading screen
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        operation.allowSceneActivation = false; // Prevent auto activation

        while (operation.progress < 0.9f) // Wait until scene is mostly loaded
        {
            //Debug.Log("Progress: " + operation.progress);
            yield return null; // Wait next frame
        }

        // Scene is ready, smooth delay before activation
        for (float t = 0; t < 2f; t += Time.deltaTime)
        {
            //Debug.Log(t);
            //progressBar.value = Mathf.Lerp(progressBar.value, 1f, t / 2f);
            yield return null;
        }

        operation.allowSceneActivation = true; // Activate scene
    }
}
