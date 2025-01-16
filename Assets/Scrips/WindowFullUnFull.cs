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
        // Устанавливаем состояние Toggle в зависимости от текущего режима
        WinddowOn.isOn = Screen.fullScreen;

        // Добавляем обработчик события изменения Toggle
        WinddowOn.onValueChanged.AddListener(SetFullscreenMode);
    }

    // Метод для смены режима экрана
    void SetFullscreenMode(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;  // Переключаем режим
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
