using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    //ì¸óÕÇµÇΩÉLÅ[ÇÃKeyCodeéÊìæ
    KeyCode keycode;
    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(code))
                {
                    Debug.Log(code);
                    keycode = code;
                    GetComponent<TextMeshProUGUI>().text = code.ToString();
                    break;
                }
            }
        }
    }
}
