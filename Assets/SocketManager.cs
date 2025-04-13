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
    public string name;
    public string roomID;
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
    string uri="http://localhost:3000";
    void Start()
    {
        socket = new SocketIOUnity(uri, new SocketIOOptions
            {
                Query = new Dictionary<string, string> { { "token", "UNITY" } },
                Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
            })
            {
                JsonSerializer = new NewtonsoftJsonSerializer()
            };
socket = new SocketIOUnity(uri, new SocketIOOptions
            {
                Query = new Dictionary<string, string> { { "token", "UNITY" } },
                Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
            })
            {
                JsonSerializer = new NewtonsoftJsonSerializer()
            };
            socket.Connect();
            socket.OnUnityThread("conn",dat=>{
                SceneManager.LoadScene("Login");
            });
    }

    private void OnApplicationQuit() {
        socket.Disconnect();
    }
}
