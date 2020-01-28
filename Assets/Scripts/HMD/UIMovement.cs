using UnityEngine;

namespace VRStandardAssets.Utils
{
    // This class is used to move UI elements in ways that are
    // generally useful when using VR, specifically looking at
    // the camera and rotating so they're always in front of
    // the camera.
    public class UIMovement : MonoBehaviour
    {
        [SerializeField] private bool m_LookatCamera = true;    // Whether the UI element should rotate to face the camera.
        [SerializeField] private Transform m_UIElement;         // The transform of the UI to be affected.
        [SerializeField] private Transform m_Camera;            // The transform of the camera.
        [SerializeField] private bool m_RotateWithCamera;       // Whether the UI should rotate with the camera so it is always in front.
        [SerializeField] private float m_FollowSpeed = 10f;     // The speed with which the UI should follow the camera.

        [SerializeField] private Vector3 offset1 = Vector3.zero;     // Offset setting 1
        [SerializeField] private Vector3 offset2 = new Vector3(0, -10, 0);     // Offset setting 2
        [SerializeField] private Vector3 offset3 = new Vector3(0, 10, 0);     // Offset setting 3

        public int offset = 1;


        private float m_DistanceFromCamera;                     // The distance the UI should stay from the camera when rotating with it.


        private void Start ()
        {
            if (!m_UIElement)
                m_UIElement = transform;

            // Find the distance from the UI to the camera so the UI can remain at that distance.
            m_DistanceFromCamera = Vector3.Distance (m_UIElement.position, m_Camera.position);
        }


        private void Update()
        {
            // If the UI should look at the camera set it's rotation to point from the UI to the camera.
            if(m_LookatCamera)
                m_UIElement.rotation = Quaternion.LookRotation(m_UIElement.position - m_Camera.position);

            // If the UI should rotate with the camera...
            if (m_RotateWithCamera)
            {
                // Find the direction the camera is looking but on a flat plane.
                Vector3 targetDirection;
                if (offset == 1)
                    targetDirection = m_Camera.forward.normalized + offset1;
                else if (offset == 2)
                    targetDirection = m_Camera.forward.normalized + offset2;
                else if (offset == 3)
                    targetDirection = m_Camera.forward.normalized + offset3;
                else targetDirection = m_Camera.forward.normalized;

                targetDirection = Vector3.Lerp(m_UIElement.forward, targetDirection, m_FollowSpeed * Time.deltaTime);

                // Calculate a target position from the camera in the direction at the same distance from the camera as it was at Start.
                Vector3 targetPosition = m_Camera.position + targetDirection * m_DistanceFromCamera;

                // Set the target position  to be an interpolation of itself and the UI's position.
//                targetPosition = Vector3.Lerp(m_UIElement.position, targetPosition, m_FollowSpeed * Time.deltaTime);

                // Since the UI is only following on the XZ plane, negate any y movement.
  //              targetPosition.y = m_UIElement.position.y;


                // Set the UI's position to the calculated target position.
                m_UIElement.position = targetPosition;
            }
        }
    }
}