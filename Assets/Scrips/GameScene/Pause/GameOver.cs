using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    // ���������� ��� �������� ���������� ��������
    private static int woodCollected = 0;
    private static int rockCollected = 0;
    private static int cropCollected = 0;
    private static int foodCollected = 0;

    // ���������� ��� �������� ������� ������
    private float sessionTime = 0f;

    // ������ �� ��������� ���� ��� ����������� ����������
    public TMP_Text woodText;
    public TMP_Text rockText;
    public TMP_Text cropText;
    public TMP_Text foodText;
    public TMP_Text timeText;

    private void Update()
    {
        // ��������� ����� ������
        sessionTime += Time.deltaTime;

        // ��������� UI
        UpdateUI();
    }

    // ����� ��� ���������� ��������
    public static void AddResource(string resourceType, int amount)
    {
        switch (resourceType)
        {
            case "wood":
                woodCollected += amount;
                break;
            case "rock":
                rockCollected += amount;
                break;
            case "crop":
                cropCollected += amount;
                break;
            case "food":
                foodCollected += amount;
                break;
            default:
                Debug.LogWarning($"Unknown resource type: {resourceType}");
                break;
        }
    }

    // ����� ��� ���������� UI
    private void UpdateUI()
    {
        if (woodText != null) woodText.text = $"{woodCollected}";
        if (rockText != null) rockText.text = $"{rockCollected}";
        if (cropText != null) cropText.text = $"{cropCollected}";
        if (foodText != null) foodText.text = $"{foodCollected}";

        // ����������� ����� � ������ � �������
        int minutes = Mathf.FloorToInt(sessionTime / 60f);
        int seconds = Mathf.FloorToInt(sessionTime % 60f);
        if (timeText != null) timeText.text = $"{minutes:00}:{seconds:00}";
    }
}
