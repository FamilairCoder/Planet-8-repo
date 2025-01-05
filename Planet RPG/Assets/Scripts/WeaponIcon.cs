using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIcon : MonoBehaviour
{
    public GameObject highlight;
    public bool basic_laser, laser_rod, laser_beam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((basic_laser && PlayerWeapon.basic_laser) || (laser_rod && PlayerWeapon.laser_rod) || (laser_beam && PlayerWeapon.laser_beam))
        {
            highlight.SetActive(true);
        }

        else {  highlight.SetActive(false); }
    }
}
