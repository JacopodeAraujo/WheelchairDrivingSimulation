using UnityEngine;
using EXP_StateMachine;

public class State5 : State<StateLogic> {
    private static State5 _instance;
    private int stateTime = 6;

    Vector3 from = Vector3.zero;
    Vector3 to = Vector3.zero;

    public float rotateIncrement = 1f;
    private bool turnRight = true;
    private bool answerRight = true;

    int openvibeKeypressSimulator;
    private State5() {
        if (_instance != null)
            return;

        _instance = this;
    }

    public static State5 Instance
    {
        get
        {
            if (_instance == null)
                new State5();
            return _instance;
        }
    }

    public bool TurningRight { get => turnRight; set => turnRight = value; }
    public bool AnswerRight { get => answerRight; set => answerRight = value; }

    public override void EnterState(StateLogic _owner) {
        Debug.Log("Entering State5");
        _owner.stateTransitionTime = stateTime;

        from = _owner.transform.localEulerAngles;

        // XNOR Logic: If subject wants to go right, and his answer is accepted -> He goes right. 
        // If subject wants to go left, and his answer is not accepted -> he goes right.
        // Else, he is going left

        if (turnRight == answerRight)
            to = new Vector3(0f, 135f, 0f); // Right turn
        else
            to = new Vector3(0f, 45f, 0f); // Left turn

        
        if (answerRight)
            openvibeKeypressSimulator = 5;
        else
            openvibeKeypressSimulator = 6;
        _owner.LSLMarkerStart("EMO START: Satisfied - "+answerRight, openvibeKeypressSimulator);
        _owner.AddCurrentTimeToEmoList();
    }
    public override void InState(StateLogic _owner) {

        Vector3 r = _owner.wheelchair.transform.localEulerAngles;

        if (turnRight == answerRight && r.y < to.y) {
             r.y += rotateIncrement;
            _owner.wheelchair.transform.localEulerAngles = r;
        }
        else if (turnRight != answerRight && r.y > to.y) {
            r.y -= rotateIncrement;
            _owner.wheelchair.transform.localEulerAngles = r;
        }
        else {
            _owner.wheelchair.transform.Translate(Vector3.forward * Time.deltaTime);
            //Debug.Log("Moving");
        }

        if (_owner.seconds >= 3) {
            if (answerRight)
                _owner.state5CorrectAnimation.SetActive(true);  
            else
                _owner.state5IncorrectAnimation.SetActive(true);
        }

    }
    public override void ExitState(StateLogic _owner) {
        Debug.Log("Exiting State5");
        if (answerRight)
            _owner.state5CorrectAnimation.SetActive(false);
        else
            _owner.state5IncorrectAnimation.SetActive(false);
        _owner.LSLMarkerStop("EMO STOP: Satisfied - " + answerRight, openvibeKeypressSimulator);
    }

    public override void UpdateState(StateLogic _owner) {
        if (_owner.switchState) {
            if (_owner.CheckIfNextTrial()) 
                _owner.stateMachine.ChangeState(State1.Instance);
            else {
                _owner.WrapUpLevel();
                _owner.stateMachine.ChangeState(StateT1.Instance);
            }
        }
        else _owner.stateMachine.ChangeState(this);


    }


}
