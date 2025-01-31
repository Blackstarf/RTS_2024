using UnityEngine;

public class BuildingsHP : MonoBehaviour
{
    public ConfigManager configManager;

    private int maxHP;
    public int currentHP;

    void Start()
    {
        InitializeBuildingStats();
    }

    void InitializeBuildingStats()
    {
        // �������� ��� ������ (������� "(Clone)", ���� ��� ����)
        string buildingName = gameObject.name.Replace("(Clone)", "").Trim();

        // ��������� ������ ������
        BuildingData config = configManager.GetBuildingConfig(buildingName);

        if (config == null)
        {
            Debug.LogError($"������ ��� ������ {buildingName} �� ������!");
            return;
        }

        // ����������� �������� �� �������
        maxHP = config.durability;
        currentHP = maxHP;

        Debug.Log($"{buildingName} ���������������. ����. HP: {maxHP}");
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log($"{gameObject.name} ������� {damage} �����. �������� HP: {currentHP}");

        if (currentHP <= 0)
        {
            DestroyBuilding();
        }
    }

    void DestroyBuilding()
    {
        Debug.Log($"{gameObject.name} ���������!");
        Destroy(gameObject);
    }
}
