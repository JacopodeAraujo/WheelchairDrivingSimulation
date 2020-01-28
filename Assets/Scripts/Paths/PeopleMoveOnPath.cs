using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleMoveOnPath : MonoBehaviour
{
    public EditorPath pathToFollow;
    public int currentWayPointIndex;
    private float speed;
    public float reachDistance = 1.0f;
    //public float rotationSpeed = 5.0f;
    public string pathName;

    Vector3 last_position;
    Vector3 current_position;

    // Start is called before the first frame update
    void Start()
    {
        last_position = GetComponent<Transform>().position;
        //pathToFollow = GameObject.Find(pathName).GetComponent<EditorPath>();
        float shortestDist = 100000000000000f;
        foreach (Transform path_obj in pathToFollow.path_objs)
        {
            float dist = Vector3.Distance(path_obj.position, last_position);
            if (dist < shortestDist)
            {
                shortestDist = dist;
                currentWayPointIndex = path_obj.GetSiblingIndex();
            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(pathToFollow.path_objs[currentWayPointIndex].position, transform.position);

        //transform.position = Vector3.MoveTowards(transform.position, pathToFollow.path_objs[currentWayPointIndex].position, Time.deltaTime * speed);

        var rotation = Quaternion.LookRotation(pathToFollow.path_objs[currentWayPointIndex].position - transform.position);
        transform.rotation = rotation; // Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

        // When the object reaches a point (within threshold reachDistance), then go to next point
        if (distance <= reachDistance)
        {
            last_position = pathToFollow.path_objs[currentWayPointIndex].position;
            currentWayPointIndex++;
        }

        // Looping the path
        if (currentWayPointIndex >= pathToFollow.path_objs.Count)
        {
            currentWayPointIndex = 0;
        }
    }
}
