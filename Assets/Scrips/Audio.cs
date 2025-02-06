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
    private string jsonPath = "Assets\\Scrips\\GameScene\\JsonBuilding\\BuilduingAndUnit.json";
    string json;
    JObject data;

    private void Start()
    {
       // LoadAudioSettings();
        json = File.ReadAllText(jsonPath);
        data = JObject.Parse(json);
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
        data["GameSettings"]["soundEnabled"] = WinddowOn.isOn;
        File.WriteAllText(jsonPath, data.ToString());
        On.SetActive(isOn);
        Off.SetActive(!isOn);
    }

    public void ChangeVolume(float value)
    {
        audioSource.volume = WinddowOn.isOn ? value : 0;
        data["GameSettings"]["volume"] = slider.value;
        File.WriteAllText(jsonPath, data.ToString());
    }
}
