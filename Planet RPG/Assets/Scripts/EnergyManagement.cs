using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnergyManagement : MonoBehaviour
{
    public ShipStats stats;
    public static float energy;
    public TextMeshProUGUI text;
    public RectTransform bar;
    private HUDmanage HUD;
    private Vector3 activePos;
    // Start is called before the first frame update
    void Start()
    {
        energy = SaveManager.GetFloat("player energy", 100);
        HUD = FindObjectOfType<HUDmanage>();
        activePos = GetComponent<RectTransform>().anchoredPosition;
        StartCoroutine(EnergyRegen());
    }

    // Update is called once per frame
    void Update()
    {
        if (HUD.code_has_secondary.Count > 0)
        {
            GetComponent<RectTransform>().anchoredPosition = activePos;
            text.text = Mathf.Round(energy) + "/" + (100 + stats.energyCapacityBonus).ToString();
            var x = Mathf.Lerp(0, 0.84528f, energy / (100 + stats.energyCapacityBonus));
            bar.localScale = new Vector3(x, 1);
            energy = Mathf.Clamp(energy, 0, (100 + stats.energyCapacityBonus));
        }
        else
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -1000);
        }
    }

    IEnumerator EnergyRegen()
    {
        while (true)
        {
            if (energy < (100 + stats.energyCapacityBonus))
            {
                energy += 1 + stats.energyRegenBonus;
            }
            energy = Mathf.Clamp(energy, 0, (100 + stats.energyCapacityBonus));
            SaveManager.SetFloat("player energy", energy);
            yield return new WaitForSeconds(1);
        }
    }
}
