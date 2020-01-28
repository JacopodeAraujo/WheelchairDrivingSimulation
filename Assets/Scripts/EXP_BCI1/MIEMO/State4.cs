using UnityEngine;
using EXP_StateMachine;

public class State4 : State<StateLogic> {
    private static State4 _instance;
    private int stateTime = 5;

    private State4() {
        if (_instance != null)
            return;

        _instance = this;
    }

    public static State4 Instance
    {
        get
        {
            if (_instance == null)
                new State4();
            return _instance;
        }
    }

    public override void EnterState(StateLogic _owner) {
        Debug.Log("Entering State4");
        _owner.stateTransitionTime = stateTime;

        //_owner.LSLMarkerStart("Rest START", 5); // -- This event could potentially can be used for rest classification, in between MI and EMO. Omitted for simplicity
    }
    public override void InState(StateLogic _owner) {
        _owner.wheelchair.transform.Translate(Vector3.forward * Time.deltaTime);
    }
    public override void ExitState(StateLogic _owner) {
        Debug.Log("Exiting State4");
        //_owner.LSLMarkerStop("Rest STOP", 5); // -- This event could potentially can be used for rest classification, in between MI and EMO. Omitted for simplicity
    }

    public override void UpdateState(StateLogic _owner) {
        if (_owner.switchState)
            _owner.stateMachine.ChangeState(State5.Instance);
        else _owner.stateMachine.ChangeState(this);

    }

}
