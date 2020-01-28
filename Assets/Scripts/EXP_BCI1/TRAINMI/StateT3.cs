using UnityEngine;
using UnityEngine.UI;
using EXP_StateMachine;

public class StateT3 : State<StateLogic> {

    private static StateT3 _instance;
    private int stateTime = 5;

    GameObject im;
    GameObject text_im;
    GameObject t_sec;

    bool flagImage;

    private StateT3() {
        if (_instance != null)
            return;

        _instance = this;
    }

    public static StateT3 Instance
    {
        get
        {
            if (_instance == null)
                new StateT3();
            return _instance;
        }
    }

    public override void EnterState(StateLogic _owner) {
        Debug.Log("Entering StateT3");
        _owner.stateTransitionTime = stateTime;
        _owner.AddCurrentTimeToMItrainingList();
        _owner.LSLMarkerStart("ME START: " + (_owner.GetMIMovement(true)+6), (_owner.GetMIMovement(true)+6));
        _owner.stateT3ani.SetActive(true);

        im = _owner.stateT3ani.transform.Find("Image").gameObject;
        text_im = _owner.stateT3ani.transform.Find("imageText").gameObject;
        t_sec = _owner.stateT3ani.transform.Find("textSecVar").gameObject;

        _owner.GetComponent<ImageToggle>().SetImage(im, _owner.GetMIMovement(true));
        _owner.GetComponent<ImageToggle>().SetImageText(text_im, _owner.GetMIMovement(true));
    }
    public override void InState(StateLogic _owner) {
        //Debug.Log("In State");
        if ((Time.time / Mathf.RoundToInt(Time.time) - 1f) > 0) {
            if (flagImage) {
                _owner.GetComponent<ImageToggle>().ToggleImage(im);
                flagImage = false;
            }
        }
        else {
            if (!flagImage) {
                _owner.GetComponent<ImageToggle>().ToggleImage(im);
                flagImage = true;
            }
        }
        t_sec.GetComponent<Text>().text = (stateTime - _owner.seconds).ToString();
    }

    public override void ExitState(StateLogic _owner) {
        _owner.LSLMarkerStop("ME STOP: " + (_owner.GetMIMovement(true)+6), (_owner.GetMIMovement(true)+6));
        Debug.Log("Exiting StateT3");
        _owner.stateT3ani.SetActive(false);
    }
    public override void UpdateState(StateLogic _owner) {
        if (_owner.switchState) {
            if (_owner.CheckIfNextTrial())
                _owner.stateMachine.ChangeState(StateT2.Instance);
            else 
                _owner.stateMachine.ChangeState(StateT4.Instance);
        }
        else _owner.stateMachine.ChangeState(this);

    }

}
