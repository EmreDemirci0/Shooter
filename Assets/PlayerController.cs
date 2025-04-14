using Akila.FPSFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Player player;
   public bool canMove = true;

    private void Start()
    {
        SetControl();
    }
    public void SetControl()
    {
        GetComponent<FirstPersonController>().canControl = canMove;
        foreach (var item in GetComponentInChildren<Inventory>().items)
        {
            item.gameObject.GetComponent<Firearm>().canMove = canMove;
        }
    }
}
