using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Akila.FPSFramework;
using System;
using Unity.Mathematics;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public GameObject player;
    public Transform spawnPos;
    void Start()
    {
        GameObject pla=Instantiate(player,spawnPos.position,quaternion.identity);
        UIManagerNew.Instance.SetPlaInfo(SocketManager.Instance.player);

    }
}
