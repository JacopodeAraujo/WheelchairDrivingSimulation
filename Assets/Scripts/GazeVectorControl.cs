using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;
using UnityEngine.UI;

public class GazeVectorControl : MonoBehaviour
{

    //FoveInterface2 fint;
    StdCtrlInterface std;
    //ControlInterface ci;
    //VREyeRaycaster vrray;

    public Button b_free;
    public Button b_brakeReverse;
    public Button b_brakeReverse2;
    public Button b_reverseLeft;
    public Button b_reverseRight;

    float vert_acc = 0f;
    float hori_acc = 0f;
    public float dist_limit_z1 = 0.1f;
    public float dist_limit_z2 = 5f;
    public float dist_limit_x1 = 0.3f;
    public float dist_limit_x2 = 3f;

    public float speed_limit_x1 = 0.001f;
    public float speed_limit_x2 = 1f;

    bool brake_flag = false;
    bool flag = false;

    float fx_a;
    float fx_b;

    private void Start() {
        std = GetComponentInParent<StdCtrlInterface>();

        // Exponential function parameters for horizontal acceleration
        fx_a = Mathf.Pow(speed_limit_x2/ speed_limit_x1, 1 / (dist_limit_x2 - dist_limit_x1));
        fx_b = speed_limit_x1 / (Mathf.Pow(fx_a, dist_limit_x1));
        
    }
    private void OnEnable() {
        //b_free.GetComponent<VRInteractiveItem>().OnClick += ResetAcceleration;
        b_brakeReverse.GetComponent<VRInteractiveItem>().OnClick += delegate { flag = true; BrakeOrReverse(); };
        b_brakeReverse2.GetComponent<VRInteractiveItem>().OnClick += delegate { flag = true; BrakeOrReverse(); };
        b_reverseLeft.GetComponent<VRInteractiveItem>().OnClick += delegate { flag = true; hori_acc = -1f; BrakeOrReverse(); };
        b_reverseRight.GetComponent<VRInteractiveItem>().OnClick += delegate { flag = true; hori_acc = 1f; BrakeOrReverse(); };

        b_brakeReverse.GetComponent<VRInteractiveItem>().OnOut += delegate { flag = false; };
        b_brakeReverse2.GetComponent<VRInteractiveItem>().OnOut += delegate { flag = false; };
        b_reverseLeft.GetComponent<VRInteractiveItem>().OnOut += delegate { flag = false; };
        b_reverseRight.GetComponent<VRInteractiveItem>().OnOut += delegate { flag = false; };

        //ci = GetComponentInParent<ControlInterface>();
        //vrray = GetComponentInChildren<VREyeRaycaster>();

    }

    private void OnDisable() {
        //b_free.GetComponent<VRInteractiveItem>().OnClick -= ResetAcceleration;
        b_brakeReverse.GetComponent<VRInteractiveItem>().OnClick -= delegate { flag = true; BrakeOrReverse(); };
        b_brakeReverse2.GetComponent<VRInteractiveItem>().OnClick -= delegate { flag = true; BrakeOrReverse(); };
        b_reverseLeft.GetComponent<VRInteractiveItem>().OnClick -= delegate { flag = true; hori_acc = -1f; BrakeOrReverse(); };
        b_reverseRight.GetComponent<VRInteractiveItem>().OnClick -= delegate { flag = true; hori_acc = 1f; BrakeOrReverse(); };

        b_brakeReverse.GetComponent<VRInteractiveItem>().OnOut -= delegate { flag = false; };
        b_brakeReverse2.GetComponent<VRInteractiveItem>().OnOut -= delegate { flag = false; };
        b_reverseLeft.GetComponent<VRInteractiveItem>().OnOut -= delegate { flag = false; };
        b_reverseRight.GetComponent<VRInteractiveItem>().OnOut -= delegate { flag = false; };
    }

    public void GazeVectorUpdate(RaycastHit hit) { 
        //Vector3 gazeVector = transform.worldToLocalMatrix.MultiplyPoint(hit.point);
        Vector3 gazeVector = hit.point - transform.position;
        gazeVector = transform.InverseTransformDirection(gazeVector);

        // Press e to print GazeVector for Debug
        if (Input.GetKeyDown("e"))
            Debug.Log("GazeVector: " + gazeVector);

        // Limit horizontal control action to vector distances
        if (Mathf.Abs(gazeVector.x) < dist_limit_x1)
            hori_acc = 0f;
        else if (gazeVector.x > dist_limit_x2)
            hori_acc = 1f;
        else if (gazeVector.x < -dist_limit_x2)
            hori_acc = -1f;
        // If vector is within limits, then use exponential functions to describe steering angle depending on how far away you look vertically
        else {
            // Gaze long-range vertically
            if (gazeVector.z > 1.0f) {
                if (gazeVector.x < 0)
                    hori_acc = -fx_b*Mathf.Pow(fx_a, Mathf.Abs(gazeVector.x)); //hori_acc = 1 / (dist_limit_x2-dist_limit_x1) * gazeVector.x; //--- Linear function
                if (gazeVector.x > 0)
                    hori_acc = fx_b * Mathf.Pow(fx_a, gazeVector.x);
            }
            // Gaze mid-range vertically, slope of exponential function is more aggressive
            else if (gazeVector.z > 0.6f)
            {
                if (gazeVector.x < 0)
                    hori_acc = -fx_b * Mathf.Pow(2f*fx_a, Mathf.Abs(gazeVector.x)); //hori_acc = 1 / (dist_limit_x2-dist_limit_x1) * gazeVector.x; //--- Linear function
                if (gazeVector.x > 0)
                    hori_acc = fx_b * Mathf.Pow(2f*fx_a, gazeVector.x);
            }
            // Gaze short-range, set steering angle to max to allow sharp turns with reduced speed
            else {
                if (gazeVector.x < 0)
                    hori_acc = -1f;
                if (gazeVector.x > 0)
                    hori_acc = 1f;
            } 
        }

        //If gaze is above vertical threshold, then no control action
        if (gazeVector.z > dist_limit_z2) {
            ResetAcceleration();
            brake_flag = false;
        }
        //If gaze is below threshold, then reset acceleration
        else if (gazeVector.z < dist_limit_z1 && !flag) {
            ResetAcceleration();
            //brake_flag = true;
        }
        //else if (gazeVector.z < 0.7f) {
        //    brake_flag = false;
        //    vert_acc = 1 / (2f*(dist_limit_z2 - dist_limit_z1)) * gazeVector.z;
        //}
        else {
            brake_flag = false;
            vert_acc = 1 / (dist_limit_z2 - dist_limit_z1) * gazeVector.z;
        }

        //Debug.Log("Gaze Vector: " + gazeVector);
        //Debug.Log("Vert: " + vert_acc);
        //Debug.Log("Hori: " + hori_acc);
    }

    public float GetVertAcc() {
        return vert_acc;
    }
    public float GetHoriAcc() {
        return hori_acc;
    }

    public bool isBraking() {
        return brake_flag;
    }
    public void ResetAcceleration() {
        //Debug.Log("Reset");
        vert_acc = 0f;
        hori_acc = 0f;
    }
    public void Brake(bool braking) {
        if (braking) {
            ResetAcceleration();
            //Debug.Log("Brake");
        }
        brake_flag = braking;
    }

    public void BrakeOrReverse() {
        if (std.GetVelocityMagnitude() < 0.1f) {
            //Debug.Log("Reverse");
            Brake(false);
            vert_acc = -0.1f;
        }
        else Brake(true);
    }
}
