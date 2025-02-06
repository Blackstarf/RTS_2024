using System.IO;
using TMPro;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class Dropdown : MonoBehaviour
{
    public TMP_Dropdown DropdownSize;
    private string configFilePath= "Assets\\Scrips\\GameScene\\JsonBuilding\\BuilduingAndUnit.json";
    public void DropSize()
    {
        // Сохраняем текущий режим экрана
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
        // Читаем JSON из файла
        string json = File.ReadAllText(configFilePath);
        // Парсим строку в JObject
        JObject jObject = JObject.Parse(json);
        jObject["GameSettings"]["screenResolution"] = newResolution;
        // Преобразуем обновлённый JObject обратно в строку
        string newJson = jObject.ToString();
        // Записываем обновлённый JSON в файл
        File.WriteAllText(configFilePath, newJson);
    }
}
