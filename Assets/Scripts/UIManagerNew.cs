using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManagerNew : Singleton<UIManagerNew>
{
    public TextMeshProUGUI nameTmp;
    
    public void SetPlaInfo(Player pla)
    {
        nameTmp.text = pla.PlaName;
    }
}
