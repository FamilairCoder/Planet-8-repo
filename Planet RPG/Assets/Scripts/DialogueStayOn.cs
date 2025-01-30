using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueStayOn : MonoBehaviour
{
    public GameObject stayOn, textObj;
    //private DialogueManager manager;
    public string text;
    public bool did;
    // Start is called before the first frame update
    void Start()
    {
        textObj.GetComponent<TextMeshPro>().text = "";
        StartCoroutine(DisplayText());
    }

    // Update is called once per frame
    void Update()
    {
        if (stayOn != null)
        {
            var stayPos = stayOn.transform.position;
            transform.position = new(stayPos.x, stayPos.y + 1f, -5f);
        }
        else Destroy(gameObject);
    }

    private IEnumerator DisplayText()
    {
        var audio = GetComponent<AudioSource>();
        for (int i = 0; i < text.Length; i++)
        {
            var chance = Random.Range(0f, 1f);
            textObj.GetComponent<TextMeshPro>().text += text[i];
            audio.pitch = Random.Range(.85f, 1.15f);
            audio.Play();            
            yield return new WaitForSeconds(.05f);
        }
        did = true;
        yield return null;
    }
}
