using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class AddVrags : MonoBehaviour
{
    public TMP_Dropdown enemyCountDropdown;
    public GameObject Player, Complexity, SizeMap;
    public GameObject Vrag1,Vrag2,Vrag3,Vrag4,Vrag5,Vrag6;
    public UnityEngine.UI.Button ButVrag1, ButVrag2, ButVrag3, ButVrag4, ButVrag5, ButVrag6, ButPlayer;
    public Sprite Blue, Green, Orange, Purple, Red, Yellow, White;
    private Sprite[] sprites;
    private UnityEngine.UI.Button[] buttons;
    private bool[] vrags;
    int startDropdown = 1;

    void Start()
    {
        vrags = new bool[] { Vrag1.gameObject.activeSelf, Vrag2.gameObject.activeSelf, Vrag3.gameObject.activeSelf, Vrag4.gameObject.activeSelf, Vrag5.gameObject.activeSelf, Vrag6.gameObject.activeSelf, Player.gameObject.activeSelf };
        enemyCountDropdown.onValueChanged.AddListener(OnEnemyCountChanged);
    }
    public void OnEnemyCountChanged(int selectedValue)
    {
        buttons = new UnityEngine.UI.Button[] { ButVrag1, ButVrag2, ButVrag3, ButVrag4, ButVrag5, ButVrag6, ButPlayer };
        sprites = new Sprite[] { Blue, Green, Orange, Purple, Red, Yellow, White };
        List<Sprite> colorList = new List<Sprite>() { Blue, Green, Orange, Purple, Red, Yellow, White };
        //Debug.Log("Выбранное количество противников: " + (selectedValue + 1));
        if (startDropdown == 1) 
        {
            startDropdown = selectedValue + 1;
            if (startDropdown == 2)
            {
                Vrag2.gameObject.SetActive(true);
                Vrag3.gameObject.SetActive(false);
                Vrag4.gameObject.SetActive(false);
                Vrag5.gameObject.SetActive(false);
                Vrag6.gameObject.SetActive(false);
                Vector3 newPositionPlayer = Coordinat(Player, 50f);
            }
            else if (startDropdown == 3)
            {
                Vrag2.gameObject.SetActive(true);
                Vrag3.gameObject.SetActive(true);
                Vrag4.gameObject.SetActive(false);
                Vrag5.gameObject.SetActive(false);
                Vrag6.gameObject.SetActive(false);
                Vector3 newPositionPlayer = Coordinat(Player, 100f);
            }
            else if (startDropdown == 4)
            {
                Vrag2.gameObject.SetActive(true);
                Vrag3.gameObject.SetActive(true);
                Vrag4.gameObject.SetActive(true);
                Vrag5.gameObject.SetActive(false);
                Vrag6.gameObject.SetActive(false);
                Vector3 newPositionPlayer = Coordinat(Player, 150f);
            }
            else if (startDropdown == 5)
            {
                Vrag2.gameObject.SetActive(true);
                Vrag3.gameObject.SetActive(true);
                Vrag4.gameObject.SetActive(true);
                Vrag5.gameObject.SetActive(true);
                Vrag6.gameObject.SetActive(false);
                Vector3 newPositionPlayer = Coordinat(Player, 200f);
            }
            else if (startDropdown == 6)
            {
                Vrag2.gameObject.SetActive(true);
                Vrag3.gameObject.SetActive(true);
                Vrag4.gameObject.SetActive(true);
                Vrag5.gameObject.SetActive(true);
                Vrag6.gameObject.SetActive(true);
                Vector3 newPositionPlayer = Coordinat(Player, 250f);
            }
            Sprite ButSpriteNOW, randomSprite;
            bool[] vragsNew= { Vrag1.gameObject.activeSelf, Vrag2.gameObject.activeSelf, Vrag3.gameObject.activeSelf, Vrag4.gameObject.activeSelf, Vrag5.gameObject.activeSelf, Vrag6.gameObject.activeSelf, Player.gameObject.activeSelf };
            int y=0;
            while (y != 7)
            {
                for (int i = 0; i < vragsNew.Length; i++)
                {
                    if (vrags[i] != vragsNew[i])
                    {
                        UnityEngine.UI.Image buttonImage = buttons[i].GetComponent<UnityEngine.UI.Image>();
                        Sprite currentSprite = buttonImage.sprite;
                        for (int j = 0; j < vragsNew.Length; j++)
                        {
                            if (vragsNew[i] == true)
                            {
                                UnityEngine.UI.Image buttonImageNOW = buttons[j].GetComponent<UnityEngine.UI.Image>();
                                ButSpriteNOW = buttonImageNOW.sprite;
                                colorList.Remove(ButSpriteNOW);
                                //break;
                            }
                        }
                        //Debug.Log(colorList.Count);
                        if (colorList.Count == 0)
                        {
                            //Debug.Log("colorList.Count=0");
                        }
                        else
                        {
                            randomSprite = colorList[UnityEngine.Random.Range(0, colorList.Count)];
                            if (currentSprite == randomSprite)
                            {
                                Debug.Log("Почти пиздец");
                                while (currentSprite == randomSprite)
                                {
                                    randomSprite = colorList[UnityEngine.Random.Range(0, colorList.Count)];
                                }
                                buttonImage.sprite = randomSprite;

                            }
                            else
                            {
                                buttonImage.sprite = randomSprite;
                            }
                        }
                    }
                   
                }
                vrags = vragsNew;
                y++;
                colorList = new List<Sprite>() { Blue, Green, Orange, Purple, Red, Yellow, White };
            }

        }
        else if(startDropdown != 1) 
        {
            Player.transform.localPosition = new Vector3(Player.transform.localPosition.x, 122, Player.transform.localPosition.z);
            Complexity.transform.localPosition = new Vector3(Complexity.transform.localPosition.x, 72, Complexity.transform.localPosition.z);
            SizeMap.transform.localPosition = new Vector3(SizeMap.transform.localPosition.x, 22, SizeMap.transform.localPosition.z);
            Vrag2.gameObject.SetActive(false);
            Vrag3.gameObject.SetActive(false);
            Vrag4.gameObject.SetActive(false);
            Vrag5.gameObject.SetActive(false);
            Vrag6.gameObject.SetActive(false);
        }
        startDropdown = selectedValue + 1;
    }
    Vector3 Coordinat(GameObject Image, float Y)
    {
        Player.transform.localPosition = new Vector3(Player.transform.localPosition.x, Player.transform.localPosition.y - Y, Player.transform.localPosition.z);
        Complexity.transform.localPosition = new Vector3(Complexity.transform.localPosition.x, Complexity.transform.localPosition.y - Y, Complexity.transform.localPosition.z);
        SizeMap.transform.localPosition = new Vector3(SizeMap.transform.localPosition.x, SizeMap.transform.localPosition.y - Y, SizeMap.transform.localPosition.z);
        return Image.transform.localPosition;
    }
}