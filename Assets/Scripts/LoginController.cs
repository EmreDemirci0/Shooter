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
    public JoinRoomController joinRoomPrefab;
    public Transform container;

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


        SocketManager.Instance.socket.OnUnityThread("Rooms", rooms =>
        {
            var room = JsonConvert.DeserializeObject<List<List<Room>>>(rooms.ToString())[0];
            for (int i = 0; i < container.childCount; i++)
            {
                print(container.GetChild(i).name);
                Destroy(container.GetChild(i).gameObject);

            }
            for (int i = 1; i < room.Count; i++)
            {
                JoinRoomController joinRoomController = Instantiate(joinRoomPrefab, container);
                string st = JsonUtility.ToJson(room[i]);

                joinRoomController.SetInfo(room[i]);
            }

        });
        SocketManager.Instance.socket.Emit("GetRooms");
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
