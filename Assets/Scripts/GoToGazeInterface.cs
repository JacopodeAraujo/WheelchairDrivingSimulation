using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using VRStandardAssets.Utils;
using Fove.Unity;

public class GoToGazeInterface : MonoBehaviour
{
    public NavMeshSurface surface;
    public NavMeshAgent agent;
    public Camera cam;
    FoveInterface fint;
    public GameObject gtginterface;
    Vector3 gaze_origin;
    Vector3 gaze_direction;
    float gaze_distance;
    float maxRayDistance = 100f;

    List<Vector3> gazePoints;
    public int numOfPoints = 40;
    public float verificationRadius = 10f;
    SelectionRadial selectionRadial;

    public GameObject targetMark;

    public Button b_brakeReverse;
    public Button b_leftrotation;
    public Button b_rightrotation;

    bool reverse_flag = false;

    private void OnEnable() {

        b_brakeReverse.GetComponent<VRInteractiveItem>().OnClick += delegate { AgentBrake(true); };
        b_brakeReverse.GetComponent<VRInteractiveItem>().OnOut += delegate { AgentBrake(false); reverse_flag = false; };

        b_leftrotation.GetComponent<VRInteractiveItem>().OnClick += delegate { AgentRotate(false); };
        b_rightrotation.GetComponent<VRInteractiveItem>().OnClick += delegate { AgentRotate(true); };

        fint = GetComponentInChildren<FoveInterface>();
        gazePoints = new List<Vector3>();
        selectionRadial = GetComponentInChildren<SelectionRadial>();
        gtginterface.SetActive(true);
        surface.BuildNavMesh();
    }
    private void OnDisable() {
        b_brakeReverse.GetComponent<VRInteractiveItem>().OnClick -= delegate { AgentBrake(true); };
        b_brakeReverse.GetComponent<VRInteractiveItem>().OnOut -= delegate { AgentBrake(false); reverse_flag = false; };

        b_leftrotation.GetComponent<VRInteractiveItem>().OnClick -= delegate { AgentRotate(false); };
        b_rightrotation.GetComponent<VRInteractiveItem>().OnClick -= delegate { AgentRotate(true); };
        //agent.ResetPath();
        if (targetMark.GetComponent<MeshRenderer>() != null)
            targetMark.GetComponent<MeshRenderer>().enabled = false;
        selectionRadial.HandleOut();
        gtginterface.SetActive(false);
    }
    public void UpdateNavMeshSurface() {
        surface.BuildNavMesh();
    }
    public void GoToGazeUpdate(RaycastHit hit) {
        AgentBrake(false);

        gazePoints.Add(hit.point);

        if (gazePoints.Count > 1) {

            // Find max distance between points in list
            Vector3[] list = findMaxDist(gazePoints);

            // If the 2 most distant points are within verification circle...
            if (list[0].x <= 2*verificationRadius) {
                float process = (float) gazePoints.Count / numOfPoints;

                // Update Selection Radial - OBS: THIS IS FRAME-UPDATE DEPENDENT, MEANING THAT THE G2G RADIAL WILL FILL IN DIFFERENT TEMPOS DEPENDENT ON THE PC RUNNING THE GAME. 
                // A BETTER SOLUTION IS TO USE selectionRadial.HandleOver() & selectionRadial.HandleOut() and wait the OnSelectionComplete-event, as with VRInteractiveItems.
                // No time to implement this solution, but should definitely be implemented in future developments. It shouldn't be too complicated.
                selectionRadial.Show();
                selectionRadial.FillSelectionRadial(process);

                if (gazePoints.Count >= numOfPoints) {
                    Vector3 centerPoint = ((list[2] - list[1]) / 2) + list[1];  // Center point between 2 most distant points
                    //Debug.Log("Point verified. Center: "+centerPoint);
                    agent.updateRotation = true;
                    agent.SetDestination(hit.point);
                    gazePoints.Clear();
                    gazePoints.TrimExcess();
                }
            }
            else {
                // Clear Selection Radial
                selectionRadial.FillSelectionRadial(0f);
                selectionRadial.Hide();
                // Clear List
                gazePoints.Clear();
                gazePoints.TrimExcess();
            }
        }
        
        if (agent.hasPath) {
            targetMark.GetComponent<MeshRenderer>().enabled = true;
            targetMark.transform.position=agent.pathEndPosition;
            targetMark.transform.localScale.Set(verificationRadius, 0.01f, verificationRadius);
        }
        else targetMark.GetComponent<MeshRenderer>().enabled = false;

    }
    // Update is called once per frame
    void Update()
    {
        if (reverse_flag)
            AgentReverse();
        //gaze_origin = fint.GetGazeConvergence().ray.origin;
        //gaze_direction = fint.GetGazeConvergence().ray.direction;
        //gaze_distance = fint.GetGazeConvergence().distance;

        //Ray ray = new Ray(gaze_origin, gaze_direction);

        //if (Input.GetMouseButtonDown(0))
        //{
        //    //agent.transform.SetPositionAndRotation(GetComponent<Transform>().position, GetComponent<Transform>().rotation);

        //    if (EventSystem.current.IsPointerOverGameObject())
        //        return; 
        //    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;

        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        agent.SetDestination(hit.point);
        //    }
        //}
    }


    Vector3[] findMaxDist(List<Vector3> list) {
        float temp;
        float max=0f;
        Vector3[] distantPoints = new Vector3[3];
        for (int i=0; i<list.Count; i++) {
            for (int j = 0; j < list.Count; j++) {
                if (i != j) {
                    temp = Vector3.Distance(list[i], list[j]); // Distance between points

                    if (temp > max) {
                        max = temp;
                        distantPoints[1] = list[i];
                        distantPoints[2] = list[j];
                    }
                }
            }
        }
        distantPoints[0] = new Vector3(max, max, max);
        return distantPoints;
    }

    public void AgentBrake(bool brake)
    {
        if (brake)
        {
            if (agent.velocity.magnitude > 0.1f) {
                Debug.Log("Agent Brake");
                agent.isStopped = true;
                agent.ResetPath();
            }
            else reverse_flag = true;
            targetMark.GetComponent<MeshRenderer>().enabled = false;
        }
        else agent.isStopped = false;

    }

    public void AgentReverse() {
        agent.updateRotation = false;
        Vector3 backwards = b_brakeReverse.transform.localPosition + (Vector3.back * 100f);
        agent.SetDestination(transform.TransformPoint(backwards));

    }

    public void AgentRotate(bool right) {
        agent.updateRotation = true;
        if (right) {
            agent.SetDestination(transform.TransformPoint(b_brakeReverse.transform.localPosition + (Vector3.right * 100f)));
        }
        else agent.SetDestination(transform.TransformPoint(b_brakeReverse.transform.localPosition + (Vector3.left * 100f)));
    }

    public void TransitionVelocity(Vector3 _velocity)
    {
        Debug.Log("Transf. vel: " + _velocity);
        if (_velocity != Vector3.zero)
        {
            agent.SetDestination(GetComponent<Transform>().TransformPoint(0,0,3000));
            agent.velocity = _velocity;
        }
        agent.ResetPath();
       
    }

}
