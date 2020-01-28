using System;
using UnityEngine;
using System.Linq;
using Fove.Unity;

namespace VRStandardAssets.Utils {
    // In order to interact with objects in the scene
    // this class casts a ray into the scene and if it finds
    // a VRInteractiveItem it exposes it for other classes to use.
    // This script should be generally be placed on the camera.
    public class VREyeRaycaster : MonoBehaviour {
        public event Action<RaycastHit> OnRaycasthit;                   // This event is called every frame that the user's gaze is over a collider.


        [SerializeField] private Transform m_Camera;
        [SerializeField] private LayerMask m_ExclusionLayers;           // Layers to exclude from the raycast.
        [SerializeField] private Reticle m_Reticle;                     // The reticle, if applicable.
        [SerializeField] private SelectionRadial m_SelectionRadial;     // The Selection radial
        //[SerializeField] private VRInput m_VrInput;                   // Used to call input based events on the current VRInteractiveItem.
        [SerializeField] private bool m_ShowDebugRay;                   // Optionally show the debug ray.
        [SerializeField] private float m_DebugRayLength = 5f;           // Debug ray length.
        [SerializeField] private float m_DebugRayDuration = 1f;         // How long the Debug ray will remain visible.
        [SerializeField] private float m_RayLength = 500f;              // How far into the scene the ray is cast.


        private VRInteractiveItem m_CurrentInteractible;                //The current interactive item
        private VRInteractiveItem m_LastInteractible;                   //The last interactive item
        private float radial_dur;
        FoveInterface fint;
        ControlInterface ci;
        GoToGazeInterface gtg;
        public GazeVectorControl gvc;

        Vector3 gaze_origin;
        Vector3 gaze_direction;
        float gaze_distance;
        GazeConvergenceData gazeData;

        // Utility for other classes to get the current interactive item
        public VRInteractiveItem CurrentInteractible
        {
            get { return m_CurrentInteractible; }
        }



        //private void Start() {
        //    fint = GetComponent<FoveInterface2>();
        //}

        private void OnEnable() {
            fint = GetComponentInChildren<FoveInterface>();
            ci = GetComponentInParent<ControlInterface>();
            gtg = GetComponentInParent<GoToGazeInterface>();
            //gvc = GetComponentInParent<GazeVectorControl>();
            m_Reticle = GetComponent<Reticle>();
            m_SelectionRadial = GetComponent<SelectionRadial>();

            m_SelectionRadial.OnSelectionComplete += HandleClick;
            radial_dur = m_SelectionRadial.SelectionDuration;
            //m_VrInput.OnClick += HandleClick;
            //m_VrInput.OnDoubleClick += HandleDoubleClick;
            //m_VrInput.OnUp += HandleUp;
            //m_VrInput.OnDown += HandleDown;
        }


        private void OnDisable() {
            //m_VrInput.OnClick -= HandleClick;
            //m_VrInput.OnDoubleClick -= HandleDoubleClick;
            //m_VrInput.OnUp -= HandleUp;
            //m_VrInput.OnDown -= HandleDown;
        }


        private void Update() {
            EyeRaycast();
        }

        
        private void EyeRaycast() {
            m_SelectionRadial.SelectionDuration = radial_dur;
            // Show the debug ray if required
            //if (m_ShowDebugRay)
            //{
            //    Debug.DrawRay(m_Camera.position, m_Camera.forward * m_DebugRayLength, Color.blue, m_DebugRayDuration);
            //}
            gazeData = fint.GetGazeConvergence();
            gaze_origin = gazeData.ray.origin;
            gaze_direction = gazeData.ray.direction;
            gaze_distance = gazeData.distance;

            if (m_ShowDebugRay) {
                Debug.DrawRay(gaze_origin, gaze_direction * m_DebugRayLength, Color.blue, m_DebugRayDuration);
            }
            // Create a ray that points forwards from the camera.
            //Ray ray = new Ray(m_Camera.position, m_Camera.forward);
            // Ray that points at gaze
            Ray ray = new Ray(gaze_origin, gaze_direction);
            //RaycastHit hit;
            RaycastHit[] hit;
            // Do the raycast forweards to see if we hit an interactive item
            //if (Physics.Raycast(ray, out hit, m_RayLength, ~m_ExclusionLayers)) {
            hit = Physics.RaycastAll(ray, 20f, ~m_ExclusionLayers).OrderBy(h => h.distance).ToArray();
            if (hit.Length > 0) {

                
                VRInteractiveItem interactible = hit[0].collider.GetComponent<VRInteractiveItem>(); //attempt to get the VRInteractiveItem on the hit object

                // If we hit an interactive item and it's not the same as the last interactive item, then call Out and then Over
                if (interactible && interactible != m_LastInteractible) {
                    DeactiveLastInteractible();
                    m_CurrentInteractible = null;
                    interactible.Over();
                    //Debug.Log("not the same itneractive item.");
                }
                m_CurrentInteractible = interactible;

                // Deactive the last interactive item 
                if (interactible != m_LastInteractible)
                    DeactiveLastInteractible();

                m_LastInteractible = interactible;


                //Something was hit, set at the hit position.
                if (m_Reticle)
                    m_Reticle.SetPosition(hit[0]);

                //Debug.Log("Layer: " + hit[0].collider.gameObject.layer);
                // Send hit to GoToGaze interface if activated, exclude obstacle (15) and UI (5) layer 
                if (ci.ci2_GoToGazeActive) {
                    //Debug.Log("GTG test");
                    if (hit[0].collider.gameObject.layer == 15) // Obstacle layer
                        m_Reticle.reticleColorHit(false);

                    if (hit[0].collider.gameObject.layer != 15 && hit[0].collider.gameObject.layer != 5) {
                        m_Reticle.reticleColorHit(true);
                        gtg.GoToGazeUpdate(hit[0]);
                    }
                    else m_SelectionRadial.FillSelectionRadial(0f);
                }
                // if GazeVectorControl is activated, send hit if hitting groundlayer (13)
                else if (ci.ci3_GazeVectorControl) {
                    //Debug.Log("GVC test");

                    if (hit[0].collider.gameObject.layer == 5) {
                        if (hit[0].collider.gameObject.tag == "GVFREE") {
                            m_SelectionRadial.SelectionDuration = 0.01f;
                            if (gvc.GetVertAcc() > 0f)
                                gvc.ResetAcceleration();
                        }
                    }
                    else {
                        for (int i = 0; i < hit.Length; ++i) {
                            //Debug.Log("test: " + i + " layer " + hit[i].collider.gameObject.layer);
                            if (hit[i].collider.gameObject.layer == 13 || hit[i].collider.gameObject.layer == 9) {
                                m_Reticle.reticleColorHit(true);
                                gvc.GazeVectorUpdate(hit[i]);
                            }
                            //if (i == hit.Length - 1) {
                            //    m_Reticle.reticleColorHit(false);
                            //    m_SelectionRadial.FillSelectionRadial(0f);
                            //    gvc.ResetAcceleration();
                            //}
                        }
                    }
                }
                if (OnRaycasthit != null)
                    OnRaycasthit(hit[0]);

            }
            else {
                //Debug.Log("Nothing was hit");
                // Nothing was hit, deactive the last interactive item.
                DeactiveLastInteractible();
                m_CurrentInteractible = null;

                // Position the reticle at default distance.
                if (m_Reticle)
                    m_Reticle.SetPosition(gaze_origin, gaze_direction, gaze_distance);

                m_Reticle.reticleColorHit(false);
            }
        }


        private void DeactiveLastInteractible() {
            if (m_LastInteractible == null)
                return;

            m_LastInteractible.Out();
            m_LastInteractible = null;
        }


        private void HandleUp() {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.Up();
        }


        private void HandleDown() {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.Down();
        }


        private void HandleClick() {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.Click();
        }


        private void HandleDoubleClick() {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.DoubleClick();

        }

    }
}