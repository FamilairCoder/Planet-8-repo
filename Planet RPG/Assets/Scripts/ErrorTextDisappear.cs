using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ErrorTextDisappear : MonoBehaviour
{
    private GameObject player;
    public Vector2 offsetPos;
    private float offsetY;
    // Start is called before the first frame update
    void Start()
    {
        
        player = HUDmanage.playerReference;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        offsetY += 1 * Time.deltaTime;
        GetComponent<TextMeshPro>().color -= new Color(0, 0, 0, .6f) * Time.deltaTime;
        if (GetComponent<TextMeshPro>().color.a < 0) Destroy(gameObject);
        if (offsetPos == new Vector2(0, 0)) transform.position = new Vector3(player.transform.position.x, player.transform.position.y + offsetY);
        else transform.position = new Vector3(offsetPos.x, offsetPos.y + offsetY);
    }
}
