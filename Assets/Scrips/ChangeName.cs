using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeName : MonoBehaviour
{
    public TMP_Text textComponent; // ������ �� TextMeshPro
    public TMP_InputField inputField; // ������ �� InputField

    public void UpdateTextButton() // ���������� ��� ������� ������
    {
        textComponent.text = inputField.text;
    }
}
