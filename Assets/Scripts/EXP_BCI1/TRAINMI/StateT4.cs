using UnityEngine;
using EXP_StateMachine;

public class StateT4 : State<StateLogic> {

    private static StateT4 _instance;
    private int stateTime = 8;

    private StateT4() {
        if (_instance != null)
            return;

        _instance = this;
    }

    public static StateT4 Instance
    {
        get
        {
            if (_instance == null)
                new StateT4();
            return _instance;
        }
    }

    public override void EnterState(StateLogic _owner) {
        Debug.Log("Entering StateT4");
        _owner.stateTransitionTime = stateTime;
        _owner.stateT4ani.SetActive(true);
    }
    public override void InState(StateLogic _owner) {
        //Debug.Log("In State");
    }

    public override void ExitState(StateLogic _owner) {
        Debug.Log("Exiting StateT4");
        _owner.stateT4ani.SetActive(false);
        _owner.WrapUpLevel();
    }
    public override void UpdateState(StateLogic _owner) {
        if (_owner.switchState)
            _owner.stateMachine.ChangeState(State1.Instance);
        else _owner.stateMachine.ChangeState(this);

    }

}
