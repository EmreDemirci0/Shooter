using System;
using System.Collections;
using System.Collections.Generic;
using SocketIOClient;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class Player
{

    public string PlaName;
    public string socketID;
    public string userID;
    public string roomID;
    public Vector3 pos;
    public Vector3 rotate;
    public string animType;
    public bool isCam;
}
[Serializable]
public class Room
{
    public string roomID;
    public List<Player> players;
}

[Serializable]
public class Gun
{
    public string gunName;
    public string gunID;
    public string userID;
    public string CrossID;
    public int index;
}

public class SocketManager : Singleton<SocketManager>
{
    public SocketIOUnity socket;
    public Player player;
    public Room room = new();
    private System.Diagnostics.Stopwatch pingStopwatch; // Ping ölçmek için zamanlayıcı
    public float currentPing;

    //string uri = "http://185.242.161.111:1234";// server 
    //string uri = "http://10.20.48.179:1234";// LAN server
    string uri = "http://localhost:1234"; // localhost server

    void OnEnable()
    {
        socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Query = new Dictionary<string, string> { { "token", "UNITY" } }
        });
        socket.OnConnected += (s, e) =>
        {

        };
        socket.OnUnityThread("pong", response =>
       {
           // Sunucudan gelen "pong" cevabını işleyin
           if (pingStopwatch != null && pingStopwatch.IsRunning)
           {
               pingStopwatch.Stop();
               currentPing = pingStopwatch.ElapsedMilliseconds; // Ping değerini milisaniye olarak al
           }
       });
        StartCoroutine(SendPing());



        socket.OnDisconnected += (sender, e) =>
        {
            Debug.Log("Socket.IO disconnected.");
        };

        socket.Connect();
    }
    private void OnGUI()
    {
        // Ping değerini ekrana yazdır
        GUI.Label(new Rect(10, 10, 200, 20), $"Ping: {currentPing} ms");
    }
    private IEnumerator SendPing()
    {
        while (true)
        {
            if (socket.Connected)
            {
                pingStopwatch = System.Diagnostics.Stopwatch.StartNew(); // Zamanlayıcıyı başlat
                socket.Emit("ping"); // Sunucuya "ping" gönder
            }
            yield return new WaitForSeconds(5); // Her 5 saniyede bir ping gönder
        }
    }
    private void OnApplicationQuit()
    {
        socket.Disconnect();
    }
}
