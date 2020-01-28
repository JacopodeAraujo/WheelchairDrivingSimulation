using UnityEngine;
using EXP_StateMachine;

public class State1 : State<StateLogic> {

    private static State1 _instance;
    private int stateTime = 5;

    private State1() {
        if (_instance != null)
            return;

        _instance = this;
    }

    public static State1 Instance
    {
        get
        {
            if (_instance == null)
                new State1();
            return _instance;
        }
    }

    public override void EnterState(StateLogic _owner) {
        Debug.Log("Entering State1");
        _owner.stateTransitionTime = stateTime;
        _owner.c_trial++;

        _owner.wheelchair.transform.position = _owner.startPosition;
        _owner.wheelchair.transform.localEulerAngles = _owner.startOrientation;

    }

    public override void InState(StateLogic _owner) {
        if (_owner.seconds >= 1) {
            _owner.state5IncorrectAnimation.SetActive(false);
            _owner.state5CorrectAnimation.SetActive(false);
        }
        _owner.wheelchair.transform.Translate(Vector3.forward * 2*Time.deltaTime);
    }
    public override void ExitState(StateLogic _owner) {
        Debug.Log("Exiting State1");
    }

    public override void UpdateState(StateLogic _owner) {
        if (_owner.switchState)
            _owner.stateMachine.ChangeState(State2.Instance);
        else _owner.stateMachine.ChangeState(this);
    }


    
}