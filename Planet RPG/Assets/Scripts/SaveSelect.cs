using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSelect : MonoBehaviour
{
    public int saveId;
    public static bool selected;
    public List<GameObject> secondariesEditor = new List<GameObject>();
    public static List<GameObject> secondaries = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.DeleteKey("Save File ID");
        secondaries = secondariesEditor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        PlayerPrefs.SetInt("Save File ID", saveId);
        //();
        SaveManager.SaveFileID = saveId;
        selected = true;
        transform.localScale = new Vector3(8, 8);
    }
    private void OnMouseOver()
    {
        if (PlayerPrefs.GetInt("Save File ID") != saveId)
        {
            transform.localScale = new Vector3(7, 7);
        }
    }
    private void OnMouseExit()
    {
        transform.localScale = new Vector3(8, 8);
    }
}
