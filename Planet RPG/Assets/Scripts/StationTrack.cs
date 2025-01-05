using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StationTrack : MonoBehaviour, IPointerClickHandler
{
    public GameObject player, station;
    private bool track;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var player_pos = player.transform.position;
        var enemy_pos = station.transform.position;
        if (track && Vector2.Distance(player_pos, enemy_pos) > 50)
        {
            GetComponent<LineRenderer>().enabled = true;
            GetComponent<LineRenderer>().SetPosition(0, player_pos);
            GetComponent<LineRenderer>().SetPosition(1, enemy_pos);
        }
        else if (track && Vector2.Distance(player_pos, enemy_pos) < 50)
        {
            GetComponent<LineRenderer>().enabled = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (track)
        {
            track = false;
            GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
            GetComponent<LineRenderer>().enabled = false;

        }
        else
        {
            track = true;
            GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Underline;
            GetComponent<LineRenderer>().enabled = true;
        }

    }
}
