using UnityEngine;
using EXP_StateMachine;

public class StateT1 : State<StateLogic> {

    private static StateT1 _instance;
    private int stateTime = 20;

    private StateT1() {
        if (_instance != null)
            return;

        _instance = this;
    }

    public static StateT1 Instance
    {
        get
        {
            if (_instance == null)
                new StateT1();
            return _instance;
        }
    }

    public override void EnterState(StateLogic _owner) {
        Debug.Log("Entering StateT1");
        _owner.stateTransitionTime = stateTime;
        _owner.stateT1ani.SetActive(true);

    }

    public override void InState(StateLogic _owner) {
        if (Input.GetMouseButton(0))
            _owner.stateTransitionTime = _owner.seconds + 1;
    }
    public override void ExitState(StateLogic _owner) {
        Debug.Log("Exiting StateT1");
        _owner.stateT1ani.SetActive(false);
    }

    public override void UpdateState(StateLogic _owner) {
        if (_owner.switchState)
            _owner.stateMachine.ChangeState(StateT2.Instance);
        else _owner.stateMachine.ChangeState(this);

    }
    
}