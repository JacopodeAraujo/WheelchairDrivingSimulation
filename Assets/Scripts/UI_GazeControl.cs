using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VRStandardAssets.Utils;
using Fove.Unity;

public class UI_GazeControl : MonoBehaviour
{
    public float acc_slight = 0.05f;
    public float acc_full = 1f;

    public float acc_F1 = 0.15f;
    public float acc_F2 = 0.3f;
    public float acc_F3 = 0.7f;

    public float acc_X1 = 0.15f;
    public float acc_X2 = 0.3f;
    public float acc_X3 = 0.7f;

    public float acc_B = 0.1f;

    float horizontal_acceleration = 0f;
    float vertical_acceleration = 0f;

    public Button m_F1, m_F2, m_F3, m_TL, m_L1, m_L2, m_L3, m_FL, m_TR, m_R1, m_R2, m_R3, m_FR, m_brake_R, m_C, m_L, m_R;

    public bool brake_flag;
    public StdCtrlInterface stdinterface;
    FoveInterface fint;

    private void OnEnable() {

        m_L.GetComponent<VRInteractiveItem>().OnClick += delegate { TaskOnClickL(); Brake(false); };
        m_L.GetComponent<VRInteractiveItem>().OnOut += delegate { TaskOnOutL(); horizontal_acceleration = 0; };
        m_R.GetComponent<VRInteractiveItem>().OnClick += delegate { TaskOnClickR(); Brake(false); };
        m_R.GetComponent<VRInteractiveItem>().OnOut += delegate { TaskOnOutR(); horizontal_acceleration = 0; };

        m_L1.GetComponent<BoxCollider>().enabled = false;
        m_L2.GetComponent<BoxCollider>().enabled = false;
        m_L3.GetComponent<BoxCollider>().enabled = false;

        m_R1.GetComponent<BoxCollider>().enabled = false;
        m_R2.GetComponent<BoxCollider>().enabled = false;
        m_R3.GetComponent<BoxCollider>().enabled = false;

        m_L1.GetComponent<VRInteractiveItem>().OnOver += delegate { horizontal_acceleration = -acc_X1; Brake(false); };
        m_L2.GetComponent<VRInteractiveItem>().OnOver += delegate { horizontal_acceleration = -acc_X2; Brake(false); };
        m_L3.GetComponent<VRInteractiveItem>().OnOver += delegate { horizontal_acceleration = -acc_X3; Brake(false); };
        m_R1.GetComponent<VRInteractiveItem>().OnOver += delegate { horizontal_acceleration = acc_X1; Brake(false); };
        m_R2.GetComponent<VRInteractiveItem>().OnOver += delegate { horizontal_acceleration = acc_X2; Brake(false); };
        m_R3.GetComponent<VRInteractiveItem>().OnOver += delegate { horizontal_acceleration = acc_X3; Brake(false); };

        //m_L1.GetComponent<VRInteractiveItem>().OnOut += delegate { horizontal_acceleration = 0; Brake(false); };
        //m_L2.GetComponent<VRInteractiveItem>().OnOut += delegate { horizontal_acceleration = 0; Brake(false); };
        //m_L3.GetComponent<VRInteractiveItem>().OnOut += delegate { horizontal_acceleration = 0; Brake(false); };
        //m_R1.GetComponent<VRInteractiveItem>().OnOut += delegate { horizontal_acceleration = 0; Brake(false); };
        //m_R2.GetComponent<VRInteractiveItem>().OnOut += delegate { horizontal_acceleration = 0; Brake(false); };
        //m_R3.GetComponent<VRInteractiveItem>().OnOut += delegate { horizontal_acceleration = 0; Brake(false); };



        m_F1.GetComponent<VRInteractiveItem>().OnClick += delegate { TaskOnClick(acc_F1, 0f); Brake(false); };
        m_F2.GetComponent<VRInteractiveItem>().OnClick += delegate { TaskOnClick(acc_F2, 0f); Brake(false); };
        m_F3.GetComponent<VRInteractiveItem>().OnClick += delegate { TaskOnClick(acc_F3, 0f); Brake(false); };

        m_TL.GetComponent<VRInteractiveItem>().OnClick += delegate { TaskOnClick(acc_F1, -acc_full); Brake(false); };
        m_TR.GetComponent<VRInteractiveItem>().OnClick += delegate { TaskOnClick(acc_F1, acc_full); Brake(false); };

        m_FL.GetComponent<VRInteractiveItem>().OnClick += delegate { TaskOnClick(acc_F2, -acc_slight); Brake(false); };
        m_FR.GetComponent<VRInteractiveItem>().OnClick += delegate { TaskOnClick(acc_F2, acc_slight); Brake(false); };

        m_brake_R.GetComponent<VRInteractiveItem>().OnClick += delegate { TaskOnClick(0f, 0f); BrakeOrReverse(); };

        m_C.GetComponent<VRInteractiveItem>().OnClick += delegate { TaskOnClick(0f, 0f); };


    }
    private void OnDisable() {
        m_L.GetComponent<VRInteractiveItem>().OnClick -= delegate { TaskOnClickL(); Brake(false); };
        m_L.GetComponent<VRInteractiveItem>().OnOut -= delegate { TaskOnOutL(); horizontal_acceleration = 0; };
        m_R.GetComponent<VRInteractiveItem>().OnClick -= delegate { TaskOnClickR(); Brake(false); };
        m_R.GetComponent<VRInteractiveItem>().OnOut -= delegate { TaskOnOutR(); horizontal_acceleration = 0; };

        m_L1.GetComponent<VRInteractiveItem>().OnOver -= delegate { horizontal_acceleration = -acc_X1; Brake(false); };
        m_L2.GetComponent<VRInteractiveItem>().OnOver -= delegate { horizontal_acceleration = -acc_X2; Brake(false); };
        m_L3.GetComponent<VRInteractiveItem>().OnOver -= delegate { horizontal_acceleration = -acc_X3; Brake(false); };
        m_R1.GetComponent<VRInteractiveItem>().OnOver -= delegate { horizontal_acceleration = acc_X1; Brake(false); };
        m_R2.GetComponent<VRInteractiveItem>().OnOver -= delegate { horizontal_acceleration = acc_X2; Brake(false); };
        m_R3.GetComponent<VRInteractiveItem>().OnOver -= delegate { horizontal_acceleration = acc_X3; Brake(false); };

        //m_L1.GetComponent<VRInteractiveItem>().OnOut -= delegate { horizontal_acceleration = 0; Brake(false); };
        //m_L2.GetComponent<VRInteractiveItem>().OnOut -= delegate { horizontal_acceleration = 0; Brake(false); };
        //m_L3.GetComponent<VRInteractiveItem>().OnOut -= delegate { horizontal_acceleration = 0; Brake(false); };
        //m_R1.GetComponent<VRInteractiveItem>().OnOut -= delegate { horizontal_acceleration = 0; Brake(false); };
        //m_R2.GetComponent<VRInteractiveItem>().OnOut -= delegate { horizontal_acceleration = 0; Brake(false); };
        //m_R3.GetComponent<VRInteractiveItem>().OnOut -= delegate { horizontal_acceleration = 0; Brake(false); };

        m_F1.GetComponent<VRInteractiveItem>().OnClick -= delegate { TaskOnClick(acc_F1, 0f); Brake(false); };
        m_F2.GetComponent<VRInteractiveItem>().OnClick -= delegate { TaskOnClick(acc_F2, 0f); Brake(false); };
        m_F3.GetComponent<VRInteractiveItem>().OnClick -= delegate { TaskOnClick(acc_F3, 0f); Brake(false); };

        m_TL.GetComponent<VRInteractiveItem>().OnClick -= delegate { TaskOnClick(acc_F1, -acc_X3); Brake(false); };
        m_TR.GetComponent<VRInteractiveItem>().OnClick -= delegate { TaskOnClick(acc_F1, acc_X3); Brake(false); };

        m_FL.GetComponent<VRInteractiveItem>().OnClick -= delegate { TaskOnClick(acc_F2, -acc_X1); Brake(false); };
        m_FR.GetComponent<VRInteractiveItem>().OnClick -= delegate { TaskOnClick(acc_F2, acc_X1); Brake(false); };

        m_brake_R.GetComponent<VRInteractiveItem>().OnClick -= delegate { TaskOnClick(0f, 0f); BrakeOrReverse(); };

        m_C.GetComponent<VRInteractiveItem>().OnClick -= delegate { TaskOnClick(0f, 0f); };
    }

    // Start is called before the first frame update
    void Start()
    {
        fint = FindObjectOfType<FoveInterface>();

        //m_L.onClick.AddListener(delegate { TaskOnClick2(1, m_L.GetComponent<RectTransform>().rect, m_L.GetComponent<RectTransform>().anchoredPosition); Brake(false); });
        //m_R.onClick.AddListener(delegate { TaskOnClick2(2, m_R.GetComponent<RectTransform>().rect, m_R.GetComponent<RectTransform>().anchoredPosition); Brake(false); });
        //m_F.onClick.AddListener(delegate { TaskOnClick2(3, m_F.GetComponent<RectTransform>().rect, m_F.GetComponent<RectTransform>().anchoredPosition); Brake(false); });

        //m_TL.onClick.AddListener(delegate { TaskOnClick(Y_acc_T, -X_acc_T); Brake(false); });
        //m_TR.onClick.AddListener(delegate { TaskOnClick(Y_acc_T, X_acc_T); Brake(false); });

        //m_FL.onClick.AddListener(delegate { TaskOnClick(Y_acc_F,-X_acc_F); Brake(false); });
        //m_FR.onClick.AddListener(delegate { TaskOnClick(Y_acc_F, X_acc_F); Brake(false); });

        //m_brake_R.onClick.AddListener(delegate { TaskOnClick(0f, 0f); BrakeOrReverse(); });

        //m_C.onClick.AddListener(delegate { TaskOnClick(0f, 0f); });

        //m_L.GetComponent<BoxCollider>().size.Set(m_L.GetComponent<RectTransform>().rect.width, m_L.GetComponent<RectTransform>().rect.height, 1);
        //m_R.GetComponent<BoxCollider>().size.Set(m_L.GetComponent<RectTransform>().rect.width, m_L.GetComponent<RectTransform>().rect.height, 1);
        //m_F.GetComponent<BoxCollider>().size.Set(m_L.GetComponent<RectTransform>().rect.width, m_L.GetComponent<RectTransform>().rect.height, 1);
        //m_TL.GetComponent<BoxCollider>().size.Set(m_L.GetComponent<RectTransform>().rect.width, m_L.GetComponent<RectTransform>().rect.height, 1);
        //m_TR.GetComponent<BoxCollider>().size.Set(m_L.GetComponent<RectTransform>().rect.width, m_L.GetComponent<RectTransform>().rect.height, 1);
        //m_FL.GetComponent<BoxCollider>().size.Set(m_L.GetComponent<RectTransform>().rect.width, m_L.GetComponent<RectTransform>().rect.height, 1);
        //m_FR.GetComponent<BoxCollider>().size.Set(m_L.GetComponent<RectTransform>().rect.width, m_L.GetComponent<RectTransform>().rect.height, 1);
        //m_brake_R.GetComponent<BoxCollider>().size.Set(m_L.GetComponent<RectTransform>().rect.width, m_L.GetComponent<RectTransform>().rect.height, 1);
        //m_C.GetComponent<BoxCollider>().size.Set(m_L.GetComponent<RectTransform>().rect.width, m_L.GetComponent<RectTransform>().rect.height, 1);
    }
    void TaskOnClickL() {
        m_L1.GetComponent<BoxCollider>().enabled = true;
        m_L2.GetComponent<BoxCollider>().enabled = true;
        m_L3.GetComponent<BoxCollider>().enabled = true;

        m_L.GetComponent<BoxCollider>().enabled = false;
    }
    void TaskOnOutL() {
        m_L1.GetComponent<BoxCollider>().enabled = false;
        m_L2.GetComponent<BoxCollider>().enabled = false;
        m_L3.GetComponent<BoxCollider>().enabled = false;

        m_L.GetComponent<BoxCollider>().enabled = true;
    }
    void TaskOnClickR() {
        m_R1.GetComponent<BoxCollider>().enabled = true;
        m_R2.GetComponent<BoxCollider>().enabled = true;
        m_R3.GetComponent<BoxCollider>().enabled = true;

        m_R.GetComponent<BoxCollider>().enabled = false;
    }

    void TaskOnOutR() {
        m_R1.GetComponent<BoxCollider>().enabled = false;
        m_R2.GetComponent<BoxCollider>().enabled = false;
        m_R3.GetComponent<BoxCollider>().enabled = false;

        m_R.GetComponent<BoxCollider>().enabled = true;
    }
    void TaskOnClick(float _vert_acc, float _hori_acc)
    {
        vertical_acceleration = _vert_acc;
        horizontal_acceleration = _hori_acc;
    }
    void TaskOnClick2(int i, Rect rec, Vector2 pos)
    {
        Vector3 v_mouse = Input.mousePosition;
        
        //Debug.Log("Pos: " + pos);
        //Debug.Log("Rec: " + rec);
        //Debug.Log("Mouse: "+v_mouse);

        float _hori_acc;
        float _vert_acc;

        float y_limit1 = pos.y - rec.height / 2;
        float y_limit2 = pos.y + rec.height / 2;

        float x_limit1 = pos.x - rec.width/ 2;
        float x_limit2 = pos.x + rec.width / 2;

        float y_diff = rec.height;
        float x_diff = rec.width;

        float y_speed_diff=0f;
        float x_speed_diff=0f;
        if (i < 3)
        {
            y_speed_diff = acc_F3 - (acc_F1+0.1f);
            x_speed_diff = 1f-0.05f;

            _vert_acc = (y_speed_diff) / (y_diff) * (v_mouse.y-y_limit1) + acc_F1;    // y=ax+b, linear function for vert acc
            vertical_acceleration = _vert_acc;

            if (i == 1)
            {
                _hori_acc = x_speed_diff / (x_diff) * (x_diff-v_mouse.x) + 0.05f;        //  y=ax+b, linear function for hori acc
                horizontal_acceleration = -_hori_acc;
            }
            if (i == 2)
            {
                _hori_acc = x_speed_diff / (x_diff) * (v_mouse.x-(Screen.width-x_diff)) + 0.05f;        //  y=ax+b, linear function for hori acc
                horizontal_acceleration = _hori_acc;
            }
        }
        if (i == 3)
        {
            y_speed_diff = 1f - 0.05f;

            _hori_acc = 0f;
            //          Speed def   /   Button size *   (Setting mouseposition to range from 0 to button range)
            _vert_acc = (y_speed_diff) / (y_diff) * (v_mouse.y-(Screen.height-y_diff)) + 0.05f;    // y=ax+b, linear function for vert acc

            vertical_acceleration = _vert_acc;
            horizontal_acceleration = _hori_acc;
        }

        //Debug.Log(vertical_acceleration);
        //Debug.Log(horizontal_acceleration);
    } // fully variable speed control depending on where on the button you press. Works for mouse click only
    public float GetVertInput()
    {
        return vertical_acceleration;
    }
    public float GetHoriInput()
    {
        return horizontal_acceleration;
    }
    public void SetVertInput(float _vert_acc)
    {
        vertical_acceleration=_vert_acc;
    }
    public void SetHoriInput(float _hori_acc)
    {
        horizontal_acceleration= _hori_acc;
    }
    public void Brake(bool _brake)
    {
        brake_flag = _brake;
    }
    public bool isBraking()
    {
        return brake_flag;
    }

    public void BrakeOrReverse()
    {
        if (stdinterface.GetVelocityMagnitude() < 0.1f)
        {
            // Reverse
            Brake(false);
            vertical_acceleration = -0.1f;
        }
        else Brake(true);
    }
}
