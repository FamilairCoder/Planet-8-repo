using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public AudioClip clip;
    private DialogueManager manager;
    private GameObject createdDialogue;
    private bool spinning, inDanger, threatening, retreating;
    private float timeleft = 2f, threatenTime = 3f;
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
                timeleft = 2;
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

        if (!threatening && npcM.is_pirate && npcM.target != null)
        {
            var d = Instantiate(manager.dialogueBox, transform.position, Quaternion.identity);
            createdDialogue = d;
            var script = d.GetComponent<DialogueStayOn>();
            script.stayOn = gameObject;
            d.GetComponent<AudioSource>().clip = clip;
            script.text = manager.pirateThreat[Random.Range(0, manager.pirateThreat.Count)];

            threatening = true;
        }



        if (!retreating && npcM.is_pirate && npcM.retreat)
        {
            var d = Instantiate(manager.dialogueBox, transform.position, Quaternion.identity);
            createdDialogue = d;
            var script = d.GetComponent<DialogueStayOn>();
            script.stayOn = gameObject;
            d.GetComponent<AudioSource>().clip = clip;
            script.text = manager.pirateRetreat[Random.Range(0, manager.pirateRetreat.Count)];

            retreating = true;
        }

        if (npcM.is_pirate && threatening)
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
        if (npcM.is_patrol && !npcM.retreat)
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
        if (!spinning && collision.gameObject.CompareTag("player") && ang > 100 && npcM.is_npc)
        {    
            var d = Instantiate(manager.dialogueBox, transform.position, Quaternion.identity);
            createdDialogue = d;
            var script = d.GetComponent<DialogueStayOn>();
            script.stayOn = gameObject;
            if (ang < 200)
            {
                script.text = manager.weakSpinDialogues[Random.Range(0, manager.weakSpinDialogues.Count)];
            }
            else 
            {
                script.text = manager.strongSpinDialogues[Random.Range(0, manager.strongSpinDialogues.Count)];
            }
            spinning = true;
        }
    }

}
