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
    //private string jsonPath = "C:\\Users\\B-ZONE\\OneDrive\\Рабочий стол\\RTS_2024\\Assets\\Scrips\\GameScene\\JsonBuilding\\BuilduingAndUnit.json";
    private string jsonPath = "D:\\Projects\\RTS_2024\\Assets\\Scrips\\GameScene\\JsonBuilding\\BuilduingAndUnit.json";


    void Start()
    {
        LoadWindowMode();
        WinddowOn.onValueChanged.AddListener(SetFullscreenMode);
    }

    // Метод для смены режима экрана
    void SetFullscreenMode(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;  // Переключаем режим
        SaveWindowMode(isFullscreen);  // Сохраняем в JSON
        ChangeWindowMode(); // Обновляем UI
    }

    // Метод для изменения текста и активации кнопок
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
            ChangeWindowMode(); // Обновляем UI при старте
        }
        else
        {
            Debug.LogError("Файл JSON не найден: " + jsonPath);
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

            Debug.Log("Режим окна сохранён в JSON: " + isFullscreen);
        }
        else
        {
            Debug.LogError("Файл JSON не найден: " + jsonPath);
        }
    }
}
