
using UnityEngine;
using UnityEngine.AI;

public class ControlInterface : MonoBehaviour
{

    public StdCtrlInterface stdCtrl;
    public GoToGazeInterface g2gCtrl;
    public NavMeshAgent agent;
    public Rigidbody rb;
    public Camera maincam;
    public UserInterface ui;
    public GazeVectorControl gvc;

    // Control interface flags
    public bool ci1Active = false;
    public bool ci1_GazeControl = false;
    public bool ci1_BrainControl = false;

    public bool ci2Active = false;
    public bool ci2_GoToGazeActive = false;
    public bool ci2_MappedEnvironment = false;

    public bool ci3Active = false;
    public bool ci3_GazeVectorControl = false;
    public bool ci3_BrainControl2 = false;

    public bool brainbrakesICE = false;

    // Control interface config variables
    int ci1_config = 0;
    int ci2_config = 0;
    int ci3_config = 0;

    // Start is called before the first frame update
    void Start()
    {
        stdCtrl = GetComponent<StdCtrlInterface>();
        g2gCtrl = GetComponent<GoToGazeInterface>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        maincam = GetComponentInChildren<Camera>();
        gvc = GetComponentInChildren<GazeVectorControl>();

        // Default setting
        DefaultControl();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1")) 
        {
            if (ci1Active)
                ui.HandleClick();
             else ActivateCtrlInterface1();
        }
        if (Input.GetKeyDown("2"))
        {
            if (ci2Active)
                ui.HandleClick();
            else ActivateCtrlInterface2();
        }
        if (Input.GetKeyDown("3")) {
            if (ci3Active)
                ui.HandleClick();
            else ActivateCtrlInterface3();
        }
        if (Input.GetKey(KeyCode.Space))
            BrakeSystem(true);
        else BrakeSystem(false);
    }

    public void DefaultControl()
    {
        ci1_config = 2;
        ci2_config = 1;
        ci3_config = 1;
        ActivateCtrlInterface1();

        BrainBrakesICE(true);
    }

    public void ActivateCtrlInterface1()
    {
        Vector3 vel = Vector3.zero;
        if (ci2Active) {
            // Get agent velocity for transfer
            vel = g2gCtrl.agent.velocity;
        }
        //if (ci3Active) {
        //    // Change CI button name
        //    GameObject.Find("CtrlInt3").name = "CtrlInt1";
        //}


        // Deactivate CI2 and CI3
        CI2_Deactivate();
        CI3_Deactivate();

        // Use with last settings as last time and update UI button color
        ToggleCI1();
            
        // Activate CI1 
        Debug.Log("Control Interface 1:         Activated ");
        stdCtrl.enabled = true;
        ci1Active = true;

        if (vel != Vector3.zero)
            stdCtrl.TransitionVelocity(vel);
            // Transfer velocity to other controlinterface
    }

    public void ToggleCI1()
    {
        if (ci1_config == 1)
        {
            CI1_GazeControl(true);
            CI1_BrainControl(false);
            ui.ButtonSelected(ui.ci1_b, true);
        }
        else if (ci1_config == 2)
        {
            CI1_BrainControl(true);
            CI1_GazeControl(false);
            ui.ButtonSelected(ui.ci1_b, false);
        }
        else Debug.Log("Error reading CI1 config variable");
        
    }
    public void CI1_GazeControl(bool _enabled)
    {
        Debug.Log("CI1 Gaze Control: "+_enabled);
        ci1_GazeControl = _enabled;
        ui.ShowGazeControl(_enabled);
    }

    public void CI1_BrainControl(bool _enabled)
    {
        Debug.Log("CI1 Brain Control: " + _enabled);
        ci1_BrainControl = _enabled;
        ui.ShowBCIControl1(_enabled);
        
    }

    public void CI1_Deactivate()
    {
        if (ci1_GazeControl)
            ci1_config = 1;
        if (ci1_BrainControl)
            ci1_config = 2;
        Debug.Log("CI1 Disabled");
        stdCtrl.enabled = false;
        ci1Active = false;
        CI1_BrainControl(false);
        CI1_GazeControl(false);
    }


    public void ActivateCtrlInterface2()
    {
        // Get agent velocity
        Vector3 vel = GetComponent<Rigidbody>().velocity;

        // Deactivate CI1 and CI3
        CI1_Deactivate();
        CI3_Deactivate();

        // Activate CI2 
        Debug.Log("Control Interface 2:         Activated");
        rb.isKinematic = true;
        agent.enabled = true;
        ci2Active = true;
        // Use with last settings as last time and update UI button
        if (ci2_config == 1)
        {
            CI2_GoToGaze(true);
            ui.ButtonSelected(ui.ci2_b, true);
        }
        else if (ci2_config == 2)
        {
            CI2_MappedEnvironment(true);
            ui.ButtonSelected(ui.ci2_b, false);
        }
        else Debug.Log("Error reading CI2 config variable");

        // Transfer velocity to other controlinterface
        g2gCtrl.TransitionVelocity(vel);

    }

    public void CI2_MappedEnvironment(bool _enabled)
    {
        Debug.Log("CI2 Mapped Environment: " + _enabled);
        ci2_MappedEnvironment = _enabled;
    }

    public void CI2_GoToGaze(bool _enabled)
    {
        Debug.Log("CI2 Go To Gaze: " + _enabled);
        if (_enabled)
        {
            g2gCtrl.enabled = true;
            g2gCtrl.UpdateNavMeshSurface();
            ci2_GoToGazeActive = true;
        }
        else if (!_enabled)
        {
            g2gCtrl.enabled = false;
            ci2_GoToGazeActive = false;
        }
    }

    public void CI2_Deactivate()
    {
        if (ci2_GoToGazeActive)
            ci2_config = 1;
        if (ci2_MappedEnvironment)
            ci2_config = 2;
        Debug.Log("CI2 Disabled");
        agent.enabled = false;
        rb.isKinematic = false;
        CI2_GoToGaze(false);
        CI2_MappedEnvironment(false);
        ci2Active = false;
    }

    public void ActivateCtrlInterface3() {

        Vector3 vel = Vector3.zero;
        if (ci2Active) {
            // Get agent velocity for transfer
            vel = g2gCtrl.agent.velocity;
        }
        //if (ci1Active) {
        //    // Change CI button name
        //    GameObject.Find("CtrlInt1").name = "CtrlInt3";
        //}

        // Deactivate CI1 and CI2
        CI1_Deactivate();
        CI2_Deactivate();

        // Use with last settings as last time and update UI button color
        ToggleCI3();

        // Activate CI1 
        Debug.Log("Control Interface 3:         Activated ");
        stdCtrl.enabled = true;
        ci3Active = true;

        if (vel != Vector3.zero)
            stdCtrl.TransitionVelocity(vel);
            // Transfer velocity to other controlinterface
    }

    public void ToggleCI3() {
        if (ci3_config == 1) {
            CI3_GazeVectorControl(false);
            CI3_BrainControl2(true);
            //ui.ButtonSelected(ui.ci1_b, true);
        }
        else if (ci3_config == 2) {
            CI3_GazeVectorControl(true);
            CI3_BrainControl2(false);
            //ui.ButtonSelected(ui.ci1_b, false);
        }
        else Debug.Log("Error reading CI3 config variable");

    }

    public void CI3_GazeVectorControl(bool _enabled) {
        Debug.Log("CI3 Gaze Vector Control: " + _enabled);
        ci3_GazeVectorControl = _enabled;
        ui.ShowGazeVectorControl(_enabled);
    }

    public void CI3_BrainControl2(bool _enabled) {
        Debug.Log("CI3 Brain Control 2: " + _enabled);
        //stdCtrl.FindBCI2();
        ci3_BrainControl2 = _enabled;
        ui.ShowBCIControl2(_enabled); // Should be a BCIControl2 interface when developed, but now it is BCI1 with feedback given to OVCamera instead of driver
    }

    public void CI3_Deactivate() {
        if (ci3_GazeVectorControl)
            ci3_config = 1;
        if (ci3_BrainControl2)
            ci3_config = 2;
        Debug.Log("CI3 Disabled");
        stdCtrl.enabled = false;
        ci3Active = false;
        CI3_BrainControl2(false);
        CI3_GazeVectorControl(false);
    }

    public void BrainBrakesICE(bool _enabled)
    {
        Debug.Log("ICE Brain Brakes: "+ _enabled);
        brainbrakesICE = _enabled;
    }

    void BrakeSystem(bool isBraking)
    {
        if (isBraking)
        {
            Debug.Log("Braking");
            if (g2gCtrl.enabled)
            {
                // Braking through subinterfaces
                GetComponent<GoToGazeInterface>().AgentBrake(true);
                // Braking around subinterfaces
                GetComponent<NavMeshAgent>().isStopped = true;
                GetComponent<NavMeshAgent>().ResetPath();
            }
            GetComponent<StdCtrlInterface>().Brake(true);
            GetComponent<StdCtrlInterface>().frontL_W.brakeTorque = GetComponent<StdCtrlInterface>().brakeForce;
            GetComponent<StdCtrlInterface>().frontR_W.brakeTorque = GetComponent<StdCtrlInterface>().brakeForce;
            
        }
        else
        {
            if (g2gCtrl.enabled)
            {
                GetComponent<GoToGazeInterface>().AgentBrake(false);
                GetComponent<NavMeshAgent>().isStopped = false;
            }
            GetComponent<StdCtrlInterface>().Brake(false);

        }
    }

}
