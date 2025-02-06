using System.IO;
using UnityEngine;

#region Application Settings

// Класс для хранения данных настроек приложения
[System.Serializable]
public class AppSettingsData
{
    public string screenResolution; // разрешение экрана, например "1920x1080"
    public string windowMode;       // режим окна, например "Fullscreen" или "Windowed"
    public bool soundEnabled;       // включены ли звуки
    public bool musicEnabled;       // включена ли музыка
    public float volume;            // громкость (от 0 до 1)
}

// ScriptableObject для настроек приложения
public class AppSettings : ScriptableObject
{
    public AppSettingsData data;

    public void LoadFromJson(string json)
    {
        data = JsonUtility.FromJson<AppSettingsData>(json);
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(data, true);
    }
}
#endregion

#region Game Session Configuration

// Настройки для конкретного уровня сложности
[System.Serializable]
public class DifficultySettings
{
    public float spawnInterval;     // интервал появления вражеских юнитов
    public string[] unitTypes;      // типы юнитов (например, "Swordsman", "Archer", "Catapult")
    public int initialArmyLimit;    // начальный лимит армии
    public int armyIncrement;       // увеличение лимита армии при каждой атаке
}

// Контейнер для настроек всех уровней сложности
[System.Serializable]
public class DifficultyModifiers
{
    public DifficultySettings Easy;
    public DifficultySettings Medium;
    public DifficultySettings Hard;
}

// Класс для хранения всех параметров игровой сессии
[System.Serializable]
public class GameSessionConfigData
{
    public DifficultyModifiers difficultyModifiers;
    public float freeZoneRadius;        // радиус свободной зоны вокруг Ратуши
    public string[] trainingTexts;      // тексты обучения
    public float resourceSpawnTime;     // время появления ресурсов
    public int resourceCount;           // количество ресурсов
}

// ScriptableObject для конфигурации игровой сессии
public class GameSessionConfig : ScriptableObject
{
    public GameSessionConfigData data;

    public void LoadFromJson(string json)
    {
        data = JsonUtility.FromJson<GameSessionConfigData>(json);
    }
}
#endregion

#region ConfigManager

public class ConfigManagerGameSes : MonoBehaviour
{
    // Пути к внешним JSON файлам
    private string appSettingsPath;
    private string gameSessionConfigPath;

    public AppSettings appSettings;
    public GameSessionConfig gameSessionConfig;

    private void Awake()
    {
        // Формируем пути к файлам в папке persistentDataPath
        appSettingsPath = Path.Combine(Application.persistentDataPath, "AppSettings.json");
        gameSessionConfigPath = Path.Combine(Application.persistentDataPath, "GameSessionConfig.json");

        // Загружаем настройки
        LoadAppSettings();
        LoadGameSessionConfig();
    }

    #region AppSettings Methods

    // Загрузка настроек приложения
    public void LoadAppSettings()
    {
        if (File.Exists(appSettingsPath))
        {
            string json = File.ReadAllText(appSettingsPath);
            appSettings = ScriptableObject.CreateInstance<AppSettings>();
            appSettings.LoadFromJson(json);
            Debug.Log("AppSettings успешно загружены:\n" + json);
        }
        else
        {
            Debug.LogError("Файл AppSettings.json не найден по пути: " + appSettingsPath);
            // Тут можно задать поведение по умолчанию или уведомить пользователя
        }
    }

    // Сохранение настроек приложения
    public void SaveAppSettings()
    {
        if (appSettings != null)
        {
            string json = appSettings.ToJson();
            File.WriteAllText(appSettingsPath, json);
            Debug.Log("AppSettings сохранены:\n" + json);
        }
    }

    // Пример изменения настроек (например, изменение громкости)
    public void SetVolume(float newVolume)
    {
        if (appSettings != null && appSettings.data != null)
        {
            appSettings.data.volume = newVolume;
            SaveAppSettings();
        }
    }

    #endregion

    #region GameSessionConfig Methods

    // Загрузка конфигурации игровой сессии
    public void LoadGameSessionConfig()
    {
        if (File.Exists(gameSessionConfigPath))
        {
            string json = File.ReadAllText(gameSessionConfigPath);
            gameSessionConfig = ScriptableObject.CreateInstance<GameSessionConfig>();
            gameSessionConfig.LoadFromJson(json);
            Debug.Log("GameSessionConfig успешно загружена:\n" + json);
        }
        else
        {
            Debug.LogError("Файл GameSessionConfig.json не найден по пути: " + gameSessionConfigPath);
            // Здесь тоже можно создать дефолтную конфигурацию или уведомить об ошибке
        }
    }

    #endregion
}
#endregion
