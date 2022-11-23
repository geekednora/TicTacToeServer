using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkedServer : MonoBehaviour
{
    private int _hostID;
    /* ~~ USER DATA ~~ */

    private string _lg = "root";

    /* ~~ END OF USER DATA ~~ */
    private readonly int _maxConnections = 1000;
    private string _pw = "toor";
    private int _reliableChannelID;
    private readonly int _socketPort = 5492;
    private int _unreliableChannelID;

    [Obsolete]
    // Start is called before the first frame update
    private void Start()
    {
        NetworkTransport.Init();
        var config = new ConnectionConfig();
        _reliableChannelID = config.AddChannel(QosType.Reliable);
        _unreliableChannelID = config.AddChannel(QosType.Unreliable);
        var topology = new HostTopology(config, _maxConnections);
        _hostID = NetworkTransport.AddHost(topology, _socketPort, null);
    }

    // Update is called once per frame
    [Obsolete]
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
        }

        int recHostID;
        int recConnectionID;
        var recBuffer = new byte[1024];
        var bufferSize = 1024;
        int dataSize;
        byte error = 0;

        var recNetworkEvent = NetworkTransport.Receive(out recHostID, out recConnectionID,
            out _, recBuffer, bufferSize, out dataSize, out error);

        switch (recNetworkEvent)
        {
            case NetworkEventType.Nothing:
                break;
            case NetworkEventType.ConnectEvent:
                Debug.Log("Connection, " + recConnectionID);
                break;
            case NetworkEventType.DataEvent:
                var msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                ProcessRecievedMsg(msg, recConnectionID);
                break;
            case NetworkEventType.DisconnectEvent:
                Debug.Log("Disconnection, " + recConnectionID);
                break;
        }
    }

    [Obsolete]
    public void SendMessageToClient(string msg, int id)
    {
        byte error = 0;
        var buffer = Encoding.Unicode.GetBytes(msg);
        NetworkTransport.Send(_hostID, id, _reliableChannelID, buffer, msg.Length * sizeof(char), out error);
    }

    private void ProcessRecievedMsg(string msg, int id)
    {
        Debug.Log("msg recieved = " + msg + ".  connection id = " + id);
    }

    private void Login()
    {
    }


    private void ReadUserLoginReq(string login, string pw)
    {
        Debug.Log
        (
            "Logging in user... \nUser ID: " +
            login +
            "User Password: " +
            pw
        );
    }
}