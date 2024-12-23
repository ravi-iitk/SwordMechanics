using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class Listener : MonoBehaviour
{
    private Thread thread;
    private UdpClient udpServer;
    private bool isRunning = true;
    public int connectionPort = 25001;
    //new section
    public float[] LeftHandData = new float[3];//// Created 2 arrays for left and right hand
    public float[] RightHandData = new float[3];


    void Start()
    {
        thread = new Thread(new ThreadStart(GetData));
        thread.Start();
    }
    //chatgpt
    void Update()
    {
        UnityEngine.Debug.Log("Listener LeftHandData: " + string.Join(", ", LeftHandData));
        UnityEngine.Debug.Log("Listener RightHandData: " + string.Join(", ", RightHandData));
    }

    void GetData()
    {
        try
        {
            udpServer = new UdpClient(connectionPort);
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, connectionPort);

            while (isRunning)
            {
                if (udpServer.Available > 0)
                {
                    byte[] receivedBytes = udpServer.Receive(ref remoteEndPoint);
                    string dataReceived = Encoding.UTF8.GetString(receivedBytes);
                    UnityEngine.Debug.Log("Received data: " + dataReceived);
                    ParseData(dataReceived);
                }

                Thread.Sleep(10);
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log("SocketException: " + e);
        }
        finally
        {
            Cleanup();
        }
    }
    void ParseData(string data)
    {
        string[] values = data.Split(' ');
        //new section
        if (values[0] == "Left")
        {
            LeftHandData[0] = float.Parse(values[1]);
            LeftHandData[1] = float.Parse(values[2]);
            LeftHandData[2] = float.Parse(values[3]);

            if (values.Length > 5)
            {  // Ensure Right hand data exists
                RightHandData[0] = float.Parse(values[5]);
                RightHandData[1] = float.Parse(values[6]);
                RightHandData[2] = float.Parse(values[7]);
            }
        }
        else if (values[0] == "Right")
        {
            RightHandData[0] = float.Parse(values[1]);
            RightHandData[1] = float.Parse(values[2]);
            RightHandData[2] = float.Parse(values[3]);

            if (values.Length > 5)
            {  // Ensure Left hand data exists
                LeftHandData[0] = float.Parse(values[5]);
                LeftHandData[1] = float.Parse(values[6]);
                LeftHandData[2] = float.Parse(values[7]);
            }
        }
    }

    void OnApplicationQuit()
    {
        isRunning = false;
        thread.Join();
        Cleanup();
    }

    void Cleanup()
    {
        if (udpServer != null)
        {
            udpServer.Close();
            udpServer = null;
        }
    }
}