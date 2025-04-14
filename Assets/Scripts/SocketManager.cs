using System;
using System.Collections;
using System.Collections.Generic;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class Player{
    public string userID;
    public string PlaName;
    public string roomID;
    public Vector3 pos;
}
[Serializable]
public class Room{
    public string roomID;
    public List<Player> players;
}

public class SocketManager : Singleton<SocketManager>
{
    public SocketIOUnity socket;
    public Player player;
    public Room room=new();
    //string uri="http://185.242.161.111:1234";
    string uri="http://localhost:1234";
    void OnEnable()
    {
           socket = new SocketIOUnity(uri, new SocketIOOptions
    {
        Query = new Dictionary<string, string> { { "token", "UNITY" } },
        Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
    })
    {
        JsonSerializer = new NewtonsoftJsonSerializer()
    };

    

    socket.OnDisconnected += (sender, e) =>
    {
        Debug.Log("Socket.IO disconnected.");
    };

    socket.Connect();
    }


    private void OnApplicationQuit() {
        socket.Disconnect();
    }
}
