using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckClumpScript : MonoBehaviour
{
    private float hp = 20;
    public GameObject explosion, recodObj, photonObj;
    public Sprite recordSpr, photonSpr;
    private string key;
    private bool rec, photon;
    // Start is called before the first frame update
    void Start()
    {
        
        var scal = transform.localScale.x * Random.Range(1.5f, .75f);
        transform.localScale = new Vector3(scal, scal, scal);
        transform.localRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        
        key = transform.parent.name + gameObject.name;
        var chance = PlayerPrefs.GetFloat(key + "chance", Random.Range(0f, 1f));
        if (chance < .75f)
        {
            photon = true;
            GetComponent<SpriteRenderer>().sprite = photonSpr;
        }
        else
        {
            rec = true;
            GetComponent<SpriteRenderer>().sprite = recordSpr;
        }
        if (PlayerPrefs.GetInt(key + "mined", 0) == 1)
        {
            if (rec)
            {
                var made = Instantiate(recodObj, transform.position, Quaternion.identity);
                made.transform.localRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
            }

            Destroy(gameObject);
        }


        PlayerPrefs.SetFloat(key + "chance", chance);
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {

        if (hp <= 0)
        {
            Destroy(gameObject);
            Instantiate(explosion, transform.position, Quaternion.identity);
            GameObject made = null;
            if (rec) made = Instantiate(recodObj, transform.position, Quaternion.identity);
            else if (photon) made = Instantiate(photonObj, transform.position, Quaternion.identity);
            if (made != null) { made.transform.localRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)); }
            PlayerPrefs.SetInt(key + "mined", 1);
            PlayerPrefs.Save();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Bullet>() != null)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            hp -= .5f;
        }
    }

}
