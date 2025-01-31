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
        // Получаем имя здания (убираем "(Clone)", если оно есть)
        string buildingName = gameObject.name.Replace("(Clone)", "").Trim();

        // Загружаем конфиг здания
        BuildingData config = configManager.GetBuildingConfig(buildingName);

        if (config == null)
        {
            Debug.LogError($"Конфиг для здания {buildingName} не найден!");
            return;
        }

        // Присваиваем значения из конфига
        maxHP = config.durability;
        currentHP = maxHP;

        Debug.Log($"{buildingName} инициализирован. Макс. HP: {maxHP}");
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log($"{gameObject.name} получил {damage} урона. Осталось HP: {currentHP}");

        if (currentHP <= 0)
        {
            DestroyBuilding();
        }
    }

    void DestroyBuilding()
    {
        Debug.Log($"{gameObject.name} уничтожен!");
        Destroy(gameObject);
    }
}
