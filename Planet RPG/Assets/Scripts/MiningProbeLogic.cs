using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningProbeLogic : MonoBehaviour
{
    public float ore_amount, ore_capacity, ore_time;
    public bool copper, iron, gold;
    private float give_time;
    private bool touching, taking;
    public GameObject particle1, particle2;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMining>().gameObject;
        ore_capacity = 25;
    }

    // Update is called once per frame
    void Update()
    {
        ore_time -= Time.deltaTime;
        give_time -= Time.deltaTime;
        if (ore_time <= 0 && ore_amount < ore_capacity && !taking)
        {
            
            ore_amount++;
            ore_time = 30;
        }
        else if (ore_amount >= ore_capacity)
        {
            transform.GetChild(0).GetComponent<Animator>().enabled = false;
            particle1.SetActive(false);
            particle2.SetActive(false);
            taking = true;

        }
        else if (ore_amount < ore_capacity)
        {
            transform.GetChild(0).GetComponent<Animator>().enabled = true;
            particle1.SetActive(true);
            particle2.SetActive(true);

        }
        if (ore_amount == 0 || PlayerMining.cargo_amount >= PlayerMining.cargo_capacity)
        {
            taking = false;
        }

        if (give_time <= 0 && touching && taking)
        {
            
            if (copper) PlayerMining.cargo_copper++;
            if (iron) PlayerMining.cargo_iron++;
            if (gold) PlayerMining.cargo_gold++;
            ore_amount--;
            player.GetComponent<PlayerMining>().probe_ore.Play();
            give_time = .1f;
        }

    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            PlayerMining.touching_probe = true;
            touching = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            PlayerMining.touching_probe = false;
            touching = false;
            taking = false;
        }
    }
}
