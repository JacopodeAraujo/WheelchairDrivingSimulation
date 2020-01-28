using UnityEngine;

public class StdCtrlInterface : MonoBehaviour
{
    public UI_GazeControl gazeControlInterface;
    public GazeVectorControl gvc;
    public Bci_control bci;
    //public Bci_control bci2;
    public ControlInterface ci;

    private float m_horizontalInput;
    private float m_verticalInput;
    private bool m_brake;
    private float m_steeringAngle;

    public Quaternion testRotate;
    public Vector3 testTrans;
    public WheelCollider frontL_W, frontR_W;
    public WheelCollider rearL_W, rearR_W;
    public Transform frontL_T, frontR_T;
    public Transform rearL_T, rearR_T;
    public float maxSteerAngle = 30;
    public float motorForce = 50;
    public float brakeForce = 50;

    public float speedThreshold;
    public int stepsBelowThreshold;
    public int stepsAboveThreshold;


    Vector3 transitionvelocity = Vector3.zero;
    int testCount=100;


    private void Start() {
        //FindBCI2();
    }
    private void OnEnable()
    {
        //bci = GetComponentInChildren<Bci_control>();
        ci = GetComponent<ControlInterface>();
        //gvc = GetComponentInChildren<GazeVectorControl>();
    }
    public void GetInput()
    {
        m_horizontalInput = Input.GetAxis("Horizontal");
        m_verticalInput = Input.GetAxis("Vertical");
        //Debug.Log("Vert: " + m_verticalInput);
        //Debug.Log("Hori: " + m_horizontalInput);
    }


    private void Steer()
    {
        m_steeringAngle = maxSteerAngle * m_horizontalInput;
        frontL_W.steerAngle = m_steeringAngle;
        frontR_W.steerAngle = m_steeringAngle;
    }

    private void Accelerate()
    {
        frontL_W.motorTorque = m_verticalInput * motorForce;
        frontR_W.motorTorque = m_verticalInput * motorForce;
        //if (m_verticalInput != 0)
        //{
        //    Debug.Log("Accelerating: " + m_verticalInput);
        //    Debug.Log(m_horizontalInput);
        //}
    }
    public void Brake(bool isBraking)
    {
        if (isBraking)
        {
            //Debug.Log("Braking.");
            frontL_W.motorTorque = 0;
            frontR_W.motorTorque = 0;
            frontL_W.brakeTorque = brakeForce;
            frontR_W.brakeTorque = brakeForce;
        }
        else
        {
            frontL_W.brakeTorque = 0;
            frontR_W.brakeTorque = 0;
        }
    }
    private void UpdateWheelPoses()
    {
        //frontL_T.localRotation = testRotate;
        //frontR_T.localRotation = testRotate;
        //rearL_T.localRotation = testRotate;
        //rearR_T.localRotation = testRotate;

        UpdateWheelPose(frontL_W, frontL_T);
        UpdateWheelPose(frontR_W, frontR_T);
        UpdateWheelPose(rearL_W, rearL_T);
        UpdateWheelPose(rearR_W, rearR_T);
    }

    private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;
        _collider.GetWorldPose(out _pos, out _quat);

        _transform.rotation = _quat;
    }


    private void FixedUpdate()
    {

        // If gaze control is enabled, get input from gaze control UI
        if (ci.ci1_GazeControl)
        {
            m_verticalInput = gazeControlInterface.GetVertInput();
            m_horizontalInput = gazeControlInterface.GetHoriInput();
            Brake(gazeControlInterface.isBraking());
            
        }
        else if (ci.ci3_GazeVectorControl) {
            m_verticalInput = gvc.GetVertAcc();
            m_horizontalInput = gvc.GetHoriAcc();
            Brake(gvc.isBraking());
        }
        else if (ci.ci1_BrainControl) {
            m_verticalInput = bci.GetVertAcc();
            m_horizontalInput = bci.GetHoriAcc();
            Brake(bci.isBraking());
        }
        else if (ci.ci3_BrainControl2) {
            //m_verticalInput = bci2.GetVertAcc();
            //m_horizontalInput = bci2.GetHoriAcc();
            //Brake(bci2.isBraking());
        }
        else GetInput();
        // Print vertical and horizontal input for debug
        if (Input.GetKeyDown("e"))
            Debug.Log("Vert: " + m_verticalInput + " Hori: " + m_horizontalInput);

        if (testCount < 3)
        {
            if (testCount == 0)
            {
                GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, transitionvelocity.magnitude), ForceMode.VelocityChange);
            }
            m_verticalInput = 0.3f;
            testCount++;
        }
        Steer();
        Accelerate();
        

        UpdateWheelPoses();

        WheelCollider WheelColliders = GetComponentInChildren<WheelCollider>();
        WheelColliders.ConfigureVehicleSubsteps(speedThreshold, stepsBelowThreshold, stepsAboveThreshold);

    }

    // This function transfers the velocity when going from control interfaces utilizing this standard interface, to the GoToGaze, that utilizes a Nav Agent with non-kinematic rigidbody
    public void TransitionVelocity(Vector3 _velocity)
    {
        if (_velocity != Vector3.zero)
        {
            Debug.Log("Transf. vel: " + _velocity);
            transitionvelocity = _velocity;
            testCount = 0;
        }
    }

    public float GetVelocityMagnitude()
    {
        return GetComponent<Rigidbody>().velocity.magnitude;
    }

    //public void FindBCI2() {
    //    bci2 = GameObject.Find("OVCamera").GetComponentInChildren<Bci_control>();
    //}

}