using System;
using System.Collections;
using System.Collections.Generic;
using SocketIOClient;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class Player{

    public string PlaName;
    public string socketID;
    public string userID;
    public string roomID;
    public Vector3 pos;
    public Vector3 rotate;
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
        Query = new Dictionary<string, string> { { "token", "UNITY" } }
    });
    socket.OnConnected += (s, e) =>{
        socket.Emit("test","sa");
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
