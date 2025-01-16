using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WindowFullUnFull : MonoBehaviour
{
    public Toggle WinddowOn;
    public GameObject On, Off;
    public TextMeshProUGUI Text;
    public void ChangeWindowMode() 
    {
        if (WinddowOn.isOn)
        {
            Text.text = "ON";
        }
        else Text.text = "OFF";
        On.SetActive(WinddowOn.isOn);
        Off.SetActive(!WinddowOn.isOn);
    }
}
