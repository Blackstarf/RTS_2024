using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MusicOnOff : MonoBehaviour
{
    public UnityEngine.UI.Toggle WinddowOn;
    public GameObject On, Off;
    public Label Text;
    public void ChangeWindowMode()
    {
        if (WinddowOn.isOn)
        {
            WinddowOn.GetComponentInChildren<Text>().text = "ON";
        }
        else WinddowOn.GetComponentInChildren<Text>().text = "OFF";
        On.SetActive(WinddowOn.isOn);
        Off.SetActive(!WinddowOn.isOn);
    }
}
