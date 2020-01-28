using UnityEngine;
using UnityEngine.UI;
using EXP_StateMachine;

public class State2 : State<StateLogic> {

    private static State2 _instance;
    private int stateTime = 10;
    GameObject image_left;
    GameObject image_right;
    GameObject textLeftImage;
    GameObject textRightImage;
    GameObject image_arrow;
    GameObject textIn;
    GameObject textSec;
    GameObject textSecVar;

    bool flagImage = false;

    int MI_movement = 0;
    int MI_movement_comp = 0;

    private bool turningRight = false;

    public bool TurningRight { get => turningRight; set => turningRight = value; }

    private State2() {
        if (_instance != null)
            return;

        _instance = this;
    }

    public static State2 Instance
    {
        get
        {
            if (_instance == null)
                new State2();
            return _instance;
        }
    }

    public override void EnterState(StateLogic _owner) {
        Debug.Log("Entering State2");
        _owner.stateTransitionTime = stateTime;
        _owner.state2animation.SetActive(true);

        // Using Find function is kinda slow and vulnerable to name changes of the objects, so make sure the names here match the gameobjects in the unity editor
        image_left = _owner.state2animation.transform.Find("imageLeft").gameObject;
        image_right = _owner.state2animation.transform.Find("imageRight").gameObject;
        textLeftImage = _owner.state2animation.transform.Find("text_imageLeft").gameObject;
        textRightImage = _owner.state2animation.transform.Find("text_imageRight").gameObject;
        image_arrow = _owner.state2animation2.transform.Find("Image_direction").gameObject;
        textIn = _owner.state2animation2.transform.Find("text_in").gameObject;
        textSec = _owner.state2animation2.transform.Find("text_seconds").gameObject;
        textSecVar = _owner.state2animation2.transform.Find("text_secVar").gameObject;

        MI_movement = _owner.GetMIMovement(false);
        MI_movement_comp = _owner.GetComplementaryMIMovement();

        // Set arrow direction image
        image_arrow.GetComponent<ImageToggleDir>().SetImage(turningRight);

        if (turningRight) {
            _owner.GetComponent<ImageToggle>().SetImage(image_right, MI_movement);
            _owner.GetComponent<ImageToggle>().SetImage(image_left, MI_movement_comp);
            _owner.GetComponent<ImageToggle>().SetImageText(textRightImage, MI_movement);
            _owner.GetComponent<ImageToggle>().SetImageText(textLeftImage, MI_movement_comp);
        }
        else {
            _owner.GetComponent<ImageToggle>().SetImage(image_left, MI_movement);
            _owner.GetComponent<ImageToggle>().SetImage(image_right, MI_movement_comp);
            _owner.GetComponent<ImageToggle>().SetImageText(textLeftImage, MI_movement);
            _owner.GetComponent<ImageToggle>().SetImageText(textRightImage, MI_movement_comp);
        }

        textIn.gameObject.SetActive(true);
        textSec.SetActive(true);
        textSecVar.GetComponent<Text>().text = "X";
        textSecVar.GetComponent<Text>().color = Color.black;
    }
    public override void InState(StateLogic _owner) {
        
        if ((Time.time / Mathf.RoundToInt(Time.time) - 1f) > 0) {
            if (flagImage) {
                _owner.GetComponent<ImageToggle>().ToggleImage(image_right);
                _owner.GetComponent<ImageToggle>().ToggleImage(image_left);
                flagImage = false;
            }
        }
        else {
            if (!flagImage) {
                _owner.GetComponent<ImageToggle>().ToggleImage(image_right);
                _owner.GetComponent<ImageToggle>().ToggleImage(image_left);
                flagImage = true;
            }
        }
        if (_owner.seconds == 4) {
            _owner.state2animation2.SetActive(true);
            if (turningRight)
                _owner.state2animation2.transform.Find("text_direction").GetComponent<Text>().text = "TURN RIGHT";
            else _owner.state2animation2.transform.Find("text_direction").GetComponent<Text>().text = "TURN LEFT";
        }
        if (_owner.seconds >= 5)
            _owner.state2animation2.transform.Find("text_secVar").GetComponent<Text>().text = (stateTime - _owner.seconds).ToString();


    }
    public override void ExitState(StateLogic _owner) {
        Debug.Log("Exiting State2");
        textIn.gameObject.SetActive(false);
        textSec.SetActive(false);
        textSecVar.GetComponent<Text>().text = "NOW";
        textSecVar.GetComponent<Text>().color = Color.green;
        
        _owner.state2animation.SetActive(false);
    }

    public override void UpdateState(StateLogic _owner) {
        if (_owner.switchState)
            _owner.stateMachine.ChangeState(State3.Instance);
        else _owner.stateMachine.ChangeState(this);
    }

}
