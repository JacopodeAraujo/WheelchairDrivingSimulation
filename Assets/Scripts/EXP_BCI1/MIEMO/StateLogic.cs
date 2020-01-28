using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EXP_StateMachine;
using System.IO;
using System;
using System.Linq;
using System.Collections;
using Assets.LSL4Unity.Scripts;
using WindowsInput;
using WindowsInput.Native;
using UnityEngine.SceneManagement;

public class StateLogic : MonoBehaviour
{
    IInputSimulator inputSimulator;

    // lsl communication
    private LSLMarkerStream marker;

    // MIEMO trials and levels
    public int c_trial = 0;
    public int t_trials = 2;
    public int c_roadsplit_level = 1;
    public int t_roadsplit_levels = 2;
    // TRAINMI trials and level
    public int c_Ttrial = 0;
    int t_Ttrials = 2;
    int c_training_level = 1;
    int t_training_levels = 2;

    public int[] t_incorrectTrials;


    List<bool> trialEMO_list;
    List<int> trialMI_list;
    List<int> trialMI_list_complementary;
    List<TimeSpan> mi_times;
    List<TimeSpan> emotional_times;
    List<bool> rightorleft_randomlist;
    List<TimeSpan> mi_training_times;

    //public static string dirpath = "/Resources/Log/EXP2_BCI_MIEMO/";
    public static string dir = "";
    public static string filename = "Exp2_data";
    public static string subjectname = "name";
    public static string expTitle = "EXP 2 \t BCI MI and EMOTION Acquisition";
    public static string expDescription = "This list contains the trial bool and times for MI and emotional BCI acquisition";
    static string expRowInfo_miemo = "Level/Trial \t MI class \t MI time \t Emo class \t Emo time";
    static string expRowInfo_training = "Level/Trial \t Movement \t Time";
    Logger logger;

    DateTime starttime;

    float gameTimer;
    public int seconds = 0;
    public int stateTransitionTime = 5;
    public GameObject state2animation;
    public GameObject state2animation2;
    public GameObject state3animation;
    public GameObject state5CorrectAnimation;
    public GameObject state5IncorrectAnimation;
    public GameObject stateT1ani;
    public GameObject stateT2ani;
    public GameObject stateT3ani;
    public GameObject stateT4ani;

    public GameObject wheelchair;

    public Vector3 startPosition = new Vector3(-6, 1.07f, 0);
    public Vector3 startOrientation = new Vector3(0, 90, 0);

    // Flags
    public bool turnRight = true;
    public bool answerRight = true;
    bool flag = true;
    public bool switchState = false;
    public bool IsTraining = false;
    bool IsExperimenting = true;

    public StateMachine<StateLogic> stateMachine { get; set; }

    public Camera mycam;
    private void Start() 
    {
        mycam.targetDisplay = 1;
        if (Display.displays.Length > 1)
            Display.displays[1].Activate();

        inputSimulator = new InputSimulator();
        // lsl marker
        marker = GetComponent<LSLMarkerStream>();

        //dir = Application.dataPath + dirpath;
        string dirpath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        dir = dirpath + "/Log/EXP2_BCI_MIEMO/";
        subjectname = DetailsScript.GetName();
        expTitle = "Exp2";

        // Training levels are in between roadsplit levels, so num of training is one less than roadsplit
        t_roadsplit_levels = DetailsScript.GetMIEMOlevels();
        t_trials = DetailsScript.GetMIEMOtrials();
        t_training_levels = t_roadsplit_levels - 1;
        t_Ttrials = t_trials;
        t_incorrectTrials = DetailsScript.GetMIEMOincorrects();

        // List instantiation
        trialEMO_list = new List<bool>(t_trials);
        trialMI_list = new List<int>(t_trials);
        trialMI_list_complementary = new List<int>(t_trials);
        mi_times = new List<TimeSpan>(t_trials);
        emotional_times = new List<TimeSpan>(t_trials);
        rightorleft_randomlist = new List<bool>(t_trials);
        mi_training_times = new List<TimeSpan>(t_trials);


        // Filling Bool-list with the particular levels correct-rate, filling MI-list with the 4 classes, Shuffle/randomize lists
        PrepareTrialLists(c_roadsplit_level, t_trials);

        // Timer init and Time registration
        stateMachine = new StateMachine<StateLogic>(this);
        stateMachine.ChangeState(State1.Instance);
        gameTimer = Time.time;
        starttime = DateTime.Now;
        // Set log details and make sure row info corresponds to the order of the list args in WriteListsToLog()
        logger = new Logger(dir, expTitle, subjectname);
        logger.LogHeader(expTitle, expDescription, starttime);

    }
    public void LSLMarkerStart(string s, int i) {
        marker.Write(s);
        Debug.Log(s);
        KeyboardSimulator k = new KeyboardSimulator(inputSimulator);
        // 1 = right hand, 2 = left hand, 3 = both hands, 4 = both legs -- motor imagery
        // 5 = EMO satisfied, 6 = EMO unsatisfied,
        // 7 = right hand, 8 = left hand, 9 = both hands, 10 = both legs -- motor execution
        // Event / marker starter. To use with openVibe Keyboard stimulator, if LSL markers doesn't work
        if (i == 1)
            k.KeyDown(VirtualKeyCode.VK_A);
        if (i == 2)
            k.KeyDown(VirtualKeyCode.VK_E);
        if (i == 3)
            k.KeyDown(VirtualKeyCode.VK_I);
        if (i == 4)
            k.KeyDown(VirtualKeyCode.VK_O);
        if (i == 5)
            k.KeyDown(VirtualKeyCode.VK_P);
        if (i == 6)
            k.KeyDown(VirtualKeyCode.VK_R);
        if (i == 7)
            k.KeyDown(VirtualKeyCode.VK_T);
        if (i == 8)
            k.KeyDown(VirtualKeyCode.VK_U);
        if (i == 9)
            k.KeyDown(VirtualKeyCode.VK_Y);
        if (i == 10)
            k.KeyDown(VirtualKeyCode.VK_Z);

    }
    public void LSLMarkerStop(string s, int i) {
        marker.Write(s);
        Debug.Log(s);
        KeyboardSimulator k = new KeyboardSimulator(inputSimulator);

        if (i == 1)
            k.KeyUp(VirtualKeyCode.VK_A);
        if (i == 2)
            k.KeyUp(VirtualKeyCode.VK_E);
        if (i == 3)
            k.KeyUp(VirtualKeyCode.VK_I);
        if (i == 4)
            k.KeyUp(VirtualKeyCode.VK_O);
        if (i == 5)
            k.KeyUp(VirtualKeyCode.VK_P);
        if (i == 6)
            k.KeyUp(VirtualKeyCode.VK_R);
        if (i == 7)
            k.KeyUp(VirtualKeyCode.VK_T);
        if (i == 8)
            k.KeyUp(VirtualKeyCode.VK_U);
        if (i == 9)
            k.KeyUp(VirtualKeyCode.VK_Y);
        if (i == 10)
            k.KeyUp(VirtualKeyCode.VK_Z);
    }
    private void OnDestroy() {
        if (logger != null)
            logger.TerminateLogger();
    }
    public float GetGameTime() {
        return gameTimer;
    }
    public bool CheckIfNextTrial() {
        if (!IsTraining) {
            if (c_trial >= t_trials)
                return false;
            else
                return true;
        }
        else {
            if (c_Ttrial >= t_Ttrials)
                return false;
            else
                return true;
        }
    }
    public void PrintLists() {
        Debug.Log("Trial \t Right \t Satisfied \t MI \t MI-compl.");
        for (int i = 0; i <= trialEMO_list.Count - 1; i++)
            Debug.Log("T" + (i + 1) + ": \t" + rightorleft_randomlist[i] + "\t" + trialEMO_list[i] + "\t" + trialMI_list[i] + "\t" + trialMI_list_complementary[i]);
    }
    public void PrepareNextLevel() {
        // Preparing for TRAINMI-level
        if (!IsTraining) {
            c_roadsplit_level++;
            c_Ttrial = 0;
            //IsTraining = true;
            // ------------------------------------- End of levels in experiment
            if (c_roadsplit_level > t_roadsplit_levels) {
                //c_roadsplit_level = 0;
                IsTraining = false;
                IsExperimenting = false;
            }
        }
        // Preparing for MIEMO-level
        else {
            c_training_level++;
            c_trial = 0;
            //IsTraining = false;
            
        }
    }
    private void Update() 
    {
        /*----------- BCI DATA ACQUISITION ADMINISTRATIVE LOGIC -----------*/

        // Go to menu when pressing escape
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("StartMenu");
        }

        // If the experiments is rolling
        if (IsExperimenting) {

            if (Time.time > gameTimer + 1) {
                gameTimer = Time.time;
                seconds++;
                //Debug.Log(seconds);
            }

            if (seconds == stateTransitionTime) {
                seconds = 0;
                switchState = true;
            }
            else switchState = false;

            // Whether the WC is turning the way that the subject is trying to go, is determined by the trialBool_list
            if (c_trial != 0) {
                answerRight = trialEMO_list[c_trial - 1];
                turnRight = rightorleft_randomlist[c_trial - 1];

                State2.Instance.TurningRight = turnRight;
                State5.Instance.AnswerRight = answerRight;
                State5.Instance.TurningRight = turnRight;
            }
            else Debug.Log("HOVSA");



            stateMachine.Update();
        }

        // If experiment is over
        else {
            if (flag) {       //  Re-using state1flag to only print once
                Debug.Log("END OF EXPERIMENT");
                stateT1ani.GetComponentInChildren<Text>().text = "End of Experiment";
                flag = false;
            }
        }
    }

    private void FillTrialBoolList(int level) {
        
        trialEMO_list.Clear();
        trialEMO_list.TrimExcess();
        for (int i=1; i<=t_incorrectTrials[level-1]; i++) 
        {
            trialEMO_list.Add(false);
        }
        for (int i = t_incorrectTrials[level-1]; i <= t_trials-1; i++) {
            trialEMO_list.Add(true);
        }
    }
    private void FillTrialMIList(int listsize) {

        trialMI_list.Clear();
        trialMI_list.TrimExcess();
        int j = 0;
        for (int i=1; i<=listsize; i++) {
            j++;
            trialMI_list.Add(j);
            if (j == 4)
                j = 0;
        }
    }
    // The complementary MI list ensures that when the target movement is right hand, the other option is left and, and target movement is feet, the complementary is both hands
    private void FillComplementaryMIList(int listsize) {
        trialMI_list_complementary.Clear();
        trialMI_list_complementary.TrimExcess();
        for (int i = 0; i<listsize; i++) {
            switch (trialMI_list[i]) {
                case 1: trialMI_list_complementary.Add(2); break;
                case 2: trialMI_list_complementary.Add(1); break;
                case 3: trialMI_list_complementary.Add(4); break;
                case 4: trialMI_list_complementary.Add(3); break;
            }
        }
    }
    private void FillBoolListRandomly(List<bool> list, int size) {
        list.Clear();
        list.TrimExcess();
        for (int i = 1; i <= size; i++) {
            if (i % 2 == 0)
                list.Add(false);
            else list.Add(true);
        }
    }
    public static void Shuffle<T>(List<T> ts) {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i) {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    private void LogLevel() 
    {
        String text;
        List<IList> objectList;
        if (IsTraining) {
            logger.LogColumnNames(expRowInfo_training);
            text = "TRAINMI L" + (c_training_level-1) + " T";
            // Make sure the order here is aligned with the rowinfo of the log header
            objectList = new List<IList> { (IList)trialMI_list, (IList)mi_training_times};
            logger.WriteListsToLog(objectList, t_Ttrials, text);
        }
        else {
            logger.LogColumnNames(expRowInfo_miemo);
            text = "MIEMO L" + (c_roadsplit_level-1) + " T";
            // Make sure the order here is aligned with the rowinfo of the log header
            objectList = new List<IList> { (IList)trialMI_list, (IList)mi_times, (IList)trialEMO_list, (IList)emotional_times};
            logger.WriteListsToLog(objectList, t_trials, text);
        }
    }

    public void AddCurrentTimeToMIList() {
        mi_times.Add(DateTime.Now - starttime);
        mi_times.TrimExcess();
    }
    public void AddCurrentTimeToMItrainingList() {
        mi_training_times.Add(DateTime.Now - starttime);
        mi_training_times.TrimExcess();
    }
    public void AddCurrentTimeToEmoList() {
        emotional_times.Add(DateTime.Now - starttime);
        emotional_times.TrimExcess();
    }

    private void ResetTimeListsPerLevel() {
        mi_times.Clear();
        mi_times.TrimExcess();
        emotional_times.Clear();
        emotional_times.TrimExcess();
    }
    public void WrapUpLevel() {
        PrepareNextLevel();

        if (IsTraining) {
            LogLevel();
            logger.LogLine("END OF TRAINING "+ (c_training_level-1)+"\r\n");
            logger.LogLine("START OF LEVEL " + c_roadsplit_level);
            IsTraining = false;

            // Fill lists for emotional and MI acquisition and shuffle them again
            PrepareTrialLists(c_roadsplit_level, t_trials);
        }
        else {
            LogLevel();
            logger.LogLine("END OF LEVEL " + (c_roadsplit_level-1)+ "\r\n");

            if (c_roadsplit_level <= t_roadsplit_levels) {
                logger.LogLine("START OF TRAINING " + c_training_level);
                IsTraining = true;
            }
            else {
                logger.EndLog();
            }
        }
    }


    private void PrepareTrialLists(int level, int num_trials) {
        ResetTimeListsPerLevel();
        // Order is important in this list preparation. The complementary list shall be made after the shuffle of the MI list
        FillTrialBoolList(level);
        FillTrialMIList(num_trials);
        Shuffle<bool>(trialEMO_list);
        Shuffle<int>(trialMI_list);

        FillComplementaryMIList(num_trials);
        FillBoolListRandomly(rightorleft_randomlist, t_trials);
        Shuffle<bool>(rightorleft_randomlist);
        PrintLists();
    }

    public int GetMIMovement(bool training) {
        if (training)
            return trialMI_list[c_Ttrial - 1];
        else return trialMI_list[c_trial - 1];
    }
    public int GetComplementaryMIMovement() {
        return trialMI_list_complementary[c_trial-1];
    }
}
