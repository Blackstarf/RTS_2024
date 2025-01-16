using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddVrags : MonoBehaviour
{
    public TMP_Dropdown enemyCountDropdown;
    public GameObject enemiesImagePanel;
    public TMP_FontAsset customFont; 

    void Start()
    {
        // ���������� ������� � Dropdown
        enemyCountDropdown.onValueChanged.AddListener(OnEnemyCountChanged);
    }

    // ����� ��� ��������� ��������� ������ � Dropdown (�� ������ public)
    public void OnEnemyCountChanged(int selectedValue)
    {
        Debug.Log("��������� ���������� �����������: " + (selectedValue + 1));  // ���� 1 ��� ����� ������
        for (int i = enemiesImagePanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(enemiesImagePanel.transform.GetChild(i).gameObject);
        }
        // ����������� ���������� ��� ������� ����������
        float startX = 0f;
        float startY = 0f;
        float offsetY = 60f;  // ������ �� Y ��� ��������� �����������
        float offsetX = 0f;    // ���� �� ������ ������ �� ��� X, ����� ������

        // ������ ����� ����� ��� ������� ����������
        for (int i = 0; i < selectedValue; i++)
        {
            // ������ ��������� ��� ���������� � ����������� Image
            GameObject enemyContainer = new GameObject($"Enemy_{i + 1}", typeof(Image)); // ������ ����� � Image
            enemyContainer.transform.SetParent(enemiesImagePanel.transform);

            // �������� ��������� Image � ����������� ���
            Image enemyImage = enemyContainer.GetComponent<Image>();
            enemyImage.color = Color.gray;  // ������ ����� ���� ��� ����

            // ��������� �������� ���������� (���������� RectTransform)
            RectTransform rectTransform = enemyContainer.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(65, 4); // ������������� ������� ��� ������� ����� (������ 610, ������ 40)

            // ������������� ������� ��� �������� ���������� � ������ ��������
            rectTransform.anchoredPosition = new Vector2(startX + offsetX, startY - (i * offsetY)); // �������� �� Y ��� ������������ ���������

            // �������� � ��������� ������ "��������� X"
            GameObject textObj = new GameObject("EnemyText");
            textObj.transform.SetParent(enemyContainer.transform);
            TextMeshProUGUI enemyText = textObj.AddComponent<TextMeshProUGUI>();
            enemyText.text = $"��������� {i + 1}";
            enemyText.fontSize = 2;   // ������������� ������ ������ ������
            enemyText.color = Color.black;
            // ��������� ��� �����
            if (customFont != null)
            {
                enemyText.font = customFont; // ��������� ����������� �����
            }

            // ��������� RectTransform ��� ������
            RectTransform textRect = enemyText.GetComponent<RectTransform>();
            textRect.anchoredPosition = new Vector2(-20, 0);  // ��������� ����� � ����� ����

            // ������������� ������ � ������� ��� ������ (���� �����)
            textRect.sizeDelta = new Vector2(15,3); // ����� ������ � ������ ������ (������)
        }


    }
}
