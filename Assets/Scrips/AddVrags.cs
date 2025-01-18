using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class AddVrags : MonoBehaviour
{
    public TMP_Dropdown enemyCountDropdown;
    public GameObject Vrag2,Vrag3,Vrag4,Vrag5,Vrag6;
    public GameObject Player, Complexity, SizeMap;
    int startDropdown = 1;

    void Start()
    {
        enemyCountDropdown.onValueChanged.AddListener(OnEnemyCountChanged);
    }
    public void OnEnemyCountChanged(int selectedValue)
    {
        Debug.Log("Выбранное количество противников: " + (selectedValue + 1));  // Плюс 1 для учёта сдвига
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
            //Debug.Log(startDropdown);
        }
        else if(startDropdown != 1) 
        {
            Debug.Log("Выбранный пиздец: " + (selectedValue + 1));  // Плюс 1 для учёта сдвига
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