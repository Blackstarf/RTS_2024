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
    private string jsonPath = "Assets\\Scrips\\GameScene\\JsonBuilding\\BuilduingAndUnit.json";
    //private string jsonPath = "D:\\Projects\\RTS_2024\\Assets\\Scrips\\GameScene\\JsonBuilding\\BuilduingAndUnit.json";
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
        string json = File.ReadAllText(jsonPath);
        JObject data = JObject.Parse(json);
        bool windowMode = data["GameSettings"]["windowMode"].ToObject<bool>();
        Screen.fullScreen = !windowMode;
        data["GameSettings"]["windowMode"] = WinddowOn.isOn;
        File.WriteAllText(jsonPath, data.ToString());
        Debug.Log(WinddowOn.isOn);
        On.SetActive(WinddowOn.isOn);
        Off.SetActive(!WinddowOn.isOn);
    }
}
