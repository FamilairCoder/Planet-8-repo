using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public LayerMask pirateMask;
    public AudioSource boostSound, inhibitedBoostSound, bountySound;
    public ParticleSystem boostParticles;
    public GameObject current_ship, inhibitText;
    private GameObject ship_prefab;
    public float zoomspd;
    public static bool dead;
    private float spd, turning_spd, spd_up, spd_down, dead_time = 3;
    public static float target_zoom, beam_slow;

    private float zoom_offset;
    private bool did_position, concreteMovement;
    public bool inhibited;
    // Start is called before the first frame update
    void Start()
    {
        var ship = current_ship.GetComponent<ShipStats>();
        spd = ship.spd;
        spd_up = spd * 8;
        spd_down = spd;
        turning_spd = ship.turning_spd;
        target_zoom = Camera.main.orthographicSize;
        
        
    }


    // Update is called once per frame
    void FixedUpdate()
    {



        var rb = GetComponent<Rigidbody2D>();
        if (!dead)
        {
            //concreteMovement = true;
            bool moving = false;
            if (Input.GetKey(KeyCode.W))
            {
                // rb.AddForce(transform.up * spd * Time.fixedDeltaTime, ForceMode2D.Impulse);
                if (!concreteMovement) Move(transform.up);
                else if (concreteMovement) Move(new Vector2(0, 1));

                moving = true;
            }


            else if (Input.GetKey(KeyCode.S))
            {
                //if (!concreteMovement) rb.AddForce(-transform.up * spd * Time.fixedDeltaTime, ForceMode2D.Impulse);
                if (!concreteMovement) Move(-transform.up);
                else if (concreteMovement) Move(new Vector2(0, -1));

                moving = true;

            }


            if (Input.GetKey(KeyCode.A))
            {
                //if (!concreteMovement) rb.AddForce(-transform.right * spd * Time.fixedDeltaTime, ForceMode2D.Impulse);

                if (!concreteMovement) Move(-transform.right);
                else if (concreteMovement) Move(new Vector2(-1, 0));

                moving = true;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                //if (!concreteMovement) rb.AddForce(transform.right * spd * Time.fixedDeltaTime, ForceMode2D.Impulse);

                if (!concreteMovement) Move(transform.right);
                else if (concreteMovement) Move(new Vector2(1, 0));

                moving = true;
            }
            if (!moving) rb.velocity = Vector2.Lerp(rb.velocity, new(0, 0), .1f);


            if (Input.GetKey(KeyCode.X))
            {
                rb.velocity = Vector2.Lerp(rb.velocity, new(0, 0), .2f);
            }
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // Set Z to 0 because we are working in 2D

            // Calculate the direction from the ship to the mouse
            Vector3 direction = (mousePosition - transform.position).normalized;

            // Calculate the angle to rotate the ship
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

            // Create a rotation representing the target angle
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, targetAngle));

            // Smoothly rotate the ship towards the target angle
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turning_spd * Time.deltaTime);
        }


        rb.velocity = Vector2.ClampMagnitude(rb.velocity, spd);





        
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt("ConcreteMovement", 0) == 1) concreteMovement = true;
        else concreteMovement = false;

        if (!did_position)
        {
            transform.position = new Vector2(PlayerPrefs.GetFloat("player positionx", 0), PlayerPrefs.GetFloat("player positiony", 0));
            did_position = true;
        }

        PlayerPrefs.SetFloat("player positionx", transform.position.x);
        PlayerPrefs.SetFloat("player positiony", transform.position.y);

        spd_up = spd * 8;
        inhibited = false;
        var hitPirates = Physics2D.OverlapCircleAll(transform.position, 75, pirateMask);
        foreach (var p in hitPirates)
        {
            if (p.GetComponent<NPCmovement>() != null && p.GetComponent<NPCmovement>().inhibit) { spd_up = spd * 1.25f; inhibited = true; }
        }

        if (dead)
        {
            spd = 0;
            dead_time -= Time.deltaTime;
            if (dead_time < 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.position = new Vector3(0, 0, 0);
                for (int i = 0; i < current_ship.transform.childCount; i++)
                {
                    var child = current_ship.transform.GetChild(i).gameObject;
                    if (child.GetComponent<Health>() != null && child.activeSelf) 
                    {
                        var chance = Random.Range(0f, 1f);
                        if (chance < .6f && current_ship.GetComponent<ShipStats>().core != child && !current_ship.GetComponent<ShipStats>().thrusters.Contains(child))
                        {
                            var healthLower = Random.Range(0f, 1f);
                            if (healthLower < .3f) child.GetComponent<Health>().hp = 0;
                            else if (healthLower > .3f) child.GetComponent<Health>().hp = child.GetComponent<Health>().orig_hp * Random.Range(.3f, .75f);


                        }
                        else
                        {
                            child.GetComponent<Health>().hp = child.GetComponent<Health>().orig_hp;
                        }

                        child.GetComponent<SpriteRenderer>().sprite = child.GetComponent<Health>().orig_sprite;

                    }
                }
                dead = false;
                dead_time = 3;
            }
        }
        else
        {
            var ship = current_ship.GetComponent<ShipStats>();
            spd = ship.spd * (1 + ship.thrust_bonus) * beam_slow;
            spd_up = spd * 8;
            spd_down = spd;
            turning_spd = ship.turning_spd * (1 + ship.turnspd_bonus) * beam_slow;
        }


        
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (!inhibited)
            {
                if (!HUDmanage.on_map) zoom_offset = Mathf.Lerp(zoom_offset, 10, .1f);
                spd = spd_up;
                
            }
            else
            {
                spd = spd_up / 1.5f;
                //if (!HUDmanage.on_map) zoom_offset = Mathf.Lerp(zoom_offset, 5, .1f);                
            }

        }
        else
        {
            spd = Mathf.Lerp(spd, spd_down, .03f);
            if (!HUDmanage.on_map) zoom_offset = Mathf.Lerp(zoom_offset, 0, .1f);
            boostParticles.Stop();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (inhibited) 
            { 
                Instantiate(inhibitText, transform.position, Quaternion.identity); 
                inhibitedBoostSound.Play();
            }
            else
            {
                boostSound.Play();
                boostParticles.Play();
            }
        }
        if (inhibited) boostParticles.Stop();

        Zoom();
        
    }

    void Zoom()
    {

        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");

        
        if (!HUDmanage.on_map)
        {
            if (scrollWheel < 0) { target_zoom += zoomspd; zoomspd *= 1.1f; }
            else if (scrollWheel > 0) { target_zoom -= zoomspd; zoomspd *= .9f; }
            
            target_zoom = Mathf.Clamp(target_zoom, 10, 80);
            //Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, target_zoom, .4f);
            //Camera.main.orthographicSize += zoom_offset;
            zoomspd = Mathf.Clamp(zoomspd, 4, 12);

            Camera.main.orthographicSize = target_zoom;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 10, 80);
            Camera.main.orthographicSize += zoom_offset;
        }
        else
        {
            if (scrollWheel < 0) { Camera.main.orthographicSize += 750; }
            else if (scrollWheel > 0) { Camera.main.orthographicSize -= 750; }


/*            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Camera.main.orthographicSize += 500;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Camera.main.orthographicSize -= 500;
            }*/
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 500, 10000);
        }


    }

    public void Move(Vector2 direction)
    {
        GetComponent<Rigidbody2D>().AddForce(direction * spd * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }
}
