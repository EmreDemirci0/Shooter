using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Init : MonoBehaviour
{
    public string SceneName="Main Menu";
    private void Start() {
        SocketManager.Instance.socket.OnUnityThread("conn",dat=>{
            SceneManager.LoadScene(SceneName);
        });
    }
}
