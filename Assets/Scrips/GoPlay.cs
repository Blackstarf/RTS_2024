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
    public TMP_Dropdown NumVrags;
    public Button saveButton;
    public GameObject Vrag1, Vrag2, Vrag3, Vrag4, Vrag5, Vrag6, Player;
    private GameObject[] gameobjects;
    private Sprite[] Colors;
    public void SaveData()
    {
        gameobjects = new GameObject[] { Vrag1, Vrag2, Vrag3, Vrag4, Vrag5, Vrag6, Player };
        string NamePlayer = inputFieldPlayer.text;
        float inputData=0f;
        if (string.IsNullOrEmpty(inputFieldPlayer.text)|| inputFieldSizeMap.text== "Введите размер карты")
        {
            inputData = (float)int.Parse(inputFieldSizeMap.text);
        }
        foreach (GameObject obj in gameobjects)
        {
            if(obj.activeSelf == true)
            {
                // Поиск компонента Button в GameObject
                Button button = obj.GetComponentInChildren<Button>();
                Image buttonImage = button.GetComponent<Image>();
                string color;
                if(buttonImage.sprite.name== "ScrollBarBg")
                {
                    color = "White";
                }
                else
                {
                    color = buttonImage.sprite.name.Substring(4);
                }
                PlayerPrefs.SetString("Color" + obj.name, color);
                Debug.Log(obj.name);
            }
        }
        int n = NumVrags.value+1;
        PlayerPrefs.SetInt("CountVrags", n);
        PlayerPrefs.SetFloat("SizeMap", inputData);
        PlayerPrefs.Save();
        PlayerPrefs.SetString("NamePlay", NamePlayer);
        PlayerPrefs.Save();
        Debug.Log("SizeMap: " + inputData);
        
        if (inputData < 10000)
        {
            if (inputData >= 2500)
            {
                SceneManager.LoadScene("GameGame");
            }
        }
        
    }
}
