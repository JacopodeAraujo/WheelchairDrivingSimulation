using VRStandardAssets.Utils;
using UnityEngine;

public class CIButton : MonoBehaviour
{
    ControlInterface ci;


    void Start() {
        ci = FindObjectOfType<ControlInterface>();
    }

    private void OnEnable() {
        GetComponent<VRInteractiveItem>().OnClick += HandleClick;
    }


    private void OnDisable() { 
        GetComponent<VRInteractiveItem>().OnClick -= HandleClick;
    }

    void HandleClick() {
        //DoStateTransition(SelectionState.Pressed, true);
        if (gameObject.name == "CtrlInt1")
            ci.ActivateCtrlInterface1();
        else if (gameObject.name == "CtrlInt2")
            ci.ActivateCtrlInterface2();
        else if (gameObject.name == "CtrlInt3")
            ci.ActivateCtrlInterface3();
        else Debug.Log("Cannot find object");
    }

    //void HandleOnOut() {
    //    DoStateTransition(SelectionState.Normal, true);

    //}
    //void HandleOnOver() {
    //    DoStateTransition(SelectionState.Highlighted, true);

    //}
}
