using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public class WindowFullUnFull : MonoBehaviour
{
    public Toggle WinddowOn;
    public GameObject On, Off;
    //private string jsonPath = "C:\\Users\\B-ZONE\\OneDrive\\������� ����\\RTS_2024\\Assets\\Scrips\\GameScene\\JsonBuilding\\BuilduingAndUnit.json";
    private string jsonPath = "D:\\Projects\\RTS_2024\\Assets\\Scrips\\GameScene\\JsonBuilding\\BuilduingAndUnit.json";


    void Start()
    {
        LoadWindowMode();
        WinddowOn.onValueChanged.AddListener(SetFullscreenMode);
    }

    // ����� ��� ����� ������ ������
    void SetFullscreenMode(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;  // ����������� �����
        SaveWindowMode(isFullscreen);  // ��������� � JSON
        ChangeWindowMode(); // ��������� UI
    }

    // ����� ��� ��������� ������ � ��������� ������
    public void ChangeWindowMode()
    {
        if (WinddowOn.isOn)
        {
            WinddowOn.GetComponentInChildren<Text>().text = "ON";
        }
        else
        {
            WinddowOn.GetComponentInChildren<Text>().text = "OFF";
        }

        On.SetActive(WinddowOn.isOn);
        Off.SetActive(!WinddowOn.isOn);
    }

    void LoadWindowMode()
    {
        if (File.Exists(jsonPath))
        {
            string json = File.ReadAllText(jsonPath);
            JObject data = JObject.Parse(json);

            bool windowMode = data["GameSettings"]["windowMode"].ToObject<bool>();
            WinddowOn.isOn = windowMode;
            Screen.fullScreen = windowMode;
            ChangeWindowMode(); // ��������� UI ��� ������
        }
        else
        {
            Debug.LogError("���� JSON �� ������: " + jsonPath);
        }
    }

    void SaveWindowMode(bool isFullscreen)
    {
        if (File.Exists(jsonPath))
        {
            string json = File.ReadAllText(jsonPath);
            JObject data = JObject.Parse(json);

            data["GameSettings"]["windowMode"] = isFullscreen;
            File.WriteAllText(jsonPath, data.ToString());

            Debug.Log("����� ���� ������� � JSON: " + isFullscreen);
        }
        else
        {
            Debug.LogError("���� JSON �� ������: " + jsonPath);
        }
    }
}
