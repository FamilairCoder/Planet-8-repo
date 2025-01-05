using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class SaveData : MonoBehaviour
{

    public bool ship, ship_save, ship_load, add_ship, player;
    public SavedShips ships = new SavedShips();
    private float save_time;

    private void Awake()
    {
        if (ship && System.IO.File.Exists(Application.persistentDataPath + "/spawnedShips.json")) ShipLoadFromJson();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        save_time -= Time.deltaTime;
        if (save_time < 0 && ship)
        {
            SaveCurrentShips();
            save_time = 1f;
        }

        if (ship_save)
        {
            ShipSaveToJson();
            ship_save = false;
        }
        if (ship_load)
        {
            ShipLoadFromJson();
            ship_load = false;
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ShipClear();
        }

    }
    public void SaveCurrentShips()
    {
        var spawned_ships = GetComponent<ShipSpawner>().spawned_ships;
        var data = ships.saved_ships;

        int numb = 0;
        foreach (var s in spawned_ships)
        {

            
            data[numb].position = s.transform.position;
            data[numb].rotation = s.transform.rotation;
            data[numb].ship_type = GetComponent<ShipSpawner>().ship_types[numb];

            
            data[numb].armor_bonus = GetComponent<ShipSpawner>().armor_bonuses[numb];
            data[numb].dmg_bonus = s.GetComponent<ShipStats>().dmg_bonus;
            data[numb].firerate_bonus = s.GetComponent<ShipStats>().firerate_bonus;
            data[numb].thrust_bonus = s.GetComponent<ShipStats>().thrust_bonus;
            data[numb].turnspd_bonus = s.GetComponent<ShipStats>().turnspd_bonus;

                
            

            numb++;
        }
        ShipSaveToJson();
    }



    public void ShipSaveToJson()
    {
        string sd = JsonUtility.ToJson(ships);
        string filePath = Application.persistentDataPath + "/spawnedShips.json";
        //Can add debug.log(filePath) to find where is saved to
        System.IO.File.WriteAllText(filePath, sd);
    }

    public void ShipLoadFromJson()
    {
        string filePath = Application.persistentDataPath + "/spawnedShips.json";
        string sd = System.IO.File.ReadAllText(filePath);

        ships = JsonUtility.FromJson<SavedShips>(sd);
    }
    void ShipClear()
    {
        ships = new SavedShips();

        // Remove the saved file
        string filePath = Application.persistentDataPath + "/spawnedShips.json";
        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
            Debug.Log("Cleared saved data and deleted file: " + filePath);
        }
    }


/*    public void AddShip()
    {
        ShipData newShip = new ShipData();
        ships.saved_ships.Add(newShip);
        ShipSaveToJson();
    }*/

}

[System.Serializable]
public class SavedShips
{
    public List<ShipData> saved_ships = new List<ShipData>();
}

[System.Serializable]
public class ShipData
{
    public Vector2 position;
    public Quaternion rotation;
    public int ship_type;
    public float armor_bonus, dmg_bonus, firerate_bonus, thrust_bonus, turnspd_bonus;
}


[System.Serializable]
public class PlayerStats
{
    public Vector2 position;
    public Quaternion rotation;
    public int ship_index;
    public float money, armor_bonus, dmg_bonus, firerate_bonus, thrust_bonus, turnspd_bonus;
    public List<float> hp_points = new List<float>();
}

