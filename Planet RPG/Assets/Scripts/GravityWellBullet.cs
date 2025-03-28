using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWellBullet : MonoBehaviour
{
    public Sprite activatedSpr;
    public Vector3 dir;
    private float spd, lifeTime = 10f;
    private bool played;
    public List<Rigidbody2D> caught = new List<Rigidbody2D>();
    public bool isPirate;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        transform.position += spd * Time.deltaTime * dir;
        if (lifeTime > 9)
        {
            spd = Mathf.Lerp(0, 30, (lifeTime - 9) / 1);
        }
        else
        {
            spd = 0;
            GetComponent<SpriteRenderer>().sprite = activatedSpr;
            if (!played)
            {
                transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                played = true;
            }
            if (lifeTime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < caught.Count; ++i)
        {
            if (caught[i] != null)
            {
                Vector3 caughtDir = (new Vector2(transform.position.x, transform.position.y) - caught[i].position).normalized;
                caught[i].AddForce(caughtDir * 100, ForceMode2D.Force);
                if (caught[i].GetComponent<NPCmovement>() != null) caught[i].GetComponent<NPCmovement>().gravityWellCaught = true;
                else if (caught[i].GetComponent<PlayerMovement>() != null) caught[i].GetComponent<PlayerMovement>().gravityWellCaught = true;
            }

        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (lifeTime < 9 && collision.GetComponent<Rigidbody2D>() != null && collision.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Dynamic && !caught.Contains(collision.gameObject.GetComponent<Rigidbody2D>()))
        {
            if ((!isPirate && collision.gameObject.CompareTag("enemy")) || (isPirate && HUDmanage.pirateTags.Contains(collision.gameObject.tag)))
            {
                caught.Add(collision.gameObject.GetComponent<Rigidbody2D>());
            }
            
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Rigidbody2D>() != null && collision.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Dynamic && caught.Contains(collision.GetComponent<Rigidbody2D>()))
        {

            caught.Remove(collision.GetComponent<Rigidbody2D>());

            if (collision.GetComponent<NPCmovement>() != null) collision.GetComponent<NPCmovement>().gravityWellCaught = false;
            else if (collision.GetComponent<PlayerMovement>() != null) collision.GetComponent<PlayerMovement>().gravityWellCaught = false;
            
        }
    }
}
