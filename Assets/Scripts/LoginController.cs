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
        SocketManager.Instance.socket.OnUnityThread("JoinRooms",rooms=>{
            var room=JsonConvert.DeserializeObject<List<Room>>(rooms.ToString())[0];
            SocketManager.Instance.room=room;
            SceneManager.LoadScene("Game");

        });
    }

    public void Play()
    {
        if (!string.IsNullOrEmpty(nameInput.text))
        {
            Player pla=new()
            {
                PlaName = nameInput.text,
                roomID="test"
            };
            SocketManager.Instance.player=pla;
            string js=JsonUtility.ToJson(pla);
            SocketManager.Instance.socket.Emit("SetPla",js);
            print("att");

        }

    }
}
