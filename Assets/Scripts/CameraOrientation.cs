using UnityEngine;

public class CameraOrientation : MonoBehaviour
{

    public float speedH = 2.0f;
    public float speedV = 2.0f;
    public float maxPitchUp = -80f;
    public float maxPitchDown = 30f;
    public float maxYaw = 75f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        transform.localEulerAngles = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch += speedV * -Input.GetAxis("Mouse Y");

        if (pitch <= maxPitchUp)
        {
            pitch = maxPitchUp;
        }
        if (pitch >= maxPitchDown)
        {
            pitch = maxPitchDown;
        }
        if (yaw <= -maxYaw)
        {
            yaw = -maxYaw;
        }
        if (yaw >= maxYaw)
        {
            yaw = maxYaw;
        }
        transform.localEulerAngles = new Vector3(pitch, yaw, 0.0f);

    }
}
