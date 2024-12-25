using System;
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

    public float[] LeftHandData = new float[3];
    public float[] RightHandData = new float[3];

    void Start()
    {
        thread = new Thread(new ThreadStart(GetData));
        thread.Start();
    }

    void GetData()
    {
        try
        {
            udpServer = new UdpClient(connectionPort); 
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, connectionPort); 

            while (isRunning)
            {
                if (udpServer.Available > 0)  // 
                {
                    byte[] receivedBytes = udpServer.Receive(ref remoteEndPoint); 
                    string dataReceived = Encoding.UTF8.GetString(receivedBytes);
                    Debug.Log("Received data: " + dataReceived);
                    ParseData(dataReceived);
                }

                Thread.Sleep(10);
            }
        }
        catch (Exception e)
        {
            Debug.Log("SocketException: " + e);
        }
        finally
        {
            Cleanup();
        }
    }
    void ParseData(string data){
        string[] values = data.Split(' ');
        if (values[0] == "Left")
        {
            LeftHandData[0] = float.Parse(values[1]);
            LeftHandData[1] = float.Parse(values[2]);
            LeftHandData[2] = float.Parse(values[3]);
            RightHandData[0] = float.Parse(values[5]);
            RightHandData[1] = float.Parse(values[6]);
            RightHandData[2] = float.Parse(values[7]);


        }
        else
        {
            RightHandData[0] = float.Parse(values[1]);
            RightHandData[1] = float.Parse(values[2]);
            RightHandData[2] = float.Parse(values[3]);
            LeftHandData[0] = float.Parse(values[5]);
            LeftHandData[1] = float.Parse(values[6]);
            LeftHandData[2] = float.Parse(values[7]);
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