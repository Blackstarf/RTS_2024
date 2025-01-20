using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class OneColor : MonoBehaviour
{
    public Sprite Blue, Green, LightGreen, Orange, Purple, Red, Yellow, White;
    public GameObject Vrag1, Vrag2, Vrag3, Vrag4, Vrag5, Vrag6, Player;
    public UnityEngine.UI.Button ButVrag1, ButVrag2, ButVrag3, ButVrag4, ButVrag5, ButVrag6, ButPlayer;
    private GameObject[] gameobjects;
    private Sprite[] sprites;
    private UnityEngine.UI.Button[] buttons;
    private Dictionary<TMP_Dropdown, Sprite> previousSelections = new Dictionary<TMP_Dropdown, Sprite>();
    void Start()
    {
        gameobjects = new GameObject[] { Vrag1, Vrag2, Vrag3, Vrag4, Vrag5, Vrag6, Player };
        sprites = new Sprite[] { Blue, Green, LightGreen, Orange, Purple, Red, Yellow, White };
        buttons = new UnityEngine.UI.Button[] { ButVrag1, ButVrag2, ButVrag3, ButVrag4, ButVrag5, ButVrag6, ButPlayer };

        foreach (UnityEngine.UI.Button but in buttons)
        {
            int index = Array.IndexOf(buttons, but);
            but.onClick.AddListener(() => ButtonsColors(sprites, buttons, index, gameobjects));
        }
    }
    void ButtonsColors(Sprite[] sprites, UnityEngine.UI.Button[] buttons, int index, GameObject[] gameobjects)
    {
        // Получите текущий спрайт кнопки
        UnityEngine.UI.Image buttonImage = buttons[index].GetComponent<UnityEngine.UI.Image>();

        if (buttonImage != null)
        {
            Sprite currentSprite = buttonImage.sprite;
            Sprite ButSpriteNOW;
            Debug.Log(currentSprite);

            Sprite randomSprite = sprites[UnityEngine.Random.Range(0, sprites.Length)];
            int i = 0;
            while (i != 1)
            {
                if (currentSprite != randomSprite)
                {
                    int j = 0;
                    List<Sprite> colorList= new List<Sprite>() { Blue, Green, LightGreen, Orange, Purple, Red, Yellow, White };
                    while (j != 1)
                    {
                        for(int f =0;f< gameobjects.Length;f++)
                        {
                            Debug.Log(gameobjects[f].gameObject.activeSelf);
                            if (gameobjects[f].gameObject.activeSelf == true)
                            {
                                UnityEngine.UI.Image buttonImageNOW = buttons[f].GetComponent<UnityEngine.UI.Image>();
                                ButSpriteNOW = buttonImageNOW.sprite;
                                colorList.Remove(ButSpriteNOW);     
                            }
                        }
                        randomSprite = colorList[UnityEngine.Random.Range(0, colorList.Count)];
                        buttonImage.sprite = randomSprite;
                        j++;
                    }
                    colorList = new List<Sprite>() { Blue, Green, Orange, Purple, Red, Yellow, White };
                    j = 0;
                    i++;
                }
                else
                {
                    randomSprite = sprites[UnityEngine.Random.Range(0, sprites.Length)];
                    Debug.Log("Уже используется,но изменилось");
                }
            }
            
            i = 0;
        }
        else
        {
            Debug.LogError("Кнопка не имеет компонента Image");
        }
    }

    //void ChangeImage(Sprite RandomColor)
    //{
    //    // Присвойте существующий спрайт переменной newSprite
    //    Sprite newSprite = RandomColor;

    //    // Установите новый спрайт для кнопки
    //    GetComponent<UnityEngine.UI.Image>().sprite = RandomColor;
    //}
}
