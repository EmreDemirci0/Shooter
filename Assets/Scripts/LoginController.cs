using System.Collections;
using System.Collections.Generic;
using Akila.FPSFramework;
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
    public GameObject[] panels;
    public GameObject[] ClosePanels;
    public GameObject menuGun;
    public Vector3 menuGunRotationSpeed = new Vector3(20f, 30f, 40f);
    void Start()
    {


        SocketManager.Instance.socket.OnUnityThread("JoinRooms", rooms =>
        {
            var room = JsonConvert.DeserializeObject<List<Room>>(rooms.ToString())[0];
            SocketManager.Instance.room = room;
            SocketManager.Instance.player.roomID = room.roomID;
            LoadingScreen.LoadScene("Game fadim");

        });


        SocketManager.Instance.socket.OnUnityThread("Rooms", rooms =>
        {
            foreach (var item in panels)
            {
                item.SetActive(true);
            }
            foreach (var item in ClosePanels)
            {
                item.SetActive(false);
            }
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
    }

    private void Update()
    {
        if (menuGun != null)
        {
            menuGun.transform.Rotate(menuGunRotationSpeed * Time.deltaTime);
        }
    }
    public void Register()
    {
        if (string.IsNullOrEmpty(nameInput.text))
        {
            return;
        }
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
    }

    public void Play()
    {

        SocketManager.Instance.socket.Emit("CreateRoom", nameInput.text);
    }
}
