using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Text;
using Newtonsoft.Json.Linq;

public class UDPReceiver : MonoBehaviour
{
    private UdpClient udpClient;
    private int port = 12345; // Port you are sending data from Python script

    public Transform leftHandTransform;
    public Transform rightHandTransform;

    void Start()
    {
        udpClient = new UdpClient(port);
        udpClient.BeginReceive(new AsyncCallback(ReceiveData), null);
    }

    private void ReceiveData(IAsyncResult result)
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
        byte[] receivedBytes = udpClient.EndReceive(result, ref endPoint);
        string message = Encoding.UTF8.GetString(receivedBytes);
        ProcessHandData(message);

        // Continue listening for incoming data
        udpClient.BeginReceive(new AsyncCallback(ReceiveData), null);
    }

    private void ProcessHandData(string message)
    {
        try
        {
            JObject handData = JObject.Parse(message);
            if (handData["left"] != null)
            {
                float leftX = handData["left"]["x"].Value<float>();
                float leftY = handData["left"]["y"].Value<float>();
                leftHandTransform.position = new Vector3(leftX, leftY, leftHandTransform.position.z);
            }

            if (handData["right"] != null)
            {
                float rightX = handData["right"]["x"].Value<float>();
                float rightY = handData["right"]["y"].Value<float>();
                rightHandTransform.position = new Vector3(rightX, rightY, rightHandTransform.position.z);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error processing hand data: " + e.Message);
        }
    }

    void OnApplicationQuit()
    {
        udpClient.Close();
    }
}