using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public class Audio : MonoBehaviour
{
    public Toggle WinddowOn;
    public GameObject On, Off;
    public Slider slider;
    public AudioSource audioSource;
    //private string jsonPath = "D:\\Projects\\RTS_2024\\Assets\\Scrips\\GameScene\\JsonBuilding\\BuilduingAndUnit.json";
    private string jsonPath = "C:\\Users\\B-ZONE\\OneDrive\\Рабочий стол\\RTS_2024\\Assets\\Scrips\\GameScene\\JsonBuilding\\BuilduingAndUnit.json";

    private void Start()
    {
        LoadAudioSettings();
        WinddowOn.onValueChanged.AddListener(ChangeWindowMode);
        slider.onValueChanged.AddListener(ChangeVolume);
    }

    private void Update()
    {
        audioSource.volume = WinddowOn.isOn ? slider.value : 0;
    }

    public void ChangeWindowMode(bool isOn)
    {
        if (isOn)
        {
            WinddowOn.GetComponentInChildren<Text>().text = "ON";
            audioSource.volume = slider.value;
        }
        else
        {
            WinddowOn.GetComponentInChildren<Text>().text = "OFF";
            audioSource.volume = 0;
        }

        On.SetActive(isOn);
        Off.SetActive(!isOn);

        SaveAudioSettings();
    }

    public void ChangeVolume(float value)
    {
        audioSource.volume = WinddowOn.isOn ? value : 0;
        SaveAudioSettings();
    }

    private void LoadAudioSettings()
    {
        if (File.Exists(jsonPath))
        {
            string json = File.ReadAllText(jsonPath);
            JObject data = JObject.Parse(json);

            bool soundEnabled = data["GameSettings"]["soundEnabled"].ToObject<bool>();
            float volume = data["GameSettings"]["volume"].ToObject<float>();

            WinddowOn.isOn = soundEnabled;
            slider.value = volume;
            audioSource.volume = soundEnabled ? volume : 0;

            ChangeWindowMode(soundEnabled);
        }
        else
        {
            Debug.LogError("Файл JSON не найден: " + jsonPath);
        }
    }

    private void SaveAudioSettings()
    {
        if (File.Exists(jsonPath))
        {
            string json = File.ReadAllText(jsonPath);
            JObject data = JObject.Parse(json);

            data["GameSettings"]["soundEnabled"] = WinddowOn.isOn;
            data["GameSettings"]["volume"] = slider.value;

            File.WriteAllText(jsonPath, data.ToString());
            Debug.Log("Настройки звука сохранены в JSON");
        }
        else
        {
            Debug.LogError("Файл JSON не найден: " + jsonPath);
        }
    }
}
