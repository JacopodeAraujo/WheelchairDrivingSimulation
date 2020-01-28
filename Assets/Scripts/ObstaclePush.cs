using UnityEngine;
using System;
using System.Threading;
public class ObstaclePush : MonoBehaviour
{
    public Transform target;
    public Rigidbody rb;

    public Vector3 pushForce = new Vector3(0, 0, -1000);
    bool state = false;
    public float aimSpeed = 2f;

    
    void Update()
    {
        if (!state)
        {
            GetComponent<Collider>().isTrigger=true;
            GetComponent<Rigidbody>().isKinematic = true;
            rb.useGravity = false;
            //transform.position=startPosition;
            Vector3 direction = target.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, aimSpeed * Time.deltaTime);
            
        }

    }   

    public void ShootObstacle()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        rb.useGravity = true;
        //Debug.Log("shoot");
        rb.AddRelativeForce(pushForce, ForceMode.Impulse);
        GetComponent<Collider>().isTrigger = false;
        state = true;
    }


}
