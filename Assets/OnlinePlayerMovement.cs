using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OnlinePlayerMovement 
{
    public string userId;
    public float speed;
    //public Vector3 pos; 
}

[Serializable]
public class Fire
{
    public string id;
    public Vector3 firePosition = Vector3.zero;
    public Quaternion fireRotation = Quaternion.identity;
    public Vector3 fireDirection = Vector3.zero;

}
