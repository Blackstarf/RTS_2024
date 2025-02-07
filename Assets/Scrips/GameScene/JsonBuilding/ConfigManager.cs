using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

// Стоимость тренировки или строительства
[System.Serializable]
public class Cost
{
    public int wood;
    public int rock;
}

// Атака юнита или здания
[System.Serializable]
public class Attack
{
    public float minRange;
    public float maxRange;
    public float attackDelay;
    public int damage;
}

// Лечение
[System.Serializable]
public class Healing
{
    public float minRange;
    public float maxRange;
    public float healingDelay;
    public int healingAmount;
}

// Характеристики строителя
[System.Serializable]
public class Builder
{
    public float resourceGatheringSpeed;
    public float repairSpeed;
    public int repairEfficiency;
}
[System.Serializable]
public class GameConfig
{
    public List<UnitData> units;
    public List<BuildingData> buildings;
}

[System.Serializable]
public class UnitData
{
    public string name;
    public float movementSpeed;
    public int health;
    public Cost trainingCost;
    public float detectionRange;
    public Attack attack;
    public Builder builder;
    public Healing healing;
    public int capacity;
}

[System.Serializable]
public class BuildingData
{
    public string name;
    public int durability;
    public Cost constructionCost;
    public List<string> trainsUnits;
    public string producesResource;
    public float detectionRadius;
    public float buildZoneRadius;
    public int archerCapacity;
    public Attack attack;
    public bool increasesMaxResources;
}
// ScriptableObject для юнита
[CreateAssetMenu(fileName = "UnitConfig", menuName = "Configs/Unit", order = 0)]
public class UnitConfig : ScriptableObject
{
    public string unitName;
    public float movementSpeed;
    public int health;
    public Cost trainingCost;
    public float detectionRange;
    public Attack attack;
    public Builder builder;
    public Healing healing;
    public int capacity; // Для осадной башни
}

// ScriptableObject для здания
[CreateAssetMenu(fileName = "BuildingConfig", menuName = "Configs/Building", order = 1)]
public class BuildingConfig : ScriptableObject
{
    public string buildingName;
    public int durability;
    public Cost constructionCost;
    public List<string> trainsUnits;
    public string producesResource;
    public float detectionRadius;
    public float buildZoneRadius;
    public int archerCapacity;
    public Attack attack;
    public bool increasesMaxResources;
}

// Менеджер для работы с конфигурациями
public class ConfigManager : MonoBehaviour
{
    private GameConfig config;

    private void Awake()
    {
        LoadConfig();
    }

    private void LoadConfig()
    {
        //string path = Path.Combine(Application.dataPath, "D:\\Projects\\RTS_2024\\Assets\\Scrips\\GameScene\\JsonBuilding\\BuilduingAndUnit.json");
        string path = Path.Combine(Application.dataPath, "C:\\Users\\B-ZONE\\OneDrive\\Рабочий стол\\RTS_2024\\Assets\\Scrips\\GameScene\\JsonBuilding\\BuilduingAndUnit.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            config = JsonUtility.FromJson<GameConfig>(json);
        }
        else
        {
            Debug.LogError("JSON config file not found at: " + path);
        }
    }
    public UnitData GetUnitConfig(string unitName)
    {
        return config?.units.FirstOrDefault(u => u.name == unitName);
    }

    public BuildingData GetBuildingConfig(string buildingName)
    {
        return config?.buildings.FirstOrDefault(b => b.name == buildingName);
    }
}