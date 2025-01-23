using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoPlay : MonoBehaviour
{
    public TMP_InputField inputFieldSizeMap;
    public TMP_InputField inputFieldPlayer;
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
        string NamePlayer = inputFieldPlayer.text;
        float inputData = (float)int.Parse(inputFieldSizeMap.text);
        PlayerPrefs.SetFloat("SizeMap", inputData);
        PlayerPrefs.Save();
        PlayerPrefs.SetString("NamePlay", NamePlayer);
        PlayerPrefs.Save();
        Debug.Log("SizeMap: " + inputData);
    }
}
