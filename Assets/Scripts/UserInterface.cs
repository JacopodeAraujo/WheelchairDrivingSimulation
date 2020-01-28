using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VRStandardAssets.Utils;

public class UserInterface : Selectable
{
    //Use this to check what Events are happening
    BaseEventData m_BaseEvent;
    int highlightCount = 0;

    public float transparency_c = 0.2f;

    Color normal_c;
    Color high_c;
    Color pressed_c;
    Color disabled_c;

    public Button ci1_b;
    public Button ci2_b;
    

    public ControlInterface control;
    public UI_GazeControl gazecontrol;
    public GazeVectorControl gvc;
    public Bci_control bci;
    //public Bci_control bci2;

    //public UI_GazeControlBrakes gazecontrolbrakes;

    protected override void Start()
    {
        //bci2 = GameObject.Find("BrainControl2").GetComponent<Bci_control>();

        normal_c = colors.normalColor;
        high_c = colors.highlightedColor;
        pressed_c = colors.pressedColor;
        disabled_c = colors.disabledColor;
        normal_c.a = transparency_c;

        ButtonSelected(ci1_b, false);
    }

#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
    private void OnEnable() {
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        if (GetComponent<VRInteractiveItem>()) {
            GetComponent<VRInteractiveItem>().OnOver += HandleOver;
            GetComponent<VRInteractiveItem>().OnOut += HandleOut;
            GetComponent<VRInteractiveItem>().OnClick += HandleClick;
        }
    }


#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
    private void OnDisable() {
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        if (GetComponent<VRInteractiveItem>()) {
            GetComponent<VRInteractiveItem>().OnOver -= HandleOver;
            GetComponent<VRInteractiveItem>().OnOut -= HandleOut;
            GetComponent<VRInteractiveItem>().OnClick -= HandleClick;
        }
    }

    public void ButtonSelected(Button b, bool gaze_on)
    { 
        Color ncb;
        // Green if gaze is on, red if gaze is off
        if (gaze_on)
            ncb = pressed_c;
        else ncb = disabled_c;

        ncb.a = 0.3f;
        ColorBlock cb = b.colors;
        cb.normalColor = ncb;
        b.colors = cb;

        if (b == ci1_b)
            ButtonDeselected(ci2_b);
        else if (b == ci2_b)
            ButtonDeselected(ci1_b);
        else return;
    }
    public void ButtonDeselected(Button b)
    {
        Color ncb = normal_c;
        ColorBlock cb = b.colors;
        cb.normalColor = ncb;
        b.colors = cb;
    }
    public ColorBlock ButtonColor(ColorBlock cb, Color c, int ColorBlockInteger, float transparency)
    {
        Color ncb = c;
        ncb.a = transparency;
        if (ColorBlockInteger == 1) cb.normalColor = ncb;
        else if (ColorBlockInteger == 2) cb.highlightedColor = ncb;
        else if (ColorBlockInteger == 3) cb.pressedColor = ncb;
        else if (ColorBlockInteger == 4) cb.disabledColor = ncb;
        else Debug.Log("Colorblock integer has to be 1=normal, 2=highlighted, 3=pressed, 4=disabled ");
        return cb;
    }

    public void ShowGazeControl(bool _enabled)
    {
        //if (_enabled)
        //{
        //    colors = ButtonColor(colors, pressed_c, 2, 1f);
        //    ci1_b.colors = ButtonColor(ci1_b.colors, pressed_c, 1, 0.5f);
        //}
        //else
        //{
        //    colors = ButtonColor(colors, disabled_c, 2, 1f);
        //    ci1_b.colors = ButtonColor(ci1_b.colors, disabled_c, 1, 0.5f);
        //}
        gazecontrol.enabled = _enabled;
        gazecontrol.gameObject.SetActive(_enabled);
        //gazecontrolbrakes.gameObject.SetActive(_enabled);
    }
    public void ShowGazeVectorControl(bool _enabled) {
        gvc.enabled = _enabled;
        gvc.gameObject.SetActive(_enabled);
    }
    public void ShowBCIControl1(bool _enabled) {
        bci.enabled = _enabled;
        bci.gameObject.SetActive(_enabled);
    }
    public void ShowBCIControl2(bool _enabled) {
        //bci2.enabled = _enabled;
        //bci2.gameObject.SetActive(_enabled);
    }
    public void UI_Highlight(bool _isOn) {
        DoStateTransition(SelectionState.Highlighted, _isOn);
    }
    public void UI_Normal(bool _isOn) {
        DoStateTransition(SelectionState.Normal, _isOn);
    }
    public void UI_Pressed(bool _isOn) {
        DoStateTransition(SelectionState.Pressed, _isOn);
    }
    public void UI_disabled(bool _isOn) {
        DoStateTransition(SelectionState.Disabled, _isOn);
    }
    void FixedUpdate()
    {
        // If the User Interface is gazed upon
        if (IsHighlighted(m_BaseEvent) == true)
        {
            // Keep track of for how many frames (how much time) it is gazed on
            highlightCount++;
            // First action after certain gaze time
            if (highlightCount == 30)
                HandleClick();

            if (highlightCount > 35)
                colors = ButtonColor(colors, high_c, 2, 1f);            
        }

        //Check if the GameObject is NOT highlighted
        if (IsHighlighted(m_BaseEvent) == false)
        {

           // Debug.Log("Selectable is not Highlighted");
            highlightCount = 0;

            ci1_b.GetComponentInChildren<Text>().enabled = false;
            ci2_b.GetComponentInChildren<Text>().enabled = false;


        }

    }


    public void HandleOver() {
        GetComponent<UserInterface>().UI_Normal(false);
        GetComponent<UserInterface>().UI_Highlight(true);
    }

    public void HandleOut() {
        GetComponent<UserInterface>().UI_Highlight(false);
        GetComponent<UserInterface>().UI_Normal(true);
    }

    public void HandleClick() {
        // Make CI buttons visable
        ci1_b.GetComponentInChildren<Text>().enabled = true;
        ci2_b.GetComponentInChildren<Text>().enabled = true;

        // If CI2 is active, then toggle GoToGaze and correspondingly flash button color
        if (control.ci2Active) {
            if (control.ci2_GoToGazeActive) {
                colors = ButtonColor(colors, disabled_c, 2, 1f);
                ci2_b.colors = ButtonColor(ci2_b.colors, disabled_c, 1, 0.5f);
                control.CI2_GoToGaze(false);
            }
            else if (control.ci2_GoToGazeActive == false) {
                colors = ButtonColor(colors, pressed_c, 2, 1f);
                ci2_b.colors = ButtonColor(ci2_b.colors, pressed_c, 1, 0.5f);
                control.CI2_GoToGaze(true);
            }
            else Debug.Log("Highlight.ci2active.else error");
        }
        // If CI1 is active, then toggle Gaze control and correspondingly flash button color
        if (control.ci1Active) {
            if (control.ci1_GazeControl) {
                // move to ShowGazeControl?
                colors = ButtonColor(colors, disabled_c, 2, 1f);
                ci1_b.colors = ButtonColor(ci1_b.colors, disabled_c, 1, 0.5f);
                control.CI1_GazeControl(false);
                control.CI1_BrainControl(true);
            }
            else if (control.ci1_GazeControl == false) {
                colors = ButtonColor(colors, pressed_c, 2, 1f);
                ci1_b.colors = ButtonColor(ci1_b.colors, pressed_c, 1, 0.5f);
                control.CI1_GazeControl(true);
                control.CI1_BrainControl(false);
            }
            else Debug.Log("Highlight.ci2active.else error");
        }

        // If CI3 is active, then toggle Gaze Vector control and correspondingly flash button color
        if (control.ci3Active) {
            if (control.ci3_GazeVectorControl) {
                // move to ShowGazeControl?
                colors = ButtonColor(colors, disabled_c, 2, 1f);
                ci1_b.colors = ButtonColor(ci1_b.colors, disabled_c, 1, 0.5f);
                control.CI3_GazeVectorControl(false);
                control.CI3_BrainControl2(true); // Should be CI3_BrainControl2 when developed
            }
            else if (control.ci3_GazeVectorControl == false) {
                colors = ButtonColor(colors, pressed_c, 2, 1f);
                ci1_b.colors = ButtonColor(ci1_b.colors, pressed_c, 1, 0.5f);
                control.CI3_GazeVectorControl(true);
                control.CI3_BrainControl2(false); // Should be CI3_BrainControl2 when developed
            }
            else Debug.Log("Highlight.ci3active.else error");
        }
    }
}
