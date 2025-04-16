using Akila.FPSFramework.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testcanvas : MonoBehaviour
{
    AttachmentSwitching test;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(test is null)
		{
            Debug.Log("atmaa yapýldý");
            test = FindObjectOfType<AttachmentSwitching>();
		}
		if (Input.GetKeyDown(KeyCode.K))
		{
            holo("Sight/Holo Sight");
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            x8("Sight/8X Scope");
        }

    }
    public void holo(string a)
	{
        Debug.Log("holo takýlmalý");
        test.Switch(a); /*Sight / Holo Sight*/
    }
   public  void x8(string a)
    {
        Debug.Log("8x takýlmalý");
        test.Switch(a); /*Sight / 8X Scope*/
    }
}
