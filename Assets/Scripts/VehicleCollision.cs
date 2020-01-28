using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleCollision : MonoBehaviour
{
    
    void OnCollisionEnter(Collision other)
    {
        
        if (other.collider.tag == "Vehicle")
        {
            //Physics.IgnoreCollision(collision.GetComponent<Collider>(), GetComponent<Collider>());
            Debug.Log("Vehicle Crash Ignored");
        }
        if (other.collider.tag == "Player")
        {
            Debug.Log("Hoonk hoonk");
        }
    }
}
