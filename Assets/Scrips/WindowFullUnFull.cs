using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class WindowFullUnFull : MonoBehaviour
{
    public Toggle WinddowOn;
    public GameObject On, Off;
    public Label Text;

    void Start()
    {
        // ������������� ��������� Toggle � ����������� �� �������� ������
        WinddowOn.isOn = Screen.fullScreen;

        // ��������� ���������� ������� ��������� Toggle
        WinddowOn.onValueChanged.AddListener(SetFullscreenMode);
    }

    // ����� ��� ����� ������ ������
    void SetFullscreenMode(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;  // ����������� �����
    }
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
