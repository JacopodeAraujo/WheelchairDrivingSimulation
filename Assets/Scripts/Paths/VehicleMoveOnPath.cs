using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMoveOnPath : MonoBehaviour
{
    public EditorPath pathToFollow;
    public int currentWayPointIndex;
    private float speed;
    public float cornerSpeed;
    public float regSpeed;
    public float reachDistance = 2.0f;
    public float reachDistance2 = 20.0f;
    public float rotationSpeed = 15.0f;
    public string pathName;
    //public Quaternion startOrientation;
    //public Vector3 startPosition;

    Vector3 last_position;
    Vector3 current_position;
    
    // Start is called before the first frame update
    void Start()
    {
        last_position = GetComponent<Transform>().position;
        pathToFollow = GameObject.Find(GetComponent<GetPath>().GetVehiclePath()).GetComponent<EditorPath>();
        // Find nearest waypoint in path to start from
        float shortestDist=10000f;
        foreach (Transform path_obj in pathToFollow.path_objs)
        {
            float dist = Vector3.Distance(path_obj.position, last_position);
            if (dist < shortestDist)
            {
                shortestDist = dist;
                currentWayPointIndex = path_obj.GetSiblingIndex();
            }

        }
        //startOrientation = transform.localRotation;
        //startPosition = transform.localPosition;

        speed = regSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // Distance to next waypoint and from last waypoint
        float distance = Vector3.Distance(pathToFollow.path_objs[currentWayPointIndex].position, transform.position);
        float lastDistance = Vector3.Distance(last_position, transform.position);
        // Move towards waypoint
        transform.position = Vector3.MoveTowards(transform.position, pathToFollow.path_objs[currentWayPointIndex].position, Time.deltaTime * speed);
        // Orient Z-axis towards waypoint
        var rotation = Quaternion.LookRotation(pathToFollow.path_objs[currentWayPointIndex].position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

        // When the object reaches a point (within threshold reachDistance), then go to next point
        if(distance <= reachDistance)
        {
            last_position = pathToFollow.path_objs[currentWayPointIndex].position;
            currentWayPointIndex++;
        }
        if (distance <= reachDistance2 || lastDistance <=reachDistance2)
        {
            speed = cornerSpeed;
        }
        else speed = regSpeed;

        // Looping the path
        if (currentWayPointIndex >= pathToFollow.path_objs.Count)
        {
            currentWayPointIndex = 0;
        }
    }
}
