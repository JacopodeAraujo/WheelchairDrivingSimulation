using UnityEngine;
using UnityEngine.AI;

public class TrickTriggerScript : MonoBehaviour
{

    public GameObject[] app_objs=null;
    public GameObject[] dis_objs=null;
    NavMeshSurface surface;
    private void Start()
    {
        surface = FindObjectOfType<NavMeshSurface>();
        //surface.BuildNavMesh();
        
        if (app_objs.Length > 0)
        {
            foreach (GameObject obj in app_objs)
            {
                // Start invisible on the trick-layer
                obj.GetComponent<Renderer>().enabled = false;
                obj.GetComponent<BoxCollider>().isTrigger = true;
                obj.layer = 19;
                //obj.GetComponent<BoxCollider>().isTrigger = false;
            }
        }
        if (dis_objs.Length > 0)
        {
            foreach (GameObject obj in dis_objs)
            {
                // Start visible on the object layer
                obj.GetComponent<Renderer>().enabled = true;
                obj.GetComponent<BoxCollider>().isTrigger = false;
                if (obj.tag.Contains("Obstacle"))       // If tag contains Obstacle, fx "Obstacle" or "TrickObstacle"
                    obj.layer = 15;
                if (obj.tag.Contains("Ground"))         // If tag contains Ground, fx "Obstacle" or "TrickGround"
                    obj.layer = 13;
                //obj.GetComponent<BoxCollider>().isTrigger = false;

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (app_objs.Length > 0)
        {
            foreach (GameObject obj in app_objs)
            {
                // Turn visible and switch to obstacle-layer
                obj.GetComponent<Renderer>().enabled = true;
                if (obj.tag == "Obstacle")
                    obj.layer = 15;
                if (obj.tag == "Ground")
                    obj.layer = 13;
                //obj.GetComponent<BoxCollider>().isTrigger = false;
            }
        }
        if (dis_objs.Length > 0)
        {
            foreach (GameObject obj in dis_objs)
            {
                // Turn invisible and switch to trick-layer
                obj.GetComponent<Renderer>().enabled = false;
                obj.GetComponent<BoxCollider>().isTrigger = true;
                obj.layer = 19;
            }
        }
        GetComponent<BoxCollider>().enabled = false;

        surface.BuildNavMesh();
        //GetComponent<Renderer>().enabled = false;
    }
}
