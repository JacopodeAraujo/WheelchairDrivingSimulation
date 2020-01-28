using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public static int obstacleHits;
    static bool isHittingObstacle = false;

    public TrailTrackingLog TrailTrackingLog
    {
        get => default;
        set
        {
        }
    }

    private void Start()
    {
        obstacleHits = 0;
    }
    void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.tag.Contains("Obstacle"))
        {
            obstacleHits++;
            if (collision.collider.tag.Contains("Trick")) {
                GetComponentInChildren<TrailTrackingLog>().CollisionIncrement(true);
                Debug.Log("You hit the trick obstacle!!!");
            }
            else {
                GetComponentInChildren<TrailTrackingLog>().CollisionIncrement(false);
                Debug.Log("You hit an obstacle. Hits: "+obstacleHits);
            }


            isHittingObstacle = true;
        }

        if (collision.collider.tag == "Vehicle")
        {
            obstacleHits++;
            GetComponentInChildren<TrailTrackingLog>().CollisionIncrement(false);
            Debug.Log("You hit a vehicle. Hits: " + obstacleHits);
            isHittingObstacle = true;
        }
    }

    private void OnCollisionExit(Collision collision) {
        isHittingObstacle = false;
    }

    public static int GetObstacleHits()
    {
        return obstacleHits;
    }
    public static bool IsHittingObstacle() {
        return isHittingObstacle;
    }
}
