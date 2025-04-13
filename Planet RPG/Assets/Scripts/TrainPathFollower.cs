using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
public class TrainPathFollower : MonoBehaviour
{
    public PathCreator pathCreator;
    public float spd, displace;
    float distanceTraveled;
    public TrainPathFollower headTrain;

    [Header("Draft stuf----------")]
    public bool draft;
    public List<Transform> cars = new List<Transform>();
    public List<Rigidbody2D> caught = new List<Rigidbody2D>();

    Vector3 lastPosition;
    Vector2 velocity;
    // Start is called before the first frame update
    void Start()
    {
        lastPosition = pathCreator.path.GetPointAtDistance(distanceTraveled - displace);
    }

    // Update is called once per frame
    void Update()
    {
        lastPosition = transform.position;
        distanceTraveled += spd * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTraveled - displace);
        var pathRot = pathCreator.path.GetRotationAtDistance(distanceTraveled - displace);
        transform.rotation = pathRot;

        foreach(var a in caught)
        {
            if (a == null) continue;
            a.transform.position += (transform.position - lastPosition);
        }
    }
    private void FixedUpdate()
    {
/*        velocity = ((Vector2)transform.position - lastPosition) / Time.fixedDeltaTime;
        lastPosition = transform.position;

        foreach (var a in caught)
        {
            if (a == null) continue;

            var dir = ((a.transform.position + transform.forward) - a.transform.position).normalized;
            a.AddForce(dir * 500, ForceMode2D.Force);
            
        }*/

/*        Vector2 rawVelocity = ((Vector2)transform.position - lastPosition) / Time.fixedDeltaTime;
        velocity = Vector2.Lerp(velocity, rawVelocity, 0.1f);
        lastPosition = transform.position;

        foreach (var a in caught)
        {
            if (a == null) continue;

            // Only pull objects if close enough
            float distance = Vector2.Distance(transform.position, a.position);
            if (distance > 60f) continue;

            // Pull them in the direction of the train's movement
            Vector2 velocityDiff = velocity - a.velocity;

            // Optional: scale the force by distance to smooth things
            float distanceFactor = Mathf.Clamp01(distance / 60f);
            a.AddForce(velocityDiff * (1f - distanceFactor) * 20f, ForceMode2D.Force);
        }*/
    }
    Vector2 GetClosestCarPosition(Vector2 target)
    {
        float minDist = float.MaxValue;
        Vector2 closest = Vector2.zero;

        foreach (Transform car in cars)
        {
            float d = Vector2.Distance(target, car.position);
            if (d < minDist)
            {
                minDist = d;
                closest = car.position;
            }
        }

        return closest;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (draft)
        {
            if (collision.gameObject.GetComponent<Rigidbody2D>() != null)
            {
                if (headTrain != null && !headTrain.caught.Contains(collision.gameObject.GetComponent<Rigidbody2D>()))
                {
                    headTrain.caught.Add(collision.gameObject.GetComponent<Rigidbody2D>());
                }
                else if (headTrain == null && !caught.Contains(collision.gameObject.GetComponent<Rigidbody2D>()))
                {
                    caught.Add(collision.gameObject.GetComponent<Rigidbody2D>());
                }

                if (collision.gameObject.GetComponent<PlayerMovement>() != null)
                {
                    collision.gameObject.GetComponent<PlayerMovement>().gravityWellCaught = true;
                }
            }
/*            else if (a.gameObject.GetComponent<Bullet>())
            {

            }*/
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (draft)
        {
            if (collision.gameObject.GetComponent<Rigidbody2D>() != null)
            {
                if (headTrain != null && headTrain.caught.Contains(collision.gameObject.GetComponent<Rigidbody2D>()))
                {
                    headTrain.caught.Remove(collision.gameObject.GetComponent<Rigidbody2D>());
                }
                else if (headTrain == null && caught.Contains(collision.gameObject.GetComponent<Rigidbody2D>()))
                {
                    caught.Remove(collision.gameObject.GetComponent<Rigidbody2D>());
                }

                if (collision.gameObject.GetComponent<PlayerMovement>() != null)
                {
                    collision.gameObject.GetComponent<PlayerMovement>().gravityWellCaught = false;
                }
            }
            /*            else if (a.gameObject.GetComponent<Bullet>())
                        {

                        }*/
        }
    }

}
