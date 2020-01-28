using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class GazeTargets : MonoBehaviour
{
    public Rigidbody rb;
    public NavMeshAgent agent;
    public float targetAppereanceVelocity = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("RB vel mag: " + rb.velocity.magnitude);
        //Debug.Log("Agent vel mag: " + agent.velocity.magnitude);
        if (rb.velocity.magnitude > targetAppereanceVelocity || agent.velocity.magnitude > targetAppereanceVelocity)
        {
            GetComponent<Renderer>().enabled = true;
        }
        else GetComponent<Renderer>().enabled = false;
    }


}
