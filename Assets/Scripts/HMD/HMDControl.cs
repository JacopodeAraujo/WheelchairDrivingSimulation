using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fove.Unity;


public class HMDControl : MonoBehaviour
{
    bool setAlignment = true;
    Transform tf;
    Fove.Headset headset;
    Fove.HeadsetResearch headsetResearch;
    Fove.ResearchGaze researchGaze;

    Camera[] myCams = new Camera[4];


    // Start is called before the first frame update
    void Start()
    {
        headset = FoveManager.Headset;
        headsetResearch = headset.GetResearchHeadset(Fove.ResearchCapabilities.EyeImage);

        tf = GetComponent<Transform>();

        //Get Main Camera
        myCams[0] = GetComponent<Camera>();

        //Find All other Cameras
        myCams[1] = GetComponentInChildren<Camera>();

        //Call function when new display is connected
        Display.onDisplaysUpdated += OnDisplaysUpdated;

        //Map each Camera to a Display
        mapCameraToDisplay();
    }
    void mapCameraToDisplay() {
        //Loop over Connected Displays
        for (int i = 0; i < Display.displays.Length; i++) {
            Debug.Log("Cam:" + myCams[i] + " connected to " + Display.displays[i]);
            myCams[i].targetDisplay = i; //Set the Display in which to render the camera to
            Display.displays[i].Activate(); //Enable the display
        }
    }

    void OnDisplaysUpdated() {
        Debug.Log("New Display Connected. Show Display Option Menu....");
    }

    // Update is called once per frame
    void Update()
    {
        if (setAlignment) {
            FoveManager.TareOrientation();
            setAlignment = false;
        }
        if (Input.GetKey("o"))
            FoveManager.TareOrientation();

        if (Input.GetKey("i")) {
            headsetResearch.GetGaze(out researchGaze);
            Debug.Log("Left Eye Radius measurements");
            Debug.Log("EyeBall: " + researchGaze.eyeDataLeft.eyeballRadius);
            Debug.Log("Iris: " + researchGaze.eyeDataLeft.irisRadius);
            Debug.Log("Pupil: " + researchGaze.eyeDataLeft.pupilRadius);
            Debug.Log("---------------------------------------");
            Debug.Log("Right Eye Radius measurements");
            Debug.Log("EyeBall: " + researchGaze.eyeDataRight.eyeballRadius);
            Debug.Log("Iris: " + researchGaze.eyeDataRight.irisRadius);
            Debug.Log("Pupil: " + researchGaze.eyeDataRight.pupilRadius);
            Debug.Log("---------------------------------------");
        }
            
        //Debug.Log("Gaze ray: " + fint.GetGazeConvergence().ray );
        //Debug.Log("Gaze distance: " + fint.GetGazeConvergence().distance);
        //Debug.Log("Gaze accuracy: " + fint.GetGazeConvergence().accuracy);

        //test.GetComponent<Transform>().position = fint.GetGazeConvergence().ray.origin + fint.GetGazeConvergence().ray.direction * fint.GetGazeConvergence().distance;
    }
}
