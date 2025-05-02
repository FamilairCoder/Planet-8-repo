using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoShipCreator : MonoBehaviour
{
    public GameObject cargo, leftObj, rightObj;
    private List<GameObject> cargoList = new List<GameObject>();
    public List<GameObject> createdCargo = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        var amount = Random.Range(2, 8);
        for (int i = 0; i < amount; i++)
        {
            var c = Instantiate(cargo, transform.position, Quaternion.identity);
            if (i == 0) c.GetComponent<NPCCargoTow>().followObj = leftObj.transform;
            else c.GetComponent<NPCCargoTow>().followObj = cargoList[i - 1].transform;

            if (GetComponent<Despawn>() != null) c.AddComponent<Despawn>();
            cargoList.Add(c);
            createdCargo.Add(c);
        }
        cargoList.Clear();
        for (int i = 0; i < amount; i++)
        {
            var c = Instantiate(cargo, transform.position, Quaternion.identity);
            if (i == 0) c.GetComponent<NPCCargoTow>().followObj = rightObj.transform;
            else c.GetComponent<NPCCargoTow>().followObj = cargoList[i - 1].transform;

            if (GetComponent<Despawn>() != null) c.AddComponent<Despawn>();
            cargoList.Add(c);
            createdCargo.Add(c);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
