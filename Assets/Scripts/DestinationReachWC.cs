using UnityEngine;
using UnityEngine.AI;

public class DestinationReachWC : MonoBehaviour
{
    public Material destMatTransparent;
    public Material destMatNotTransparent;

    public GameObject wheelchair;

    Rigidbody playerRB;
    NavMeshAgent agent;
    TrailTrackingLog logger;

    public GameManagerWC gameManager;
    public float velocityThreshold=0.1f;

    bool doneflag = false;
    bool putflag = false;

    public GameManagerWC GameManagerWC
    {
        get => default;
        set
        {
        }
    }

    public TrailTrackingLog TrailTrackingLog
    {
        get => default;
        set
        {
        }
    }

    private void Start() {
        playerRB = wheelchair.GetComponent<Rigidbody>();
        agent = wheelchair.GetComponent<NavMeshAgent>();
        logger = wheelchair.GetComponentInChildren<TrailTrackingLog>();
    }
    public void matTransparent(bool isTransparent)
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            if (isTransparent)
            {
                rend.material = destMatTransparent;
            }
            else rend.material = destMatNotTransparent;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        matTransparent(true);
        doneflag = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        { 
            if (playerRB.velocity.magnitude < velocityThreshold && agent.velocity.magnitude < velocityThreshold)
            {
                if (doneflag==false)
                {
                    doneflag = true;
                    Debug.Log("You won");
                    logger.EndLogging();
                    gameManager.NextLevel();

                }

            }
            else
            {
                if (putflag==false)
                {
                    Debug.Log("Stay put");
                    putflag = true;
                }
            }
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        matTransparent(false);
        putflag = false;
    }

}
