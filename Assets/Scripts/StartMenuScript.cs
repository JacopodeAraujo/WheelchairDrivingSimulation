using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

public class StartMenuScript : MonoBehaviour
{
    public CanvasRenderer adminCanvas;
    public CanvasRenderer startCanvas;
    public GameObject exp1;
    public GameObject exp2;
    public GameObject exp3;
    public Dropdown dropdown_ci;
    public Dropdown dropdown_exp;
    public Button b_save;

    public List<string> controls = new List<string>() { "GazeGUI", "Go2Gaze", "GazeVector", "BCI-MI", "Keyboard" };
    public List<string> experiment = new List<string>() { "Exp1 GazeControls", "Exp2 MIEMO", "Exp3 BCI Control"};

    bool flagLevel = false;
    bool flagTrials = false;

    private int exp = 1;

    private void Start() {
        startCanvas.gameObject.SetActive(true);
        adminCanvas.gameObject.SetActive(false);
        exp1.gameObject.SetActive(false);
        exp2.gameObject.SetActive(false);
        exp3.gameObject.SetActive(false);
        GetInput_dd_ctrlInt(0);
        GetInput_dd_exp(0);
        GetInput_dd_Lcomb1(0);
        GetInput_dd_Lcomb2(0);
        GetInput_dd_Lcomb3(0);

    }

    public void populateLists() {
        dropdown_ci.ClearOptions();
        dropdown_exp.ClearOptions();

        dropdown_ci.AddOptions(controls);
        dropdown_exp.AddOptions(experiment);
    }
    public void OnClickTraining()
    {
        Debug.Log("Start Training");
        if (exp==1)
            SceneManager.LoadScene("Level1WC");
        else if (exp == 3) {
            try {
                SceneManager.LoadScene("Level"+DetailsScript.GetExp3Levels()[0]+"WC");
            } catch {
                Debug.Log("Couldn't start specified level");
            }

            
        }
    }
    public void OnClickMIEMO() {
        Debug.Log("Start MIEMO");
        SceneManager.LoadScene("EXP2_MIEMO");
    }
    public void OnClickRLS()
    {
        Debug.Log("Start RLS");
        SceneManager.LoadScene("RLS");
    }

    public void OnClickBoxGame()
    {
        Debug.Log("Start Box Game");
        SceneManager.LoadScene("Level1");
    }

    public void OnClickSaveButton() {
        startCanvas.gameObject.SetActive(true);
        adminCanvas.gameObject.SetActive(false);
    }

    public void OnClickAdmin() {
        adminCanvas.gameObject.SetActive(true);
        startCanvas.gameObject.SetActive(false);
    }

    public void GetInput_SubjectName(string name) {
        Debug.Log("Name: " + name);
        DetailsScript.SetName(name);
    }

    public void GetInput_dd_ctrlInt(int i) {
        Debug.Log("Ctrl Int: " + controls[i]);
        DetailsScript.SetControl(controls[i]);
    }

    public void GetInput_dd_exp(int i) {
        Debug.Log("Experiment " + experiment[i]);
        if (i == 0) {
            exp = 1;
            exp1.gameObject.SetActive(true);
            exp2.gameObject.SetActive(false);
            exp3.gameObject.SetActive(false);
            b_save.enabled = true;
        }
        else if (i == 1) {
            exp = 2;
            exp1.gameObject.SetActive(false);
            exp2.gameObject.SetActive(true);
            exp3.gameObject.SetActive(false);
            b_save.enabled = false;
        }
        else if (i == 2) {
            exp = 3;
            exp1.gameObject.SetActive(false);
            exp2.gameObject.SetActive(false);
            exp3.gameObject.SetActive(true);
            b_save.enabled = true;
        }
        else {
            exp = 1;
            exp1.gameObject.SetActive(false);
            exp2.gameObject.SetActive(false);
            exp3.gameObject.SetActive(false);
        }
        
        //DetailsScript.SetExp("Exp"+(i+1));
    }

    public void GetInput_dd_Lcomb1(int i) {
        switch (i) {
            case 0: DetailsScript.SetLcomb1("A"); break;
            case 1: DetailsScript.SetLcomb1("B"); break;
            case 2: DetailsScript.SetLcomb1("C"); break;
            case 3: DetailsScript.SetLcomb1("D"); break;
            case 4: DetailsScript.SetLcomb1("E"); break;
            default: DetailsScript.SetLcomb1("A"); break;
        }
    }
    public void GetInput_dd_Lcomb2(int i) {
        switch (i) {
            case 0: DetailsScript.SetLcomb2("A"); break;
            case 1: DetailsScript.SetLcomb2("B"); break;
            case 2: DetailsScript.SetLcomb2("C"); break;
            case 3: DetailsScript.SetLcomb2("D"); break;
            case 4: DetailsScript.SetLcomb2("E"); break;
            default: DetailsScript.SetLcomb2("A"); break;
        }
    }
    public void GetInput_dd_Lcomb3(int i) {
        switch (i) {
            case 0: DetailsScript.SetLcomb3("A"); break;
            case 1: DetailsScript.SetLcomb3("B"); break;
            case 2: DetailsScript.SetLcomb3("C"); break;
            case 3: DetailsScript.SetLcomb3("D"); break;
            case 4: DetailsScript.SetLcomb3("E"); break;
            default: DetailsScript.SetLcomb3("A"); break;
        }
    }

    public void GetInput_Levels(string _levels) {
        int l;
        if (int.TryParse(_levels, out l)) {
            Debug.Log("Levels: " + _levels);
            DetailsScript.SetMIEMOlevels(l);
            flagLevel = true;
        }
        else Debug.Log("Insert numbers");
    }

    public void GetInput_Trials(string _trials) {
        int t;
        if (int.TryParse(_trials, out t)) {
            Debug.Log("Trials: " + _trials);
            DetailsScript.SetMIEMOtrials(t);
            flagTrials = true;
        }
        else Debug.Log("Insert numbers");
    }

    public void GetInput_IncorrectTrials(string _ICtrials) {
        bool flag = true;
        string[] s;
        _ICtrials = _ICtrials.Replace(" ", string.Empty);
        List<string> tt = _ICtrials.Split(',')  .ToList<string>();
        tt.RemoveAll(p => string.IsNullOrEmpty(p));
        tt.TrimExcess();
        s = tt.ToArray();

        int[] t = new int[s.Length];

        for (int i=0; i<s.Length; i++) {
            if (int.TryParse(s[i], out t[i])) {
                //Debug.Log("to: "+t[i].ToString());
                if (t[i] > DetailsScript.GetMIEMOtrials()) {
                    Debug.Log("Incorrect trials doesn't match Trials.");
                    Debug.Log("Trials = " + DetailsScript.GetMIEMOtrials() + " ICTrials = " + t[i]);
                    flag = false;
                }
            }
        }


        if (t.Length != DetailsScript.GetMIEMOlevels()) {
            Debug.Log("Amount of incorrect trials doesn't match Levels.");
            Debug.Log("Levels = " + DetailsScript.GetMIEMOlevels() + " ICLevels = " + t.Length);
            flag = false;
        }
        if (flag) {
            Debug.Log("Correct");
            DetailsScript.SetMIEMOincorrects(t);
            b_save.enabled = true;
        }
        else {
            Debug.Log("Re-enter valid incorrect trials");
            b_save.enabled = false;
        }
        

        //if (int.TryParse(_trials, out t)) {
        //    Debug.Log("Trials: " + _trials);
        //    DetailsScript.SetMIEMOtrials(t);
        //}
        //else Debug.Log("Insert numbers comma separated. Ex: '10,7,5,3,1'");
    }

    public void GetInput_Levels_Exp3(string levels) {
        
        string[] s;
        if (levels == null)
            Debug.Log("All Levels...");
        else {
            levels = levels.Replace(" ", string.Empty);
            List<string> tt = levels.Split(',').ToList<string>();
            tt.RemoveAll(p => string.IsNullOrEmpty(p));
            tt.TrimExcess();
            s = tt.ToArray();

            int[] t = new int[s.Length];

            for (int i = 0; i < s.Length; i++) {
                if (int.TryParse(s[i], out t[i])) {
                    Debug.Log("Level: " + t[i]);
                }
            }

            if (t.Length > 0)
                DetailsScript.SetExp3Levels(t);
        }
    }

}
