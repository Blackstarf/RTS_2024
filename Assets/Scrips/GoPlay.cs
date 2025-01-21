using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoPlay : MonoBehaviour
{
    public TMP_InputField inputField;
    public Button saveButton; 
    public void SwitchScene()
    {
        SceneManager.LoadScene("GameGame");
    }
    private void Start()
    {
        saveButton.onClick.AddListener(SaveData);
    }

    public void SaveData()
    {
        float inputData = (float)int.Parse(inputField.text);
        PlayerPrefs.SetFloat("SizeMap", inputData);
        PlayerPrefs.Save();
        Debug.Log("SizeMap: " + inputData);
    }
}
