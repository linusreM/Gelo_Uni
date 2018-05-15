
using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class Server : MonoBehaviour
{

    // receiving Thread
    Thread receiveThread;

    // udpclient object
    UdpClient client;
    UdpClient serve = new UdpClient();

    public int port; // define > init
    public int sendPort;
    
    static public string lastReceivedUDPPacket = "";
    static public string allReceivedUDPPackets = ""; //Main container
    static public int unhandledPackets = 0;
    static public string lastRecivedFromIP = "";
    static public IPEndPoint lastIPEndpoint;
    

    public Vector3 Pos;
    public bool run = true;



    public void Start()
    { 
        Init(); //Start UDP receive thread
    }
    /*
    void OnGUI() //Show received network packages, for debugging purposes
    {
        Rect rectObj = new Rect(40, 10, 200, 400);
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.UpperLeft;
        GUI.Box(rectObj, "Server listening on all interfaces at port:" + port + " \n"
            + "\nUnhandled Messages: \n" + allReceivedUDPPackets
            + "\nLast Packet: \n" + lastReceivedUDPPacket
            , style);
    }*/

    private void Init()
    {
        Debug.Log("Initializing UDP-Client");

        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
        

    }

    // receive thread
    private void ReceiveData()
    {
        client = new UdpClient(port);
        while (true)
        {

            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, port);
                byte[] data = client.Receive(ref anyIP);
                lastIPEndpoint = anyIP;
                
                
                //encode received data to string
                string text = Encoding.UTF8.GetString(data);
                
                lastRecivedFromIP = anyIP.Address.ToString();
                // latest UDPpacket
                lastReceivedUDPPacket = text;

                // ....
                allReceivedUDPPackets = allReceivedUDPPackets + text + " ";
                unhandledPackets += 1;
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
            if (!run) {

                
                client.Close();
                
                break;

            }
        }
    }

    public void SendData(string message) //Method to send messages from
    {
        serve.Connect(lastIPEndpoint.Address, sendPort);
        byte[] byteMessage = Encoding.ASCII.GetBytes(message); 
        serve.Send(byteMessage, byteMessage.Length);
    }
    
    public string getLatestUDPPacket() //Not really used here
    {
        allReceivedUDPPackets = "";
        return lastReceivedUDPPacket;
    }


    
    void OnApplicationQuit()  //needed to ensure proper thread termination
    {
        receiveThread.Abort();
        if (client != null)
            client.Close();
    }

}
