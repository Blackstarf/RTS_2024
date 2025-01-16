using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Dropdown : MonoBehaviour
{
    public TMP_Dropdown DropdownSize;
    public void DropSize()
    {
        // Сохраняем текущий режим экрана
        bool isFullScreen = Screen.fullScreen;

        if (DropdownSize.value == 0)
        {
            Screen.SetResolution(1920, 1080, isFullScreen);
        }
        if (DropdownSize.value == 1)
        {
            Screen.SetResolution(1280, 720, isFullScreen);
        }
        if (DropdownSize.value == 2)
        {
            Screen.SetResolution(1366, 768, isFullScreen);
        }
        if (DropdownSize.value == 3)
        {
            Screen.SetResolution(1600, 900, isFullScreen);
        }
    }
}
