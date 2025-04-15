using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Init : MonoBehaviour
{
    [SerializeField] private string nextSceneName="Main Menu";
    private void OnEnable() {
        SocketManager.Instance.socket.OnUnityThread("conn",dat=>{
            SceneManager.LoadScene(nextSceneName);
        });
    }
}
