using System.IO;
using UnityEngine;

#region Application Settings

// ����� ��� �������� ������ �������� ����������
[System.Serializable]
public class AppSettingsData
{
    public string screenResolution; // ���������� ������, �������� "1920x1080"
    public string windowMode;       // ����� ����, �������� "Fullscreen" ��� "Windowed"
    public bool soundEnabled;       // �������� �� �����
    public bool musicEnabled;       // �������� �� ������
    public float volume;            // ��������� (�� 0 �� 1)
}

// ScriptableObject ��� �������� ����������
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

// ��������� ��� ����������� ������ ���������
[System.Serializable]
public class DifficultySettings
{
    public float spawnInterval;     // �������� ��������� ��������� ������
    public string[] unitTypes;      // ���� ������ (��������, "Swordsman", "Archer", "Catapult")
    public int initialArmyLimit;    // ��������� ����� �����
    public int armyIncrement;       // ���������� ������ ����� ��� ������ �����
}

// ��������� ��� �������� ���� ������� ���������
[System.Serializable]
public class DifficultyModifiers
{
    public DifficultySettings Easy;
    public DifficultySettings Medium;
    public DifficultySettings Hard;
}

// ����� ��� �������� ���� ���������� ������� ������
[System.Serializable]
public class GameSessionConfigData
{
    public DifficultyModifiers difficultyModifiers;
    public float freeZoneRadius;        // ������ ��������� ���� ������ ������
    public string[] trainingTexts;      // ������ ��������
    public float resourceSpawnTime;     // ����� ��������� ��������
    public int resourceCount;           // ���������� ��������
}

// ScriptableObject ��� ������������ ������� ������
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
    // ���� � ������� JSON ������
    private string appSettingsPath;
    private string gameSessionConfigPath;

    public AppSettings appSettings;
    public GameSessionConfig gameSessionConfig;

    private void Awake()
    {
        // ��������� ���� � ������ � ����� persistentDataPath
        appSettingsPath = Path.Combine(Application.persistentDataPath, "AppSettings.json");
        gameSessionConfigPath = Path.Combine(Application.persistentDataPath, "GameSessionConfig.json");

        // ��������� ���������
        LoadAppSettings();
        LoadGameSessionConfig();
    }

    #region AppSettings Methods

    // �������� �������� ����������
    public void LoadAppSettings()
    {
        if (File.Exists(appSettingsPath))
        {
            string json = File.ReadAllText(appSettingsPath);
            appSettings = ScriptableObject.CreateInstance<AppSettings>();
            appSettings.LoadFromJson(json);
            Debug.Log("AppSettings ������� ���������:\n" + json);
        }
        else
        {
            Debug.LogError("���� AppSettings.json �� ������ �� ����: " + appSettingsPath);
            // ��� ����� ������ ��������� �� ��������� ��� ��������� ������������
        }
    }

    // ���������� �������� ����������
    public void SaveAppSettings()
    {
        if (appSettings != null)
        {
            string json = appSettings.ToJson();
            File.WriteAllText(appSettingsPath, json);
            Debug.Log("AppSettings ���������:\n" + json);
        }
    }

    // ������ ��������� �������� (��������, ��������� ���������)
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

    // �������� ������������ ������� ������
    public void LoadGameSessionConfig()
    {
        if (File.Exists(gameSessionConfigPath))
        {
            string json = File.ReadAllText(gameSessionConfigPath);
            gameSessionConfig = ScriptableObject.CreateInstance<GameSessionConfig>();
            gameSessionConfig.LoadFromJson(json);
            Debug.Log("GameSessionConfig ������� ���������:\n" + json);
        }
        else
        {
            Debug.LogError("���� GameSessionConfig.json �� ������ �� ����: " + gameSessionConfigPath);
            // ����� ���� ����� ������� ��������� ������������ ��� ��������� �� ������
        }
    }

    #endregion
}
#endregion
