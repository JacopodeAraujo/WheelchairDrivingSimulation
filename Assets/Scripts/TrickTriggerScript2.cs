using UnityEngine.AI;
using UnityEngine;

public class TrickTriggerScript2 : MonoBehaviour
{
    public GameObject[] app_objs = null;
    public GameObject[] disapp_obs = null;
    NavMeshSurface surface;
    float timer;
    public int time=4;
    bool flag = false;

    // Start is called before the first frame update
    void Start()
    {
        surface = FindObjectOfType<NavMeshSurface>();

        if (app_objs != null)
            MakeInvisible(app_objs);
        if (disapp_obs != null)
            MakeVisible(disapp_obs);
        surface.BuildNavMesh();
    }

    private void OnTriggerEnter(Collider other) {
        if (app_objs != null) 
            MakeVisible(app_objs);
        if (disapp_obs != null)
            MakeInvisible(disapp_obs);
        StartTimer();
        surface.BuildNavMesh();
    }
    private void MakeInvisible(GameObject[] objs) {
        if (objs.Length > 0) {
            foreach (GameObject obj in objs) {
                // Make invisble and switch to trick-layer
                obj.GetComponent<Renderer>().enabled = false;
                obj.GetComponent<BoxCollider>().isTrigger = true;
                obj.layer = 19;
            }
        }
    }

    private void MakeVisible(GameObject[] objs) {
        if (objs.Length > 0) {
            foreach (GameObject obj in objs) {
                // Turn visible and switch to obstacle-layer
                obj.GetComponent<Renderer>().enabled = true;
                obj.GetComponent<BoxCollider>().isTrigger = false;
                if (obj.tag.Contains("Obstacle"))       // If tag contains Obstacle, fx "Obstacle" or "TrickObstacle"
                    obj.layer = 15;
                if (obj.tag.Contains("Ground"))         // If tag contains Ground, fx "Obstacle" or "TrickGround"
                    obj.layer = 13;
            }
        }
    }

    private void StartTimer() {
        timer = Time.time;
        flag = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (flag)
            if (Time.time > timer + 4) {
                MakeInvisible(app_objs);
                MakeVisible(disapp_obs);
                surface.BuildNavMesh();
                GetComponent<BoxCollider>().enabled = false;
                flag = false;
            }

    }
}
