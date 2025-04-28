using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipStats : MonoBehaviour
{
    public bool ignore_key, npc;
    public float spd, turning_spd;    
    private float slow_turning_spd, cvDelay = .05f;
    public GameObject ruin, core, trail, diagnosis, circuit_view;
    public Sprite ruin_sprite;
    public List<GameObject> thrusters = new List<GameObject>();
    public float total_hp, origHp;
    private bool madeLvl2, madelvl3, cvDid;
    [Header("Bonus")]
    public float armor_bonus;
    public float dmg_bonus, firerate_bonus, thrust_bonus, turnspd_bonus, cargo_bonus, mining_bonus, ore_bonus, bounty_bonus, energyRegenBonus, energyCapacityBonus;
    [Header("Upgraded parts")]
    public List<GameObject> lvl2Parts = new List<GameObject>();
    public List<GameObject> lvl2PartsDiagnosis = new List<GameObject>();

    public List<GameObject> lvl3Parts = new List<GameObject>();
    public List<GameObject> lvl3PartsDiagnosis = new List<GameObject>();
    [Header("Dont set---------")]
    public List<int> images = new List<int>();
    public List<int> filled = new List<int>();
    public float amount_broken, slow_spd;
    public bool boosting;
    private float boostspd = 40, prefsTime;
    // Start is called before the first frame update
    void Start()
    {


        if (!npc)
        {
            armor_bonus = PlayerPrefs.GetFloat("player armor_bonus", armor_bonus);
            dmg_bonus = PlayerPrefs.GetFloat("player dmg_bonus", dmg_bonus);
            firerate_bonus = PlayerPrefs.GetFloat("player firerate_bonus", firerate_bonus);
            thrust_bonus = PlayerPrefs.GetFloat("player thrust_bonus", thrust_bonus);
            turnspd_bonus = PlayerPrefs.GetFloat("player turnspd_bonus", turnspd_bonus);

            cargo_bonus = PlayerPrefs.GetFloat("player cargo_bonus", cargo_bonus);
            mining_bonus = PlayerPrefs.GetFloat("player mining_bonus", mining_bonus);
            ore_bonus = PlayerPrefs.GetFloat("player ore_bonus", ore_bonus);

            energyRegenBonus = PlayerPrefs.GetFloat("player energyRegen", energyRegenBonus);
            energyCapacityBonus = PlayerPrefs.GetFloat("player energyCapacity", energyCapacityBonus);

            //var a = Instantiate(diagnosis, new(0, 0, 0), Quaternion.identity, FindObjectOfType<Canvas>().transform);
            //a.GetComponent<RectTransform>().localPosition = new Vector3(-824, -400);
            //diagnosis = a;
            //for (int i = 0; i < diagnosis.transform.childCount; i++)
            //{
            //    var child = diagnosis.transform.GetChild(i);
            //    transform.GetChild(i).GetComponent<Health>().linked_diagnosis = child.gameObject;
            //}





        }
        slow_spd = spd / 5;
        slow_turning_spd = turning_spd / 5;
        //Debug.Log(total_hp);
        //Debug.Log(origHp);
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Health>() != null && transform.GetChild(i).gameObject.activeSelf)
            {
                //total_hp += transform.GetChild(i).GetComponent<Health>().hp;
                //origHp += transform.GetChild(i).GetComponent<Health>().hp;
            }
        }
        
        StartCoroutine(CalculateHealth());

    }

    // Update is called once per frame
    void Update()
    {
        cvDelay -= Time.deltaTime;
        if (!npc)
        {

            if (cvDelay <= 0 && !cvDid)
            {
                MenuScript[] allMenuScripts = FindObjectsOfType<MenuScript>();

                foreach (MenuScript menuScript in allMenuScripts)
                {
                    if (menuScript != null && !menuScript.dontSpawnCV)
                    {
                        var c = Instantiate(circuit_view, new(0, 0, 0), Quaternion.identity, menuScript.transform);
                        c.GetComponent<RectTransform>().localPosition = menuScript.circuit_view_pos;
                        circuit_view = c;
                        //c.GetComponent<CVstationLink>().ship_stats = gameObject;
                        c.GetComponent<CVstationLink>().station = menuScript.station;
                    }

                }
                cvDid = true;
            }
            prefsTime -= Time.deltaTime;
            if (prefsTime <= 0)
            {
                PlayerPrefs.SetFloat("player armor_bonus", armor_bonus);
                PlayerPrefs.SetFloat("player dmg_bonus", dmg_bonus);
                PlayerPrefs.SetFloat("player firerate_bonus", firerate_bonus);
                PlayerPrefs.SetFloat("player thrust_bonus", thrust_bonus);
                PlayerPrefs.SetFloat("player turnspd_bonus", turnspd_bonus);

                PlayerPrefs.SetFloat("player cargo_bonus", cargo_bonus);
                PlayerPrefs.SetFloat("player mining_bonus", mining_bonus);
                PlayerPrefs.SetFloat("player ore_bonus", ore_bonus);


                PlayerPrefs.SetFloat("player energyRegen", energyRegenBonus);
                PlayerPrefs.SetFloat("player energyCapacity", energyCapacityBonus);
                prefsTime = 1;
            }

            
            if (!madeLvl2 && HUDmanage.lvl >= 2)
            {
                foreach (GameObject p in lvl2Parts)
                {
                    p.SetActive(true);
                }
                foreach (GameObject p in lvl2PartsDiagnosis)
                {
                    p.SetActive(true);
                }
                lvl2PartsDiagnosis[0].transform.parent.transform.localScale = new Vector2(1.45f, 1.45f);
                lvl2PartsDiagnosis[0].transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(-824, -426.8f);

                var col = transform.parent.GetComponent<CircleCollider2D>();
                col.radius = .68f;
                col.offset = new(0, .21f);
                madeLvl2 = true;
            }
            if (!madelvl3 && HUDmanage.lvl >= 3)
            {
                foreach (GameObject p in lvl3Parts)
                {
                    p.SetActive(true);
                }
                foreach (GameObject p in lvl3PartsDiagnosis)
                {
                    p.SetActive(true);
                }
                lvl3PartsDiagnosis[0].transform.parent.transform.localScale = new Vector2(1.1f, 1.1f);


                var col = transform.parent.GetComponent<CircleCollider2D>();
                col.radius = .92f;
                col.offset = new(0, .21f);
                madelvl3 = true;
            }
            //var currentTotalHP = 0f;
            //for (int i = 0; i < transform.childCount; i++)
            //{
            //    if (transform.GetChild(i).GetComponent<Health>() != null && transform.GetChild(i).gameObject.activeSelf)
            //    {
            //        currentTotalHP += transform.GetChild(i).GetComponent<Health>().hp;
            //    }
            //}
            //if (currentTotalHP > total_hp) total_hp = currentTotalHP;

        }


        amount_broken = 0;
        var count = 0;
        foreach (var t in thrusters)
        {
            if (t.activeSelf)
            {
                count++;
                if (t.GetComponent<Health>() != null && t.GetComponent<Health>().hp <= 0)
                {
                    amount_broken++;
                }
            }

        }
        if (amount_broken > 0)
        {
            
            spd = Mathf.Lerp(spd, slow_spd, amount_broken / count);
            turning_spd = Mathf.Lerp(turning_spd, slow_turning_spd, amount_broken / count);
        }
        else
        {
            spd = slow_spd * 5;
            turning_spd = slow_turning_spd * 5;
        }
        if (npc)
        {
            if (boosting)
            {
                spd = slow_spd * boostspd;
                if (GetComponent<NPCmovement>().lvl == 3 && boostspd > 5) boostspd -= 5 * Time.deltaTime;
                else if (GetComponent<NPCmovement>().lvl == 2 && boostspd > 10) boostspd -= 10 * Time.deltaTime;
            }
            else
            {
                if ((boostspd < 40 && GetComponent<NPCmovement>().lvl == 2) || (boostspd < 50 && GetComponent<NPCmovement>().lvl == 3))
                {
                    boostspd += Time.deltaTime;
                }

            }
        }

            



        if (core != null && core.GetComponent<Health>() != null && core.GetComponent<Health>().hp <= 0)
        {
            if (!npc) PlayerMovement.dead = true;
            else
            {
                var a = Instantiate(ruin, transform.position, transform.rotation);
                a.transform.localScale = transform.localScale;
                a.GetComponent<Rigidbody2D>().angularVelocity = GetComponent<Rigidbody2D>().angularVelocity * 20;
                a.GetComponent<SpriteRenderer>().sprite = ruin_sprite;
                a.AddComponent<PolygonCollider2D>();
                Instantiate(core.GetComponent<Health>().death_explosion, transform.position, Quaternion.identity);

                if (GetComponent<NPCmovement>().retreat) PlayerBash.bash = false;
                PlayerPrefs.SetFloat(GetComponent<NPCmovement>().key + "alive", 0);

                if (GetComponent<NPCmovement>().giveBounty)
                {
                    HUDmanage.money += GetComponent<NPCmovement>().bounty_cost;
                    if (GetComponent<NPCmovement>().inSquad || GetComponent<NPCmovement>().pirateLeader)
                    {
                        HUDmanage.money += GetComponent<NPCmovement>().bounty_cost * 1.5f;
                    }
                }
                if (GetComponent<PatrolID>()  != null)
                {
                    PlayerPrefs.SetFloat("alive" + GetComponent<PatrolID>().id, 0);
                    PlayerPrefs.SetFloat("taken" + GetComponent<PatrolID>().id, 0);

                    DeleteChildHealthPrefs(GetComponent<PatrolID>().id);
                    
                }
                else if (GetComponent<NPCmovement>().is_pirate)
                {
                    DeleteChildHealthPrefs(GetComponent<NPCmovement>().key);
                }
                if (GetComponent<NPCmovement>().inSquad || GetComponent<NPCmovement>().pirateLeader)
                {
                    PlayerPrefs.SetFloat(GetComponent<NPCmovement>().squadKey + "alive", 0);
                }
                PlayerPrefs.Save();
                Destroy(gameObject);
            }
        }
        

    }

    private IEnumerator CalculateHealth()
    {
        while (true)
        {
            var h = 0f;
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<Health>() != null && transform.GetChild(i).gameObject.activeSelf)
                {
                    h += transform.GetChild(i).GetComponent<Health>().hp;
                }
            }
            total_hp = h;
            
            if (total_hp > origHp) { origHp = total_hp;  }
            yield return new WaitForSeconds(Random.Range(.25f, .75f));
        }
    }
    private void OnEnable()
    {
        StartCoroutine(CalculateHealth());
    }
    void DeleteChildHealthPrefs(string key)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            PlayerPrefs.DeleteKey(key + transform.GetChild(i).GetSiblingIndex() + "hp");
            PlayerPrefs.DeleteKey(key + transform.GetChild(i).GetSiblingIndex() + "orig_hp");
        }
    }
}
