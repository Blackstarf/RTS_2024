using System.IO;
using TMPro;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class Dropdown : MonoBehaviour
{
    public TMP_Dropdown DropdownSize;
    private string configFilePath;

    private void Awake()
    {
        // ����� ���� � ����� ��������, ��������, � Application.persistentDataPath
        configFilePath = Path.Combine(Application.persistentDataPath, "D:\\Projects\\RTS_2024\\Assets\\Scrips\\GameScene\\JsonBuilding\\BuilduingAndUnit.json");
        //configFilePath = Path.Combine(Application.persistentDataPath, "C:\\Users\\B-ZONE\\OneDrive\\������� ����\\RTS_2024\\Assets\\Scrips\\GameScene\\JsonBuilding\\BuilduingAndUnit.json");
    }

    public void DropSize()
    {
        // ��������� ������� ����� ������
        bool isFullScreen = Screen.fullScreen;
        string newResolution = "1920x1080";

        if (DropdownSize.value == 0)
        {
            Screen.SetResolution(1920, 1080, isFullScreen);
            newResolution = "1920x1080";
        }
        else if (DropdownSize.value == 1)
        {
            Screen.SetResolution(1280, 720, isFullScreen);
            newResolution = "1280x720";
        }
        else if (DropdownSize.value == 2)
        {
            Screen.SetResolution(1366, 768, isFullScreen);
            newResolution = "1366x768";
        }
        else if (DropdownSize.value == 3)
        {
            Screen.SetResolution(1600, 900, isFullScreen);
            newResolution = "1600x900";
        }

        // ����� ����� ���������� ��������� JSON
        UpdateGameSettingsResolution(newResolution);
    }

    private void UpdateGameSettingsResolution(string newResolution)
    {
        if (!File.Exists(configFilePath))
        {
            Debug.LogError("���� GameConfig.json �� ������ �� ����: " + configFilePath);
            return;
        }

        // ������ JSON �� �����
        string json = File.ReadAllText(configFilePath);

        // ������ ������ � JObject
        JObject jObject = JObject.Parse(json);

        // ���� ������ "GameSettings" � ��������� �������� "screenResolution"
        if (jObject["GameSettings"] != null)
        {
            jObject["GameSettings"]["screenResolution"] = newResolution;
        }
        else
        {
            Debug.LogError("������ 'GameSettings' �� ������� � JSON.");
            return;
        }

        // ����������� ���������� JObject ������� � ������
        string newJson = jObject.ToString();

        // ���������� ���������� JSON � ����
        File.WriteAllText(configFilePath, newJson);
        Debug.Log("��������� ���������� ������ � JSON: " + newResolution);
    }
}
