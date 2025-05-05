using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProbeUIScript : MonoBehaviour
{
    public bool is_current;
    public static bool is_current_static;
    public GameObject text;
    public static float probe_amount = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SaveManager.SetFloat("probes", probe_amount);
        if (is_current)
        {
            is_current_static = true;
            text.SetActive(true);
            text.GetComponent<TextMeshProUGUI>().text = "x" + probe_amount.ToString();
        }
    }
}
