using Akila.FPSFramework;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Player player;
    public bool canMove = false;

    public Inventory inv;
    public TextMeshPro NameText;
    //public Firearm firearm;


    private void Start()
    {
        NameText.text = player.PlaName;
    }
    public void SetControl()
    {
        GetComponent<FirstPersonController>().canControl = canMove;
    }
    void Update()
    {
        //??? düzgün değil çok yanlış düzeltilecek !!!
        FireControl();
    }

    void FireControl()
    {
        foreach (var item in inv.items)
        {
            if (!item)
                return;
            if (item.gameObject.TryGetComponent<Firearm>(out var fire))
            {
                fire.canMove = canMove;
            }
        }
    }
}
