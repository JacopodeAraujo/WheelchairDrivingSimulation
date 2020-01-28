using UnityEngine;
using UnityEngine.UI;
using EXP_StateMachine;

public class State3 : State<StateLogic> {
    private static State3 _instance;
    private int stateTime = 5;
    GameObject im_mi;
    GameObject txt_image;
    bool flagImage = false;

    private State3() {
        if (_instance != null)
            return;

        _instance = this;
    }

    public static State3 Instance
    {
        get
        {
            if (_instance == null)
                new State3();
            return _instance;
        }
    }

    public override void EnterState(StateLogic _owner) {
        Debug.Log("Entering State3");
        _owner.stateTransitionTime = stateTime;
        _owner.state3animation.SetActive(true);


        im_mi = _owner.state3animation.transform.Find("Image").gameObject;
        txt_image= _owner.state3animation.transform.Find("movement").gameObject;
        _owner.GetComponent<ImageToggle>().SetImage(im_mi, _owner.GetMIMovement(false));
        _owner.GetComponent<ImageToggle>().SetImageText(txt_image, _owner.GetMIMovement(false));

        _owner.LSLMarkerStart("MI START: " + _owner.GetMIMovement(false), _owner.GetMIMovement(false));
        _owner.AddCurrentTimeToMIList();
    }
    public override void InState(StateLogic _owner) {
        //Debug.Log("In State");
        if ((Time.time / Mathf.RoundToInt(Time.time) - 1f) > 0) {
            if (flagImage) {
                _owner.GetComponent<ImageToggle>().ToggleImage(im_mi);
                flagImage = false;
            }
        }
        else {
            if (!flagImage) {
                _owner.GetComponent<ImageToggle>().ToggleImage(im_mi);
                flagImage = true;
            }
        }
    }

    public override void ExitState(StateLogic _owner) {
        Debug.Log("Exiting State3");
        _owner.state3animation.SetActive(false);
        _owner.state2animation2.SetActive(false);

        _owner.LSLMarkerStop("MI STOP: " + _owner.GetMIMovement(false), _owner.GetMIMovement(false));
    }
    public override void UpdateState(StateLogic _owner) {
        if (_owner.switchState)
            _owner.stateMachine.ChangeState(State4.Instance);
        else _owner.stateMachine.ChangeState(this);

    }

}
