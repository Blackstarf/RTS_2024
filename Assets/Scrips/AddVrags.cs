using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AddVrags : MonoBehaviour
{
    public TMP_Dropdown enemyCountDropdown;
    public GameObject Vrag2,Vrag3,Vrag4,Vrag5,Vrag6;
    public UnityEngine.UI.Button ButVrag1, ButVrag2, ButVrag3, ButVrag4, ButVrag5, ButVrag6, ButPlayer;
    private UnityEngine.UI.Button[] buttons;
    public GameObject Player, Complexity, SizeMap;
    int startDropdown = 1;

    void Start()
    {
        buttons = new UnityEngine.UI.Button[] { ButVrag1, ButVrag2, ButVrag3, ButVrag4, ButVrag5, ButVrag6, ButPlayer };

        enemyCountDropdown.onValueChanged.AddListener(OnEnemyCountChanged);
    }
    public void OnEnemyCountChanged(int selectedValue)
    {
         //List<Sprite> colorList = new List<Sprite>() { Blue, Green, LightGreen, Orange, Purple, Red, Yellow, White };
        //Debug.Log("��������� ���������� �����������: " + (selectedValue + 1));
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
        }
        else if(startDropdown != 1) 
        {
            Debug.Log("��������� ������: " + (selectedValue + 1));  // ���� 1 ��� ����� ������
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
        Debug.Log(startDropdown);
    }
    Vector3 Coordinat(GameObject Image, float Y)
    {
        Player.transform.localPosition = new Vector3(Player.transform.localPosition.x, Player.transform.localPosition.y - Y, Player.transform.localPosition.z);
        Complexity.transform.localPosition = new Vector3(Complexity.transform.localPosition.x, Complexity.transform.localPosition.y - Y, Complexity.transform.localPosition.z);
        SizeMap.transform.localPosition = new Vector3(SizeMap.transform.localPosition.x, SizeMap.transform.localPosition.y - Y, SizeMap.transform.localPosition.z);
        return Image.transform.localPosition;
    }
}