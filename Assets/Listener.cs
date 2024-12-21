using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class Listener : MonoBehaviour
{
    private Thread thread;
    private UdpClient udpServer; // Changed from TcpListener to UdpClient
    private bool isRunning = true;
    public int connectionPort = 25001;

    public float[] handData = new float[2];

    void Start()
    {
        thread = new Thread(new ThreadStart(GetData));
        thread.Start();
    }

    void GetData()
    {
        try
        {
            udpServer = new UdpClient(connectionPort); // Changed from TcpListener initialization to UdpClient
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, connectionPort); // Added to specify the remote endpoint

            while (isRunning)
            {
                if (udpServer.Available > 0) // Changed to check data availability for UDP
                {
                    byte[] receivedBytes = udpServer.Receive(ref remoteEndPoint); // Changed to receive data via UDP
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
        handData[0] = float.Parse(values[1]);
        handData[1] = float.Parse(values[2]);
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
            udpServer.Close(); // Changed from TcpClient and TcpListener cleanup to UdpClient cleanup
            udpServer = null;
        }
    }
}