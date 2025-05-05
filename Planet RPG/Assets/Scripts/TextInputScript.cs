using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextInputScript : MonoBehaviour
{
    public static bool typing;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TMP_InputField>().onEndEdit.AddListener(EditExit);
        GetComponent<TMP_InputField>().onSelect.AddListener(EditEnter);
        GetComponent<TMP_InputField>().text = SaveManager.GetFloat("stayDist", 20).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EditExit(string text)
    {
        bool valid = float.TryParse(text, out float result);
        if (valid)
        {
            //Debug.Log(result);
            PatrolID.stayDist = result;
            GetComponent<TMP_InputField>().text = result.ToString();
            SaveManager.SetFloat("stayDist", result);
            
        }
        else
        {
            GetComponent<TMP_InputField>().text = PatrolID.stayDist.ToString();
            GetComponent<TMP_InputField>().textComponent.color = Color.red;
            StartCoroutine(SwitchColorBack());
        }
        typing = false;
    }
    void EditEnter(string text)
    {
        typing = true;
    }


    IEnumerator SwitchColorBack()
    {
        yield return new WaitForSeconds(.4f);
        GetComponent<TMP_InputField>().textComponent.color = Color.white;
        yield return null;
    }
}
