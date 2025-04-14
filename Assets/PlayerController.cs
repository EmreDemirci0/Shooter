using Akila.FPSFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Player player;
    public bool canMove = false;

    public Inventory inv;

    public void SetControl()
    {
        GetComponent<FirstPersonController>().canControl = canMove;
        Invoke(nameof(FireControl),.2f);

    }

    void FireControl()
    {
        foreach (var item in inv.items)
        {
            item.gameObject.GetComponentInChildren<Firearm>().canMove = canMove;
        }
    }
}
