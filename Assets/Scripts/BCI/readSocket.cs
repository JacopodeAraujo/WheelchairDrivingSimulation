using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System;
using System.IO;
using System.Text;
public class readSocket : MonoBehaviour {
    // Use this for initialization
    TcpListener listener;
    String msg;
    public bool matlabcomm = false;

    [Serializable]
    public class MatlabData {
        public float Left = 0.0f;
        public float Right= 0.0f;
        public float Both = 0.0f;
        public float Feet = 0.0f;
        public float Cmd = 0.0f;
    }

    public MatlabData mdata;
    // Left, Right, Both arms, Feet, Command
    // Command: Prediction - 1 for left, 2 for right, 3 for both arms, 4 for feet
    // If both arms = -1, then it is a 3 class problem


    void Start() {
        mdata = new MatlabData();
        listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 55001);
        listener.Start();
        Debug.Log("is listening");
    }
    // Update is called once per frame
    void Update() {
        if (!listener.Pending()) {
            //Debug.Log("no package...");
            mdata.Left = 0f;
            mdata.Right= 0f;
            mdata.Both= 0f;
            mdata.Feet= 0f;
            mdata.Cmd= 0f;
        }
        else {
            //Debug.Log("socket comes");
            matlabcomm = true;
            TcpClient client = listener.AcceptTcpClient();
            NetworkStream ns = client.GetStream();
            StreamReader reader = new StreamReader(ns);
            msg = reader.ReadToEnd();
            //Debug.Log(msg);
            mdata = JsonUtility.FromJson<MatlabData>(msg);
            Debug.Log("Command: " + mdata.Cmd);
            Debug.Log("Left: " + mdata.Left);
            Debug.Log("Right: " + mdata.Right);
            Debug.Log("Both: " + mdata.Both);
            Debug.Log("Feet: " + mdata.Feet);

        }


    }


}