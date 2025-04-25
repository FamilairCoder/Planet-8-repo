using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //public AudioClip hitSFX;
    public GameObject explosion, came_from;
    public float spd, dmg;
    private float lifetime = 2, torpedoSpd;
    public string target_tag;
    public bool missile;
    public float turn_spd;
    public GameObject target;
    private Vector3 nextPos;
    private bool collided;
    public bool playerMade, patrolMade, pirateAccumulate, torpedo;
    public bool isPirate;
    [Header("For emp stuff-------------------")]
    public bool emp;
    public GameObject empParticles;
    public Material empMat;
    
    // Start is called before the first frame update
    void Start()
    {
        if (emp)
        {
            GetComponent<ParticleSystemRenderer>().material = empMat;
            GetComponent<ParticleSystemRenderer>().trailMaterial = empMat;
        }
        if (missile) lifetime = 8;
        if (torpedo)
        {
            torpedoSpd = 15f;
            RaycastHit2D[] hit = null;
            if (!isPirate) hit = Physics2D.RaycastAll(transform.position, transform.up, 50, LayerMask.GetMask("pirates"));
            else hit = Physics2D.RaycastAll(transform.position, transform.up, 50);
            foreach (var a in hit)
            {
                var col = a.collider;
                if (col.GetComponent<Health>() != null && col.GetComponent<Health>().hp > 0 && (came_from.transform.parent == null || !col.gameObject.transform.IsChildOf(came_from.transform.parent)))
                {
                    if (!isPirate || HUDmanage.pirateTags.Contains(col.tag))
                    {
                        target = col.gameObject;
                        break;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (missile) spd = Mathf.Lerp(60, torpedoSpd, lifetime / 8);
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

        BulletMove(transform.position + transform.up * spd * Time.deltaTime);



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
            if (collision.CompareTag(target_tag) || (came_from.GetComponent<StationWeapon>() != null && came_from.GetComponent<StationWeapon>().isPirate && HUDmanage.pirateTags.Contains(collision.tag)) || (came_from.transform.parent != null && CheckPirateTags(collision)))
            {
                Instantiate(explosion, exploPos, Quaternion.identity);//, collision.transform);
                collision.GetComponent<Health>().hp -= dmg;
                TriggerPlayerEMP(collision);
                if (collision.transform.parent != null && collision.transform.parent.GetComponent<NPCmovement>() != null)
                {
                    
                    var chance = Random.Range(0f, 1f);
                    if (playerMade)
                    {
                        collision.transform.parent.GetComponent<NPCmovement>().attackedByPlayer = true;
                        if (chance < .3f && collision.transform.parent.GetComponent<NPCmovement>() != null && came_from.transform.parent)
                        {
                            collision.transform.parent.GetComponent<NPCmovement>().target = came_from.transform.parent.gameObject;
                        }
                    }

                    if (chance < .3f && collision.transform.parent.GetComponent<NPCmovement>() != null && came_from.transform.parent)
                    {
                        collision.transform.parent.GetComponent<NPCmovement>().target = came_from.transform.parent.gameObject;
                    }
                    TriggerEMP(collision);
                }
                Destroy(gameObject);
            }

            
        }

        else if (collision.CompareTag("asteroid") || collision.CompareTag("wreck") || collision.CompareTag("big_wreck"))
        {
            Instantiate(explosion, exploPos, Quaternion.identity);//, collision.transform);
            Destroy(gameObject);
            
        }
    }
    
    
    public void BulletMove(Vector3 next)
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(next, transform.up, spd * Time.deltaTime);
        foreach (var c in hit)
        {
            //Debug.Log(c);
            if (came_from != null && target_tag != "" && target_tag != null && (came_from.transform.parent == null || !c.collider.gameObject.transform.IsChildOf(came_from.transform.parent)) && c.collider.gameObject.GetComponent<Health>() != null && c.collider.gameObject.GetComponent<Health>().hp > 0)
            {
                if (c.collider.CompareTag(target_tag) || (came_from.GetComponent<StationWeapon>() != null && came_from.GetComponent<StationWeapon>().isPirate && HUDmanage.pirateTags.Contains(c.collider.tag)) || (came_from.transform.parent != null && CheckPirateTags(c)))
                {

                    Instantiate(explosion, c.point, Quaternion.identity);//, collision.transform);
                    c.collider.GetComponent<Health>().hp -= dmg;
                    TriggerPlayerEMP(c.collider);
                    if (c.collider.transform.parent != null && c.collider.transform.parent.GetComponent<NPCmovement>() != null)
                    {
                        TriggerEMP(c.collider);
                        if (playerMade)
                        {
                            //Debug.Log("player made");
                            c.collider.transform.parent.GetComponent<NPCmovement>().attackedByPlayer = true;
                            var chance = Random.Range(0f, 1f);
                            if (chance < .3f)// && (came_from.transform.parent != null ))
                            {
                                c.collider.transform.parent.GetComponent<NPCmovement>().target = HUDmanage.playerReference.transform.GetChild(0).transform.GetChild(0).gameObject;
                            }
                        }
                        else if (patrolMade)
                        {
                            c.collider.transform.parent.GetComponent<NPCmovement>().attackedByPlayer = true;
                        }
                        var chancea = Random.Range(0f, 1f);
                        if (chancea < .3f && c.collider.transform.parent.GetComponent<NPCmovement>() != null && came_from.transform.parent != null)
                        {
                            c.collider.transform.parent.GetComponent<NPCmovement>().target = came_from.transform.parent.gameObject;
                        }

                    }
                    Destroy(gameObject);
                    break;
                }

            }

        }

        transform.position = next;
    }
    bool CheckPirateTags(RaycastHit2D c)
    {
        if (came_from.transform.parent.GetComponent<NPCmovement>() != null && came_from.transform.parent.GetComponent<NPCmovement>().is_pirate && HUDmanage.pirateTags.Contains(c.collider.tag)) return true;
        else return false;
    }
    bool CheckPirateTags(Collider2D c)
    {
        if (came_from.transform.parent.GetComponent<NPCmovement>() != null && came_from.transform.parent.GetComponent<NPCmovement>().is_pirate && HUDmanage.pirateTags.Contains(c.tag)) return true;
        else return false;
    }

    void TriggerEMP(Collider2D collision)
    {
        if (emp)
        {

            if (collision.transform.parent.GetComponent<NPCmovement>().empParticle == null)
            {
                var p = Instantiate(empParticles, collision.transform.position, Quaternion.identity);
                if (!isPirate || collision.transform.parent.GetComponent<NPCmovement>().is_patrol)
                {
                    collision.transform.parent.GetComponent<NPCmovement>().empParticle = p;
                    p.GetComponent<ParticleStayOn>().stayOn = collision.transform.parent.gameObject;
                }

                p.GetComponent<ParticleStayOn>().mat = empMat;
            }

            if (!isPirate || collision.transform.parent.GetComponent<NPCmovement>().is_patrol)
            {
                collision.transform.parent.GetComponent<NPCmovement>().stunTime += 1;
            }




        }
    }
    void TriggerPlayerEMP(Collider2D collision)
    {
        if (emp)
        {
            if (collision.transform.parent != null && collision.transform.parent.transform.parent != null)
            {
                var parentParent = collision.transform.parent.transform.parent;
                if (parentParent.GetComponent<PlayerWeapon>() != null)
                {
                    if (parentParent.GetComponent<PlayerWeapon>().empParticle == null)
                    {
                        var p = Instantiate(empParticles, collision.transform.position, Quaternion.identity);
                        p.GetComponent<ParticleStayOn>().stayOn = collision.transform.parent.transform.parent.gameObject;
                        parentParent.GetComponent<PlayerWeapon>().empParticle = p;
                        p.GetComponent<ParticleStayOn>().mat = empMat;
                    }
                    parentParent.GetComponent<PlayerWeapon>().empTime += 1;


                }
            }

        }
    }

}
