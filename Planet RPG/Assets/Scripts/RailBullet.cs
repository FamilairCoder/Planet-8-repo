using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailBullet : MonoBehaviour
{
    public GameObject particle, hitExplosion;
    [Header("dont set-------------------")]
    public float dmg;
    public Material mat;    
    public GameObject cameFrom;
    public bool isPirate;
    private float dist = 5;
    private bool hitSomething;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(HitRoutine());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator HitRoutine()
    {
        while (dist <= 100 && dmg > 0)
        {
            RaycastHit2D[] hit = null;
            if (!isPirate) { hit = Physics2D.RaycastAll(transform.position, transform.up, 5, LayerMask.GetMask("pirates")); }
            else hit = Physics2D.RaycastAll(transform.position, transform.up, 5);
            foreach (var a in hit)
            {
                
                var col = a.collider;
                if (cameFrom != null && (cameFrom.transform.parent == null || !col.gameObject.transform.IsChildOf(cameFrom.transform.parent)) && col.GetComponent<Health>() != null && col.GetComponent<Health>().hp > 0)
                {
                    if (!isPirate || HUDmanage.pirateTags.Contains(col.tag))
                    {
                        Instantiate(hitExplosion, a.point, Quaternion.identity);
                        col.GetComponent<Health>().hp -= dmg;
                        dmg -= .25f;
                    }
                }

            }

            var p = Instantiate(particle, transform.position + transform.up * 5, Quaternion.identity);
            p.GetComponent<ParticleSystemRenderer>().material = mat;
            p.GetComponent<ParticleSystemRenderer>().trailMaterial = mat;
            
            transform.position += transform.up * 5;
            dist += 5;
            yield return new WaitForSeconds(.005f);

        }
        
    }
}
