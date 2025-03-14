using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public AudioClip clip;
    private DialogueManager manager;
    private GameObject createdDialogue;
    private bool spinning, inDanger, threatening, retreating;
    private float timeleft = 3f, threatenTime = 3f;
    private NPCmovement npcM;
    // Start is called before the first frame update
    void Start()
    {
        //player = FindObjectOfType<PlayerMovement>().gameObject;
        manager = FindObjectOfType<DialogueManager>();

        npcM = GetComponent<NPCmovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (/*(threatening || retreating || spinning || inDanger) && */createdDialogue != null && createdDialogue.GetComponent<DialogueStayOn>().did)
        {
            //Debug.Log(timeleft);
            timeleft -= Time.deltaTime;
            
            if (timeleft < 0)
            {
                Destroy(createdDialogue);
                spinning = false;
                //threatening = false;                
                timeleft = 3;
            }
        }

        if (npcM.found_danger && !inDanger)
        {
            //Debug.Log("creating");
            var d = Instantiate(manager.dialogueBox, transform.position, Quaternion.identity);
            createdDialogue = d;
            var script = d.GetComponent<DialogueStayOn>();
            script.stayOn = gameObject;
            script.text = manager.pirateFear[Random.Range(0, manager.pirateFear.Count)];

            inDanger = true;
            
        }
        else if (!npcM.found_danger && inDanger)
        {
            //Debug.Log("setting inDanger to false");
            inDanger = false;
        }

        if (!threatening && npcM.target != null)
        {
            if (createdDialogue != null) Destroy(createdDialogue);
            var d = Instantiate(manager.dialogueBox, transform.position, Quaternion.identity);
            createdDialogue = d;
            var script = d.GetComponent<DialogueStayOn>();
            script.stayOn = gameObject;
            d.GetComponent<AudioSource>().clip = clip;
            if (npcM.is_pirate) script.text = manager.pirateThreat[Random.Range(0, manager.pirateThreat.Count)];
            if (npcM.is_patrol) script.text = manager.patrolThreat[Random.Range(0, manager.patrolThreat.Count)];

            threatening = true;
        }



        if (!retreating && npcM.is_pirate && npcM.retreat)
        {
            if (createdDialogue != null) Destroy(createdDialogue);
            var d = Instantiate(manager.dialogueBox, transform.position, Quaternion.identity);
            createdDialogue = d;
            var script = d.GetComponent<DialogueStayOn>();
            script.stayOn = gameObject;
            d.GetComponent<AudioSource>().clip = clip;
            script.text = manager.pirateRetreat[Random.Range(0, manager.pirateRetreat.Count)];

            retreating = true;
        }

        if ((npcM.is_pirate || npcM.is_patrol) && threatening)
        {
            threatenTime -= Time.deltaTime;
            if (npcM.target != null)
            {
                threatenTime = 3f;
            }
            if (threatenTime <= 0)
            {
                threatening = false;
            }
        }
        if ((npcM.is_pirate || npcM.is_patrol) && !npcM.retreat)
        {
            retreating = false;
        }


        //if (!npcM.retreat && npcM.target == null)
        //{
        //    threatening = false;
        //    retreating = false;
        //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var ang = Mathf.Abs(GetComponent<Rigidbody2D>().angularVelocity);
        if (!spinning /*&& collision.gameObject.CompareTag("player")*/ && ang > 100 && (npcM.is_npc || npcM.is_patrol) && npcM.target == null)
        {
            if (createdDialogue != null) Destroy(createdDialogue);
            var d = Instantiate(manager.dialogueBox, transform.position, Quaternion.identity);
            createdDialogue = d;
            if (clip != null) d.GetComponent<AudioSource>().clip = clip;
            var script = d.GetComponent<DialogueStayOn>();
            script.stayOn = gameObject;
            if (npcM.is_npc)
            {
                if (ang < 200)
                {
                    script.text = manager.weakSpinDialogues[Random.Range(0, manager.weakSpinDialogues.Count)];
                }
                else
                {
                    script.text = manager.strongSpinDialogues[Random.Range(0, manager.strongSpinDialogues.Count)];
                }
            }
            else if (npcM.is_patrol)
            {
                if (!GetComponent<PatrolID>().taken)
                {
                    script.text = manager.patrolSpinDialogue[Random.Range(0, manager.patrolSpinDialogue.Count)];
                }
                else
                {
                    script.text = manager.patrolTakenSpinDialogue[Random.Range(0, manager.patrolTakenSpinDialogue.Count)];
                }
                
            }
            spinning = true;
        }
    }

}
