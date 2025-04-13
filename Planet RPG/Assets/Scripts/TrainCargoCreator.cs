using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainCargoCreator : MonoBehaviour
{
    public GameObject cargo;
    public float amount;
    // Start is called before the first frame update
    void Start()
    {
        var displacement = 18f;
        for (int i = 0; i < amount; i++)
        {
            var c = Instantiate(cargo, transform.position, Quaternion.identity);
            c.GetComponent<TrainPathFollower>().displace = displacement;
            c.GetComponent<TrainPathFollower>().spd = GetComponent<TrainPathFollower>().spd;
            c.GetComponent<TrainPathFollower>().pathCreator = GetComponent<TrainPathFollower>().pathCreator;
            c.GetComponent<TrainPathFollower>().headTrain = GetComponent<TrainPathFollower>();
            GetComponent<TrainPathFollower>().cars.Add(c.gameObject.transform);
            displacement += 23f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
