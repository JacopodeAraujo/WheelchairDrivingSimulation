using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickObstacleShoot : MonoBehaviour
{
    Vector3 startPos;
    Quaternion startRotation;

    public ObstaclePush obs;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            obs.ShootObstacle();
        }
    }

    //private void LateUpdate()
    //{
    //    transform.rotation = startRotation;
    //    transform.position = startPos;
    //}

    //private void Start()
    //{
    //    startPos = transform.position;
    //    startRotation = transform.rotation;

    //}
}
