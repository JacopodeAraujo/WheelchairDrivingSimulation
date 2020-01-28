using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Bci_control : MonoBehaviour
{
    private float vert_acc = 0f;
    private float hori_acc = 0f;
    private bool brake_flag = false;
    private List<int> lastCmd = new List<int> { 0, 0, 0 };
    public float turnAngle = 1f;
    public float turnAcc = 0.2f;
    public float acc = 0.3f;
    public float tresh3 = 0.90f;
    public float tresh4 = 0.80f;

    //reference for slider
    Slider Lefthandactivation, feetactivation, bothactivation, righthandactivation;
    Image fill_left, fill_right, fill_feet, fill_both;

    //private Communicationwithpython commscript;
    private readSocket commscript;
    GameObject gm;

    public bool ctrl3class = false;
    public bool valpre = false;
    public float tresh = 1f;
    public float maxindexval = 0;
    public float[] activations = new float[] { 0, 0, 0, 0 };
    public int command = 0;

    // Use this for initialization
    void Start() {
        gm = GameObject.Find("GameManagerWC");
        commscript = gm.GetComponent<readSocket>();

        Slider[] sliders = GetComponentsInChildren<Slider>();
        Image[] im = { null };
        foreach (Slider s in sliders) {
            if (s.gameObject.name.Contains("feet")) {
                feetactivation = s;
                im = feetactivation.GetComponentsInChildren<Image>();
                foreach (Image m in im) {
                    if (m.gameObject.name.Equals("Fill"))
                        fill_feet = m;
                }
            }
            else if (s.gameObject.name.Contains("right")) {
                righthandactivation = s;
                im = righthandactivation.GetComponentsInChildren<Image>();
                foreach (Image m in im) {
                    if (m.gameObject.name.Equals("Fill"))
                        fill_right = m;
                }
            }
            else if (s.gameObject.name.Contains("left")) {
                Lefthandactivation = s;
                im = Lefthandactivation.GetComponentsInChildren<Image>();
                foreach (Image m in im) {
                    if (m.gameObject.name.Equals("Fill"))
                        fill_left = m;
                }
            }
            else if (s.gameObject.name.Contains("rest")) {
                bothactivation = s;
                im = bothactivation.GetComponentsInChildren<Image>();
                foreach (Image m in im) {
                    if (m.gameObject.name.Equals("Fill"))
                        fill_both= m;
                }
            }
            else
                Debug.Log("Couldn't identify slider from gameObject name");
        }

        if (fill_both != null)
            fill_both.color = Color.red;
        if (fill_feet!= null)
            fill_feet.color = Color.red;
        if (fill_right!= null)
            fill_right.color = Color.red;
        if (fill_left!= null)
            fill_left.color = Color.red;

    }

    //Update is called once per frameir
    void Update() {
        // Admin control if help is necessary with controlling wheelchair
        if (Input.GetKey(KeyCode.T))
        {
            vert_acc = Input.GetAxis("Vertical");
            hori_acc = Input.GetAxis("Horizontal");
            if (Input.GetKey(KeyCode.Space))
                brake_flag = true;
            else brake_flag = false;
        }
        // BCI Control based on predictions and estimated class probabilities
        if (commscript.matlabcomm)
        {

            if (commscript.mdata.Cmd != 0) {
                activations[0] = commscript.mdata.Left;
                activations[1] = commscript.mdata.Right;
                activations[2] = commscript.mdata.Both;
                activations[3] = commscript.mdata.Feet;
                command = (int)commscript.mdata.Cmd;

                if (activations[2] == -1)
                    ctrl3class = true;
                else ctrl3class = false;

                if (ctrl3class)
                    tresh = tresh3;
                else tresh = tresh4;

                maxindexval = activations.Max();

                Debug.Log("Max: " + maxindexval.ToString());
                if ((maxindexval > tresh)) // || (command==lastCmd[0] && lastCmd[0]==lastCmd[1]))
                    valpre = true;
                else valpre = false;

                if (ctrl3class) {
                    Lefthandactivation.value = activations[0];
                    righthandactivation.value = activations[1];
                    bothactivation.value = 0f;
                    bothactivation.enabled = false;
                    feetactivation.value = activations[3];
                }
                else {
                    Lefthandactivation.value = activations[0];
                    righthandactivation.value = activations[1];
                    bothactivation.value = activations[2];
                    feetactivation.value = activations[3];
                }

                // 3-class
                if (ctrl3class) {
                    // LEFT
                    if ((Input.GetAxis("Horizontal") <= -0.30f) || (valpre && command == 1)) {
                        // Change slider color to green
                        fill_left.color = Color.green;
                        // Make sure brake flag is off
                        brake_flag = false;
                        vert_acc = 0f;
                        // Increase turn to the right
                        if (lastCmd[0] == 2)
                            hori_acc = 0f;
                        else  
                        {
                            hori_acc = -turnAngle;
                            vert_acc = turnAcc;
                        }
                        
                        // Store current command as last command for next update
                        lastCmd.Insert(0, command);
                    }
                    else fill_left.color = Color.red;

                    // RIGHT
                    if ((Input.GetAxis("Horizontal") >= 0.30f) || (valpre && command == 2)) {
                        // Change slider color to green
                        fill_right.color = Color.green;
                        // Make sure brake flag is off
                        brake_flag = false;
                        vert_acc = 0f;
                        // Increase turn to the right
                        if (lastCmd[0] == 1)
                            hori_acc = 0f;
                        else 
                        {
                            hori_acc = turnAngle;
                            vert_acc = turnAcc;
                        }
                        // Store current command as last command for next update
                        lastCmd.Insert(0, command);
                    }
                    else fill_right.color = Color.red;
            
                    // FORWARD
                    if ((Input.GetAxis("Vertical") >= 0.30f) || (valpre && command == 4)) {
                        // Change slider color to green
                        fill_feet.color = Color.green;
                        // Make sure brake flag is off
                        brake_flag = false;
                        // Drive straight
                        hori_acc = 0f;

                        vert_acc =acc;
                        // Store current command as last command for next update
                        lastCmd.Insert(0, command);
                    }
                    else fill_feet.color = Color.red;

                    if (!valpre) {
                        vert_acc = 0;
                        hori_acc = 0;
                        brake_flag = true;
                    }
                }

                // 4-class
                else {
                    // LEFT HAND
                    if ((Input.GetAxis("Horizontal") <= -0.30f) || (valpre  && command == 1)) {
                        // Change slider color to green
                        fill_left.color = Color.green;
                        // Make sure brake flag is off
                        brake_flag = false;
                        // Stop acceleration
                        vert_acc = 0f;
                        // Increase turn to the right
                        if (lastCmd[0] == 2)
                            hori_acc = 0f;
                        else 
                        {
                            hori_acc = -turnAngle;
                            vert_acc = turnAcc;
                        }
                        // Store current command as last command for next update
                        lastCmd.Insert(0, command);
                    }
                    else fill_left.color = Color.red;

                    // RIGHT HAND
                    if ((Input.GetAxis("Horizontal") >= 0.30f ) || (valpre && command == 2)) {
                        // Change slider color to green
                        fill_right.color = Color.green;
                        // Make sure brake flag is off
                        brake_flag = false;
                        // Stop acceleration
                        vert_acc = 0f;
                        // Increase turn to the right
                        if (lastCmd[0] == 1)
                            hori_acc = 0f;
                        else {
                            hori_acc = turnAngle;
                            vert_acc = turnAcc;
                        }
                        // Store current command as last command for next update
                        lastCmd.Insert(0, command);
                    }
                    else fill_right.color = Color.red;
            
                    // BOTH HANDS
                    if ((Input.GetAxis("Vertical") <= -0.30f ) || (valpre && command == 4)) {
                        // Change slider color to green
                        fill_both.color = Color.green;
                        // Make sure brake flag is off
                        brake_flag = false;
                        // Drive straight
                        hori_acc = 0f;
                        // If last 2 predictions were acceleration, then reset speed to prevent crashing
                        if (lastCmd[0] == 3 && lastCmd[1] == 3 && lastCmd[2] == 3) {
                            vert_acc = 0f;  
                            lastCmd.Insert(0, 0);
                        }
                        else {
                            vert_acc = acc;
                            // Store current command as last command for next update
                            lastCmd.Insert(0, command);
                        }
                    }
                    else fill_feet.color = Color.red;

                    // LEGS
                    if ((Input.GetAxis("Vertical") >= 0.30f ) || (valpre && command == 3)) {
                        // Change slider color to green
                        fill_feet.color = Color.green;
                        // Brake
                        vert_acc = 0f;
                        hori_acc = 0f;
                        brake_flag = true;
                        lastCmd.Insert(0, command);
                    }
                    else fill_both.color = Color.red;

                    if (!valpre)
                        vert_acc=0f;
                }

            }
        }
    }

    public float GetVertAcc() {
        return vert_acc;
    }
    public float GetHoriAcc() {
        return hori_acc;
    }
    public bool isBraking() {
        return brake_flag;
    }
}
