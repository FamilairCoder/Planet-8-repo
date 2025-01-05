using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoBuy : MonoBehaviour
{
    public ParticleSystem player_particles;
    public bool buy_ore;
    private float buy_time;
    private bool colliding;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (colliding)
        {
            buy_time -= Time.deltaTime;
            if (buy_time < 0 && PlayerMining.cargo_amount > 0)
            {
                if (PlayerMining.cargo_copper > 0) { PlayerMining.cargo_copper -= 1; HUDmanage.money += 3; }
                if (PlayerMining.cargo_iron > 0) { PlayerMining.cargo_iron -= 1; HUDmanage.money += 8; }
                if (PlayerMining.cargo_gold > 0) { PlayerMining.cargo_gold -= 1; HUDmanage.money += 15; }
                player_particles.Play();
                buy_time = .05f;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.CompareTag("player"))
        {
            colliding = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            colliding = false;
        }
    }
}
