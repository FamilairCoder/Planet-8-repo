using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
//using static UnityEditor.PlayerSettings;


public class HUDmanage : MonoBehaviour
{
    public List<GameObject> ship_directory = new List<GameObject>();
    public List<GameObject> secondaries = new List<GameObject>();
    public List<bool> has_secondary = new List<bool>();
    public List<GameObject> code_has_secondary = new List<GameObject>();

    public GameObject player, secondary_ui, side_secondary_ui;
    public GameObject money_text, bounty_poster, tied_enemy, tied_poster, cargo_ui;
    public List<GameObject> active_bounties = new List<GameObject>();
    public Canvas canvas;    
    public Sprite picture;
    public float cost;
    public bool new_bounty;
    public static float money;    
    public int index;

    private bool create_second_image, has_one_secondary, has_two_secondary, create_side_second_image;
    private GameObject current_secondary, left_secondary, right_secondary;

    public static bool on_map;
    private bool did_zoom;
    public GameObject map;
    public static float lvl = 1;

    public static List<SetDelivery> sd = new List<SetDelivery>();

    public static AudioSource bountySound;

    public static GameObject playerReference;
    public static Material pirateSecondaryMatRef;
    public static List<string> pirateTags = new List<string>();
    [Header("for various npc needs------------------------")]
    public List<string> pirateTagsEditor = new List<string>();
    public Material pirateSecondaryMat;

    public static bool pauseMenu;
    public static float DONT;

    private float secondDelay = .5f;
    //public GameObject pauseMenuObj;
    private void Awake()
    {
        bountySound = FindObjectOfType<PlayerMovement>().bountySound;
        playerReference = FindObjectOfType<PlayerMovement>().gameObject;
        pirateTags = pirateTagsEditor;
        pirateSecondaryMatRef = pirateSecondaryMat;

        //get rid of this--------------------------
        PlayerPrefs.DeleteAll();
        Debug.Log("deleting all playerprefs");
        //get rid of this--------------------------


        sd = new List<SetDelivery>(FindObjectsOfType<SetDelivery>());
    }
    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        money = PlayerPrefs.GetFloat("money", 0);


        //get rid of this--------------------------
        money = 10000;
        Debug.Log("set money to 10000");
        //get rid of this--------------------------
    }


    // Update is called once per frame
    void Update()
    {
        DONT -= Time.deltaTime;

        //Application.targetFrameRate = -1;

        foreach ( SetDelivery sds in sd )
        {
            
            sds.MyUpdate();
        }


        PlayerPrefs.SetFloat("money", money);

        money_text.GetComponent<TextMeshProUGUI>().text = Mathf.Round(money).ToString();

        if (Input.GetKeyDown(KeyCode.M) && !TextInputScript.typing && !pauseMenu)
        {
            var pm = FindObjectOfType<PlayerMovement>();
            if (!on_map) 
            {
                if (!did_zoom)
                {
                    Camera.main.orthographicSize = 1500;
                    did_zoom = true;
                }
                map.SetActive(true);
                on_map = true;
            }
            else
            {
                if (did_zoom)
                {
                    Camera.main.orthographicSize = 80;
                    PlayerMovement.target_zoom = 10;
                    pm.zoomspd = 4;
                    did_zoom = false;
                }
                map.SetActive(false);
                on_map = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) && on_map)
        {
            if (did_zoom)
            {
                Camera.main.orthographicSize = 80;
                PlayerMovement.target_zoom = 10;
                FindObjectOfType<PlayerMovement>().zoomspd = 4;
                did_zoom = false;
            }
            map.SetActive(false);
            on_map = false;
        }
        
        else if (DONT <= 0 && Input.GetKeyDown(KeyCode.Escape) && !OpenMenu.opened && !on_map && !PatrolManager.focusFire)
        {
            if (!pauseMenu)
            {
                //pauseMenuObj.SetActive(true);
                pauseMenu = true;
            }
            else
            {
                //pauseMenuObj.SetActive(false);
                pauseMenu = false;
            }
        }
        


        if (PlayerMining.cargo_amount > 0)
        {
            cargo_ui.SetActive(true);
        }

        if (new_bounty)
        {
            var a = Instantiate(bounty_poster, new Vector3(0, 0, 0), Quaternion.identity, canvas.transform);
            a.transform.localScale = new Vector3(2, 2, 2);
            a.GetComponent<BountyPosterScript>().picture = picture;
            a.GetComponent<BountyPosterScript>().cost = cost;
            a.GetComponent<BountyPosterScript>().tied_enemy = tied_enemy;
            a.GetComponent<BountyPosterScript>().player = player;
            a.GetComponent<BountyPosterScript>().on_screen = true;
            a.GetComponent<BountyPosterScript>().tied_poster = tied_poster;
            active_bounties.Add(a);
            new_bounty = false;
        }


        secondDelay -= Time.deltaTime;
        if (secondDelay < 0)
        {
            var n = 0;
            foreach (var s in has_secondary)
            {
                if (s)
                {
                    n++;
                    if (!has_one_secondary) has_one_secondary = true;
                    if (n > 1) has_two_secondary = true;
                }
            }
            if (code_has_secondary.Count >= 1) { has_one_secondary = true; }
            else { has_one_secondary = false; }
            if (code_has_secondary.Count >= 2) { has_two_secondary = true; }
            else { has_two_secondary = false; };


            if (current_secondary != null && current_secondary.GetComponent<ProbeUIScript>() != null)
            {
                current_secondary.GetComponent<ProbeUIScript>().is_current = true;
            }
            else
            {
                ProbeUIScript.is_current_static = false;
            }

            if (has_one_secondary)
            {
                secondary_ui.SetActive(true);
                if (!create_second_image)
                {
                    //current_secondary = Instantiate(secondaries[index], secondary_ui.transform.position, Quaternion.identity, secondary_ui.transform);
                    current_secondary = Instantiate(code_has_secondary[0], secondary_ui.transform.position, Quaternion.identity, secondary_ui.transform);


                    create_second_image = true;
                }



                if (has_two_secondary)
                {
                    side_secondary_ui.SetActive(true);
                    if (!create_side_second_image)
                    {
                        CreateSideSecondary();

                        create_side_second_image = true;
                    }
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        Destroy(current_secondary);
                        Destroy(left_secondary);
                        Destroy(right_secondary);
                        index -= 1;
                        if (index < 0) index = code_has_secondary.Count - 1;
                        else if (index > code_has_secondary.Count - 1) index = 0;
                        current_secondary = Instantiate(code_has_secondary[index], secondary_ui.transform.position, Quaternion.identity, secondary_ui.transform);
                        CreateSideSecondary();
                    }
                    else if (Input.GetKeyDown(KeyCode.E))
                    {
                        Destroy(current_secondary);
                        Destroy(left_secondary);
                        Destroy(right_secondary);
                        index += 1;
                        if (index < 0) index = code_has_secondary.Count - 1;
                        else if (index > code_has_secondary.Count - 1) index = 0;
                        current_secondary = Instantiate(code_has_secondary[index], secondary_ui.transform.position, Quaternion.identity, secondary_ui.transform);
                        CreateSideSecondary();
                    }
                }
                else
                {
                    side_secondary_ui.SetActive(false);
                }

            }
            else
            {
                side_secondary_ui.SetActive(false);
                secondary_ui.SetActive(false);
            }
            secondDelay = .3f;
        }
        for (int i = 0; i < active_bounties.Count; i++)
        {
            if (active_bounties[i] == null) { active_bounties.Remove(active_bounties[i]); break; }
            active_bounties[i].GetComponent<RectTransform>().position = new Vector3(330 + ((i - 1) * 220), 960, 0f);

        }



    }


    void CreateSideSecondary()
    {
        var left_index = index - 1;
        var right_index = index + 1;
        if (left_index < 0) left_index = code_has_secondary.Count - 1;
        else if (left_index > code_has_secondary.Count - 1) left_index = 0;
        if (right_index < 0) right_index = code_has_secondary.Count - 1;
        else if (right_index > code_has_secondary.Count - 1) right_index = 0;

        if (code_has_secondary[left_index] != null)
        {
            left_secondary = Instantiate(code_has_secondary[left_index], secondary_ui.transform.position, Quaternion.identity, secondary_ui.transform);
            left_secondary.transform.localPosition = new Vector2(-71.7f, 0);
            left_secondary.transform.localScale = new Vector2(.5f, .5f);
        }
        if (code_has_secondary[right_index] != null)
        {
            right_secondary = Instantiate(code_has_secondary[right_index], secondary_ui.transform.position, Quaternion.identity, secondary_ui.transform);
            right_secondary.transform.localPosition = new Vector2(71.7f, 0);
            right_secondary.transform.localScale = new Vector2(.5f, .5f);
        }



    }

    public void PublicGotNew()
    {
        
        //if (index + 1 > 1)
        //{
        //Debug.Log(index);
        Destroy(current_secondary);
        Destroy(left_secondary);
        Destroy(right_secondary);
        //index += 1;

        if (index < 0) index = code_has_secondary.Count - 1;
        else if (index > code_has_secondary.Count - 1) index = 0; 


        current_secondary = Instantiate(code_has_secondary[index], secondary_ui.transform.position, Quaternion.identity, secondary_ui.transform);
        CreateSideSecondary();
        create_second_image = true;
        create_side_second_image = true;
        //}

    }
}
