using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //public AudioClip hitSFX;
    public GameObject explosion, came_from;
    public float spd, dmg;
    private float lifetime = 2;
    public string target_tag;
    public bool missile;
    public float turn_spd;
    public GameObject target;
    private Vector3 nextPos;
    private bool collided;
    // Start is called before the first frame update
    void Start()
    {
        if (missile) lifetime = 8;
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (missile) spd = Mathf.Lerp(60, 5, lifetime / 8);
        if (lifetime < 4) turn_spd = 0;
        if (lifetime < 0)
        {
            Destroy(gameObject);
        }
        
        if (missile)
        {
            if (target != null)
            {
                var dir = (target.transform.position - transform.position).normalized;
                transform.up = Vector3.RotateTowards(transform.up, dir, turn_spd * Time.deltaTime, 0);
                transform.rotation = Quaternion.Euler(0, 0, transform.localEulerAngles.z);
            }

        }

        nextPos = transform.position + transform.up * spd * Time.deltaTime;
        RaycastHit2D hit = Physics2D.Raycast(nextPos, transform.up, spd * Time.deltaTime);

        if (hit.collider != null)
        {

            Collision(hit.collider, hit.point);
        }

        transform.position = nextPos;

        
    }
/*    void FixedUpdate()
    {
        float moveDistance = spd * Time.fixedDeltaTime;
        Vector3 nextPos = transform.position + transform.up * moveDistance;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, moveDistance);

        if (hit.collider == null)
        {
            transform.position = Vector3.Lerp(transform.position, nextPos, 0.5f);
        }
        else
        {
            Collision(hit.collider, hit.point);
            //Destroy(gameObject);
        }
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        Collision(collision, transform.position);
    }

    void Collision(Collider2D collision, Vector2 exploPos)
    {

        if (came_from != null && target_tag != null && target_tag != "" && collision.CompareTag(target_tag) && collision.GetComponent<Health>() != null && (came_from.transform.parent == null || !collision.transform.IsChildOf(came_from.transform.parent)))
        {
            Instantiate(explosion, exploPos, Quaternion.identity);//, collision.transform);
            collision.GetComponent<Health>().hp -= dmg;
            if (collision.transform.parent != null && collision.transform.parent.GetComponent<NPCmovement>() != null)
            {
                var chance = Random.Range(0f, 1f);
                if (chance < .3f && collision.transform.parent.GetComponent<NPCmovement>() != null && came_from.transform.parent)
                {
                    collision.transform.parent.GetComponent<NPCmovement>().target = came_from.transform.parent.gameObject;
                }
                
            }
            Destroy(gameObject);
            
        }

        else if (collision.CompareTag("asteroid"))
        {
            Instantiate(explosion, exploPos, Quaternion.identity);//, collision.transform);
            Destroy(gameObject);
            
        }
    }
}
