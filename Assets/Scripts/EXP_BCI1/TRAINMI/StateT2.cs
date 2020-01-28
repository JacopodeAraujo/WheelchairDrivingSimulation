using UnityEngine;
using UnityEngine.UI;
using EXP_StateMachine;

public class StateT2 : State<StateLogic> {

    private static StateT2 _instance;
    private int stateTime = 5;

    GameObject im;
    GameObject text_im;
    GameObject t_sec;
    private StateT2() {
        if (_instance != null)
            return;

        _instance = this;
    }

    public static StateT2 Instance
    {
        get
        {
            if (_instance == null)
                new StateT2();
            return _instance;
        }
    }

    public override void EnterState(StateLogic _owner) {
        Debug.Log("Entering StateT2");
        _owner.stateTransitionTime = stateTime;
        _owner.stateT2ani.SetActive(true);
        _owner.c_Ttrial++;

        im = _owner.stateT2ani.transform.Find("Image").gameObject;
        text_im = _owner.stateT2ani.transform.Find("imageText").gameObject;
        t_sec = _owner.stateT2ani.transform.Find("textSecVar").gameObject;

        _owner.GetComponent<ImageToggle>().SetImage(im, _owner.GetMIMovement(true));
        _owner.GetComponent<ImageToggle>().SetImageText(text_im, _owner.GetMIMovement(true));
    }
    public override void InState(StateLogic _owner) {
        t_sec.GetComponent<Text>().text = (stateTime - _owner.seconds).ToString();
    }
    public override void ExitState(StateLogic _owner) {
        Debug.Log("Exiting StateT2");
        _owner.stateT2ani.SetActive(false);
    }

    public override void UpdateState(StateLogic _owner) {
        if (_owner.switchState)
            _owner.stateMachine.ChangeState(StateT3.Instance);
        else _owner.stateMachine.ChangeState(this);

    }

}
