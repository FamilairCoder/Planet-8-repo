using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldStayOn : MonoBehaviour
{
    public GameObject stayOn;
    private float drainTime;
    public bool isPirate;
    // Start is called before the first frame update
    void Start()
    {
        if (stayOn.GetComponent<Collider2D>() != null)
        {
            var scal = stayOn.GetComponent<Collider2D>().bounds.size.x * 1.3f;

            transform.localScale = new Vector3(scal, scal);
        }
    }

    // Update is called once per frame
    void Update()
    {
        drainTime -= Time.deltaTime;
        if (drainTime < 0)
        {
            if (!isPirate) EnergyManagement.energy -= 4; 
            drainTime = .5f;
        }
        if (stayOn != null && (isPirate || EnergyManagement.energy >= 4))
        {
            transform.position = stayOn.transform.position;
        }
        else if (isPirate || EnergyManagement.energy < 4)
        {
            Destroy(gameObject);
            if (!isPirate) PlayerWeapon.shieldTime = 5f;
        }
    }
}
