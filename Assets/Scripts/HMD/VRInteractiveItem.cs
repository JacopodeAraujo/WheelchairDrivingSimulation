using System;
using UnityEngine;

namespace VRStandardAssets.Utils
{
    // This class should be added to any gameobject in the scene
    // that should react to input based on the user's gaze.
    // It contains events that can be subscribed to by classes that
    // need to know about input specifics to this gameobject.
    public class VRInteractiveItem : MonoBehaviour
    {
        public event Action OnOver;             // Called when the gaze moves over this object
        public event Action OnOut;              // Called when the gaze leaves this object
        public event Action OnClick;            // Called when click input is detected whilst the gaze is over this object.
        public event Action OnDoubleClick;      // Called when double click input is detected whilst the gaze is over this object.
        public event Action OnUp;               // Called when Fire1 is released whilst the gaze is over this object.
        public event Action OnDown;             // Called when Fire1 is pressed whilst the gaze is over this object.

        private SelectionRadial m_selectionRadial;

        protected bool m_IsOver;

        bool overflag = false;

        private void Start() {
            m_selectionRadial = FindObjectOfType<SelectionRadial>();
        }


        public bool IsOver
        {
            get { return m_IsOver; }              // Is the gaze currently over this object?
        }

        public bool Overflag { get => overflag; set => overflag = value; }


        // The below functions are called by the VREyeRaycaster when the appropriate input is detected.
        // They in turn call the appropriate events should they have subscribers.
        public void Over()
        {
            if (!Overflag) {
                m_selectionRadial.Show();
                Overflag = true;
            }
            m_selectionRadial.HandleOver();

            m_IsOver = true;
            if (OnOver != null)
                OnOver();
        }


        public void Out()
        {
            m_selectionRadial.HandleOut();
            m_selectionRadial.Hide();
            Overflag = false;

            m_IsOver = false;

            if (OnOut != null)
                OnOut();
        }


        public void Click()
        {
            if (OnClick != null)
                OnClick();
        }


        public void DoubleClick()
        {
            if (OnDoubleClick != null)
                OnDoubleClick();
        }


        public void Up()
        {
            if (OnUp != null)
                OnUp();
        }


        public void Down()
        {
            if (OnDown != null)
                OnDown();
        }

        public GazeVectorControl GazeVectorControl
        {
            get => default;
            set
            {
            }
        }

        public UI_GazeControl UI_GazeControl
        {
            get => default;
            set
            {
            }
        }

        public GoToGazeInterface GoToGazeInterface
        {
            get => default;
            set
            {
            }
        }
    }
}