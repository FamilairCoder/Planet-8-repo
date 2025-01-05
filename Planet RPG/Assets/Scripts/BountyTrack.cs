using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BountyTrack : MonoBehaviour, IPointerClickHandler
{
    private bool track;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //var player_pos = FindObjectOfType<PlayerMovement>().transform.position;
        if (track)
        {
            var player_pos = transform.parent.GetComponent<BountyPosterScript>().player.transform.position;
            var enemy_pos = transform.parent.GetComponent<BountyPosterScript>().tied_enemy.transform.position;
            if (Vector2.Distance(player_pos, enemy_pos) > 20)
            {
                GetComponent<LineRenderer>().enabled = true;
                GetComponent<LineRenderer>().SetPosition(0, player_pos);
                GetComponent<LineRenderer>().SetPosition(1, enemy_pos);
            }
            else if (Vector2.Distance(player_pos, enemy_pos) < 20)
            {
                GetComponent<LineRenderer>().enabled = false;
            }
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
