using UnityEditor;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class TrailTrackingLog : MonoBehaviour
{
    //public static string dirpath = "/Resources/Log/EXP1_GC/";
    public static string dir = "";
    public static string details = "";
    public static string ctrl_interface = "GTG";
    public static string subjectname = "name";
    public static string expTitle = "Exp1";
    public static string expDescription = "Subject performance for different control interfaces. This list contains subject world coordinate points during trial among what interface he is using. \r\nImage of trail path is available.";
    public static string expRowInfo = "Position \t Time";
    public string level = "";
    public bool trickhit = false;

    Logger logger;

    float gameTimer=0;

    Vector3 startpos;
    Vector3 currentpos;
    TrailRenderer trailHittingObject;

    DateTime starttime;
    DateTime currenttime;
    Transform tf;


    bool logFlag = false;

    public Camera OverviewCam;
    public int resWidth = 1024;
    public int resHeight = 768;

    int collisions = 0;
    int spawns = 0;

    int i = 1;

    public Logger Logger
    {
        get => default;
        set
        {
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //dir = Application.dataPath + dirpath;
        string dirpath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        dir = dirpath + "/Log/EXP1_GC/";
        subjectname = DetailsScript.GetName();
        expTitle = "Exp1";
        ctrl_interface = DetailsScript.GetControl();
        level = SceneManager.GetActiveScene().name.ToString();

        //cam.enabled = false;
        tf = GetComponent<Transform>();
        trailHittingObject = GetComponent<TrailRenderer>();
        details = level+"_"+ ctrl_interface;
        logger = new Logger(dir, expTitle, subjectname, details);

        StartLogging();
        gameTimer = Time.time;
    }

    private void OnDestroy() {
        logger.TerminateLogger();
    }
    // Update is called once per frame
    void Update()
    {
        trailHittingObject.emitting = PlayerCollision.IsHittingObstacle();

        if (Time.time > gameTimer + 1) {
            gameTimer = Time.time;
            if (logFlag)
                LogPosition();
        }
    }
    public void SpawnIncrement() {
        spawns++;
        logger.LogLine("SPAWN: " + spawns);
    }
    public void CollisionIncrement(bool trick) {
        collisions++;
        if (trick) {
            trickhit = true;
            logger.LogLine("*** TRICK OBSTACLE HIT ***: " + collisions);
        }
        else logger.LogLine("OBSTACLE HIT: " + collisions); 
    }
    void StartLogging()
    {
        logFlag = true;
        startpos = tf.position;
        starttime = DateTime.Now;
        currentpos = startpos;
        currenttime = starttime;
        
        string s = "Ctrl Int: " + ctrl_interface + "\r\n" + "Level: "+ level+"\r\n"+expDescription;
        logger.LogHeader(expTitle, s, starttime);
        logger.LogColumnNames(expRowInfo);
        LogPosition();

    }
    void LogPosition()
    {
        currentpos = tf.position;
        currenttime = DateTime.Now;

        logger.LogLine("" + currentpos.ToString() + "\t" + (currenttime - starttime).TotalSeconds.ToString() + "\t");

    }
    
    public void EndLogging()
    {
        LogPosition();
        logFlag = false;
        logger.LogLine("");
        logger.LogLine("TrailTime: " + GetTrailTime()+" s");
        logger.LogLine("Obstacle Hits: " + collisions);
        logger.LogLine("Spawns: " + spawns);
        logger.LogLine("Tricked: " + trickhit);
        logger.LogLine("");
        logger.EndLog();

        GetTrailScreenshot();

        //Print the text from the file
        //Debug.Log(filepath);
    }

    double GetTrailTime()
    {
        TimeSpan trailtimespan = (currenttime - starttime);
        return trailtimespan.TotalSeconds;
    }

    void GetTrailScreenshot()
    {
        OverviewCam.enabled = true;
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        OverviewCam.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        OverviewCam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        OverviewCam.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        string _filename = logger.GetImageFileName(resWidth, resHeight);
        System.IO.File.WriteAllBytes(_filename, bytes);
        Debug.Log(string.Format("Took screenshot to: {0}", _filename));
        //cam.enabled = false;

        // Re-import screenshot for it to appear instantly
        //AssetDatabase.ImportAsset(_filename);
        Resources.Load(_filename);
    }

}
