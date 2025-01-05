using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerMining : MonoBehaviour
{
    [SerializeField] LayerMask asteroid_mask = default;
    public HUDmanage HUD;
    public GameObject mining_sparks, iron_ore, copper_ore, gold_ore, cargo_hold_text, ore_scan;
    public float mining_range, mining_strength;
    public List<string> mining_tags = new List<string>();
    public List<Sprite> ore_scan_sprites = new List<Sprite>();
    public List<Color> ore_scan_colors = new List<Color>();

    public static float cargo_amount, cargo_copper, cargo_iron, cargo_gold, cargo_capacity;
    private float mine_time, scan_time, second_ore;
    private bool made_sparks;
    private GameObject hit_asteroid;
    public GameObject created_sparks, mining_probe, probe_space_text, probe_ast_text;
    public ParticleSystem ore_scanner, probe_ore;
    private GameObject touching_asteroid, error_text_space, error_text_ast;
    public static bool touching_probe;
    private float deltaTime = 0.0f, saveTime;
    // Start is called before the first frame update
    void Start()
    {
        cargo_capacity = PlayerPrefs.GetFloat("cargo_capacity", 20);
        cargo_copper = PlayerPrefs.GetFloat("cargo_copper", 0);
        cargo_gold = PlayerPrefs.GetFloat("cargo_gold", 0);
        cargo_iron = PlayerPrefs.GetFloat("cargo_iron", 0);
    }

    // Update is called once per frame
    void Update()
    {

        //deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        //Debug.Log(Mathf.Ceil(1.0f / deltaTime));

        var stats = transform.GetChild(0).GetComponent<ShipStats>();
        cargo_capacity = 20 + stats.cargo_bonus;
        mining_strength = .5f - stats.mining_bonus;
        second_ore = 0 + stats.ore_bonus;

        cargo_amount = cargo_copper + cargo_gold + cargo_iron;
        
        if (saveTime < 0)
        {
            PlayerPrefs.SetFloat("cargo_copper", cargo_copper);
            PlayerPrefs.SetFloat("cargo_gold", cargo_gold);
            PlayerPrefs.SetFloat("cargo_iron", cargo_iron);
            PlayerPrefs.SetFloat("cargo_capacity", cargo_capacity);
            PlayerPrefs.Save();
            saveTime = 1f;
        }
        
        
        scan_time -= Time.deltaTime;
        saveTime -= Time.deltaTime;

        if (cargo_hold_text != null) { cargo_hold_text.GetComponent<TextMeshProUGUI>().text = Mathf.Round(cargo_amount).ToString() + "/" + Mathf.Round(cargo_capacity).ToString(); }
        if (Input.GetMouseButton(1) && HUD.code_has_secondary.Count > 0)
        {
            if (HUD.code_has_secondary[HUD.index].name == "MiningLaserUI") //mining laser
            {
                GetComponent<AudioSource>().enabled = true;
                if (!made_sparks)
                {
                    created_sparks = Instantiate(mining_sparks, transform.position, Quaternion.identity);
                    made_sparks = true;
                }
                var lr = GetComponent<LineRenderer>();
                lr.enabled = true;
                var cast = Physics2D.RaycastAll(transform.position, transform.up, mining_range);
                Vector3 hit = new(0, 0, -5);
                hit_asteroid = null;
                foreach (var c in cast)
                {
                    if (mining_tags.Contains(c.transform.tag))
                    {
                        hit = c.point;
                        hit_asteroid = c.collider.gameObject;
                    }

                }


                lr.SetPosition(0, transform.position);
                if (hit != new Vector3(0, 0, -5) && hit_asteroid.GetComponent<AsteroidInfo>().has_ore)
                {
                    lr.SetPosition(1, new(hit.x, hit.y, -5f));
                    created_sparks.transform.position = hit;

                    if (hit_asteroid.CompareTag("asteroid"))
                    {
                        hit_asteroid.transform.localScale -= new Vector3(.25f, .25f) * Time.deltaTime;
                        if (hit_asteroid.transform.localScale.x < .25f)
                        {
                            Destroy(hit_asteroid);
                        }

                        mine_time -= Time.deltaTime;
                        if (mine_time <= 0)
                        {
                            var chance = Random.Range(0f, 1f);
                            if (chance < .50f && hit_asteroid.GetComponent<AsteroidInfo>().has_ore)
                            {
                                for (int i = 0; i < 1; i++)
                                {
                                    if (hit_asteroid.GetComponent<AsteroidInfo>().copper) CreateOre(copper_ore, hit);
                                    else if (hit_asteroid.GetComponent<AsteroidInfo>().iron) CreateOre(iron_ore, hit);
                                    else if (hit_asteroid.GetComponent<AsteroidInfo>().gold) CreateOre(gold_ore, hit);
                                    var ore_chance = Random.Range(0f, 1f);
                                    if (ore_chance > second_ore)
                                    {
                                        break;
                                    }
                                }

                            }
                            mine_time = mining_strength;
                        }
                    }

                }
                else
                {
                    var pos = transform.position + transform.up * mining_range;
                    lr.SetPosition(1, new(pos.x, pos.y, -5f)); 
                    created_sparks.transform.position = transform.position + transform.up * mining_range;
                }
            }


        }
        else if (GetComponent<LineRenderer>() != null)
        {
            if (created_sparks != null) { Destroy(created_sparks); made_sparks = false; }
            var lr = GetComponent<LineRenderer>();
            lr.enabled = false;
            GetComponent<AudioSource>().enabled = false;

        }

        if (Input.GetMouseButtonDown(1) && HUD.code_has_secondary.Count > 0)
        {
            if (HUD.code_has_secondary[HUD.index].name == "OreScannerUI" && scan_time <= 0)
            {
                var hit = Physics2D.OverlapCircleAll(transform.position, 40, asteroid_mask);
                foreach (var ast in hit)
                {
                    if (!ast.GetComponent<AsteroidInfo>().has_ore && !ast.GetComponent<AsteroidInfo>().has_detected)
                    {
                        
                        var scan = Instantiate(ore_scan, ast.transform.position, Quaternion.identity);
                        if (ast.GetComponent<AsteroidInfo>().copper)
                        {
                            scan.GetComponent<SpriteRenderer>().sprite = ore_scan_sprites[0];
                            scan.transform.GetChild(0).GetComponent<TextMeshPro>().color = ore_scan_colors[0];
                        }
                        else if (ast.GetComponent<AsteroidInfo>().iron)
                        {
                            scan.GetComponent<SpriteRenderer>().sprite = ore_scan_sprites[1];
                            scan.transform.GetChild(0).GetComponent<TextMeshPro>().color = ore_scan_colors[1];
                        }
                        else if (ast.GetComponent<AsteroidInfo>().gold)
                        {
                            scan.GetComponent<SpriteRenderer>().sprite = ore_scan_sprites[2];
                            scan.transform.GetChild(0).GetComponent<TextMeshPro>().color = ore_scan_colors[2];
                        }
                        scan.GetComponent<ScanStayOn>().asteroid = ast.gameObject;
                        ast.GetComponent<AsteroidInfo>().has_detected = true;

                    }
                    

                }

                ore_scanner.Play();
                scan_time = 2;
            }
            if (HUD.code_has_secondary[HUD.index].name == "MiningProbeUI" && ProbeUIScript.probe_amount > 0 && touching_asteroid != null && !touching_probe)
            {
                ProbeUIScript.probe_amount -= 1;

                var dir = (touching_asteroid.transform.position - transform.position).normalized;
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                
                var p = Instantiate(mining_probe, transform.position, Quaternion.AngleAxis(angle + 90, Vector3.forward));
                p.transform.parent = touching_asteroid.transform;
                if (touching_asteroid.GetComponent<AsteroidInfo>().copper) p.GetComponent<MiningProbeLogic>().copper = true;
                if (touching_asteroid.GetComponent<AsteroidInfo>().iron) p.GetComponent<MiningProbeLogic>().iron = true;
                if (touching_asteroid.GetComponent<AsteroidInfo>().gold) p.GetComponent<MiningProbeLogic>().gold = true;
            }
            else if (HUD.code_has_secondary[HUD.index].name == "MiningProbeUI" && ProbeUIScript.probe_amount > 0 && touching_asteroid != null && touching_probe && error_text_space == null)
            {
                error_text_space = Instantiate(probe_space_text, transform.position, Quaternion.identity);
                Destroy(error_text_ast);
            }
            else if (HUD.code_has_secondary[HUD.index].name == "MiningProbeUI" && ProbeUIScript.probe_amount > 0 && touching_asteroid == null && error_text_ast == null)
            {
                error_text_ast = Instantiate(probe_ast_text, transform.position, Quaternion.identity);
                Destroy(error_text_space);
            }
        }


    }
    void CreateOre(GameObject ore, Vector3 spawn_pos)
    {
        var o = Instantiate(ore, spawn_pos, Quaternion.Euler(0, 0, Random.Range(0, 360f)));
        var dir = (transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) - spawn_pos).normalized;
        o.GetComponent<Rigidbody2D>().AddForce(dir * 10, ForceMode2D.Impulse);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("asteroid") && !collision.gameObject.GetComponent<AsteroidInfo>().has_ore)
        {
            touching_asteroid = collision.gameObject;
        }
  
        if (collision.gameObject.CompareTag("ore") && cargo_amount < cargo_capacity)
        {
            var name = collision.gameObject.name;
            if (name == "CopperOre(Clone)")
            {
                cargo_copper += 1;
            }
            if (name == "IronOre(Clone)")
            {
                cargo_iron += 1;
            }
            if (name == "GoldOre(Clone)")
            {
                cargo_gold += 1;
            }
            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("asteroid") && !collision.gameObject.GetComponent<AsteroidInfo>().has_ore)
        {
            touching_asteroid = null;
        }
    }
}
