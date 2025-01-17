using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeName : MonoBehaviour
{
    public TMP_Text textComponent; // Ссылка на TextMeshPro
    public TMP_InputField inputField; // Ссылка на InputField

    public void UpdateTextButton() // Вызывается при нажатии кнопки
    {
        textComponent.text = inputField.text;
    }
}
