using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;
using System.Linq;

public class Communicationwithpython : MonoBehaviour
{

    private Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    private byte[] _recieveBuffer = new byte[8142];
    [Serializable]
    public class Frompydata
    {
        public float Lhandpy=0.0f;
        public float Rhandpy = 0.0f;
        public float Restpy = 0.0f;
        public float Feetpy = 0.0f;
        public float Nclass = 0.0f;
    }
    public Frompydata pydata;
    public float tresh;
    public float[] activations = new float[] { 0, 0, 0, 0, 0};
    public bool DNN = false;
    public int maxindex=-1;
    private float Maxactivation;

    void Start()
    {
        SetupServer();
        pydata = new Frompydata();
        tresh = 0.1f;
    }

    void Update()
    {
        activations[0] = pydata.Rhandpy;
        activations[1] = pydata.Lhandpy;
        activations[2] = pydata.Feetpy;
        activations[3] = pydata.Restpy;
        activations[4] = pydata.Nclass;

        if (activations.Sum() > 0)
        {
            tresh = 1.2f / pydata.Nclass; //for 2 class: 0.6, 3 class: 0.4, 4 class: 0.3
            Maxactivation = activations.Take(4).Max();
            if (Maxactivation>tresh)
            {
                DNN = true;
                maxindex = activations.ToList().IndexOf(Maxactivation);
            }
            
            Debug.Log("Class: " + activations[4]);    
        }
        else
        {
            DNN = false;
            maxindex = -1;
        }
    }

    void OnApplicationQuit()
    {
        _clientSocket.Close();
    }

    private void SetupServer()
    {
        try
        {
            _clientSocket.Connect(new IPEndPoint(IPAddress.Loopback, 50003));
        }
        catch (SocketException ex)
        {
            Debug.Log(ex.Message);
        }

        _clientSocket.BeginReceive(_recieveBuffer, 0, _recieveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);

    }


    public void ReceiveCallback(IAsyncResult AR)
    {
        //Check how much bytes are recieved and call EndRecieve to finalize handshake
        int recieved = _clientSocket.EndReceive(AR);

        if (recieved <= 0)
            return;

        //Copy the recieved data into new buffer , to avoid null bytes
        byte[] recData = new byte[recieved];
        Buffer.BlockCopy(_recieveBuffer, 0, recData, 0, recieved);

        //Process data here the way you want , all your bytes will be stored in recData
        //Debug.Log(System.Text.Encoding.Default.GetString(_recieveBuffer));
        JsonUtility.FromJsonOverwrite(System.Text.Encoding.Default.GetString(recData), pydata);
        //Debug.Log(pydata);
        //SendData(System.Text.Encoding.Default.GetBytes("ping"));
        //Start receiving again
        _clientSocket.BeginReceive(_recieveBuffer, 0, _recieveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
    }

    public void SendEndRecording() {
        SendData(System.Text.Encoding.Default.GetBytes((-1).ToString()));
    }
    public void SendStartRecording() {
        SendData(System.Text.Encoding.Default.GetBytes((1).ToString()));
    }

    public void SendData(byte[] data)
    {
        SocketAsyncEventArgs socketAsyncData = new SocketAsyncEventArgs();
        socketAsyncData.SetBuffer(data, 0, data.Length);
        _clientSocket.SendAsync(socketAsyncData);
    }
}