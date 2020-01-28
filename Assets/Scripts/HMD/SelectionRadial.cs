using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace VRStandardAssets.Utils
{
    // This class is used to control a radial bar that fills
    // up as the user holds down the Fire1 button.  When it has
    // finished filling it triggers an event.  It also has a
    // coroutine which returns once the bar is filled.
    public class SelectionRadial : MonoBehaviour
    {
        public event Action OnSelectionComplete;                                                // This event is triggered when the bar has filled.


        [SerializeField] private float m_SelectionDuration = 2f;                                // How long it takes for the bar to fill.
        [SerializeField] private bool m_HideOnStart = true;                                     // Whether or not the bar should be visible at the start.
        [SerializeField] private Image m_Selection;                                             // Reference to the image who's fill amount is adjusted to display the bar.
        //[SerializeField] private VRInput m_VRInput;                                             // Reference to the VRInput so that input events can be subscribed to.
        //[SerializeField] private VRInteractiveItem m_VRInteractive;                                             // Reference to the any interactable item so that input events can be subscribed to.

        private Coroutine m_SelectionFillRoutine;                                               // Used to start and stop the filling coroutine based on input.
        private bool m_IsSelectionRadialActive;                                                    // Whether or not the bar is currently useable.
        private bool m_RadialFilled;                                                               // Used to allow the coroutine to wait for the bar to fill.


        public float SelectionDuration { get { return m_SelectionDuration; } set { m_SelectionDuration = value; } }

        public GoToGazeInterface GoToGazeInterface
        {
            get => default;
            set
            {
            }
        }



        //private void OnEnable()
        //{
        //    //m_VRInput.OnDown += HandleDown; // Subscription to input events
        //    //m_VRInput.OnUp += HandleUp;
        //    m_VRInteractive.OnOver += HandleOver;
        //    m_VRInteractive.OnOut += HandleOut;
        //}


        //private void OnDisable()
        //{
        //    //m_VRInput.OnDown -= HandleDown;
        //    //m_VRInput.OnUp -= HandleUp;
        //    m_VRInteractive.OnOver -= HandleOver;
        //    m_VRInteractive.OnOut -= HandleOut;
        //}


        private void Start()
        {
            // Setup the radial to have no fill at the start and hide if necessary.
            m_Selection.fillAmount = 0f;

            if(m_HideOnStart)
                Hide();
        }


        public void Show()
        {
            m_Selection.gameObject.SetActive(true);
            m_IsSelectionRadialActive = true;
        }


        public void Hide()
        {
            m_Selection.gameObject.SetActive(false);
            m_IsSelectionRadialActive = false;

            // This effectively resets the radial for when it's shown again.
            m_Selection.fillAmount = 0f;            
        }


        private IEnumerator FillSelectionRadial()
        {
            // At the start of the coroutine, the bar is not filled.
            m_RadialFilled = false;

            // Create a timer and reset the fill amount.
            float timer = 0f;
            m_Selection.fillAmount = 0f;
            
            // This loop is executed once per frame until the timer exceeds the duration.
            while (timer < m_SelectionDuration)
            {
                // The image's fill amount requires a value from 0 to 1 so we normalise the time.
                m_Selection.fillAmount = timer / m_SelectionDuration;

                // Increase the timer by the time between frames and wait for the next frame.
                timer += Time.deltaTime;
                yield return null;
            }

            // When the loop is finished set the fill amount to be full.
            m_Selection.fillAmount = 1f;

            // Turn off the radial so it can only be used once.
            m_IsSelectionRadialActive = false;

            // The radial is now filled so the coroutine waiting for it can continue.
            m_RadialFilled = true;

            // Change color to green
            SelectionCompleteColor(true);

            // If there is anything subscribed to OnSelectionComplete call it.
            if (OnSelectionComplete != null)
                OnSelectionComplete();
        }

        public void FillSelectionRadial(float p_progress) {
            // The show and hide shall be externally called before and after completion of the fill
            if (p_progress > 1f)
                Debug.Log("Selection Radial amount shall be percentage");
            if (p_progress < 0.75f)
                SelectionCompleteColor(false);
            if (p_progress >= 0.75)
                SelectionCompleteColor(true);

            m_Selection.fillAmount = p_progress;

            if (p_progress == 1f) {
                // Turn off the radial so it can only be used once.
                m_IsSelectionRadialActive = false;

                // The radial is now filled so the coroutine waiting for it can continue.
                m_RadialFilled = true;

                // Change color to green
                SelectionCompleteColor(true);
            }

        }

        public void SelectionCompleteColor(bool complete) {
            Color c;
            if (complete)
                c = Color.green;
            else c = Color.red;

            m_Selection.color = c;
        }
        public IEnumerator WaitForSelectionRadialToFill ()
        {
            // Set the radial to not filled in order to wait for it.
            m_RadialFilled = false;

            // Make sure the radial is visible and usable.
            Show ();

            // Check every frame if the radial is filled.
            while (!m_RadialFilled)
            {
                yield return null;
            }

            // Once it's been used make the radial invisible.
            Hide ();
        }


        public void HandleOver()
        {
            // If the radial is active start filling it.
            if (m_IsSelectionRadialActive)
            {
                m_SelectionFillRoutine = StartCoroutine(FillSelectionRadial());
            }
        }


        public void HandleOut()
        {
            SelectionCompleteColor(false);
            // If the radial is active stop filling it and reset it's amount.
            if (m_IsSelectionRadialActive)
            {
                if(m_SelectionFillRoutine != null)
                    StopCoroutine(m_SelectionFillRoutine);

                m_Selection.fillAmount = 0f;
                
            }
        }
    }
}