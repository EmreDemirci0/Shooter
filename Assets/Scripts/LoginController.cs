using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour
{
    public TMP_InputField nameInput;

    void Start()
    {
        Player pla = new()
        {
            socketID = SocketManager.Instance.socket.Id,
            userID = SocketManager.Instance.socket.Id,
            PlaName = nameInput.text,
            roomID = "test"
        };
        SocketManager.Instance.player = pla;
        string js = JsonUtility.ToJson(pla);
        SocketManager.Instance.socket.Emit("SetPla", js);

        SocketManager.Instance.socket.OnUnityThread("JoinRooms", rooms =>
        {
            var room = JsonConvert.DeserializeObject<List<Room>>(rooms.ToString())[0];
            SocketManager.Instance.room = room;
            SocketManager.Instance.player.roomID = room.roomID;
            SceneManager.LoadScene("Game");

        });
    }

    public void CreateRoom()
    {
        if (!string.IsNullOrEmpty(nameInput.text))
        {

            SocketManager.Instance.socket.Emit("CreateRoom", nameInput.text);

        }

    }

    public void Play()
    {

    }
}
