using UnityEngine;

public class BuildingsHP : MonoBehaviour
{
    public ConfigManager configManager;
    private int maxHP;
    private int currentHP;
    private Attaka attakaScript;

    void Start()
    {
        // �������������� attakaScript
        attakaScript = FindObjectOfType<Attaka>();
        if (attakaScript == null)
        {
            Debug.LogError("Attaka script not found in the scene!");
        }

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
        if (attakaScript == null)
        {
            Debug.LogError("Attaka script is not initialized!");
            return;
        }

        // ���������, ���� �� �������� Town_Center � ZonePlayer
        int countPlayer = 0;
        if (attakaScript.ZonePlayer != null)
        {
            foreach (Transform child in attakaScript.ZonePlayer.transform)
            {
                if (child.name == "Town_Center")
                {
                    countPlayer++;
                }
            }
        }
        else
        {
            Debug.LogError("ZonePlayer �� ������!");
            return;
        }

        // ��������� ������������ ������ (�� �� ���� �����)
        Transform Vrag = gameObject.transform.parent;
        if (Vrag == null)
        {
            Debug.LogError("Vrag (parent of the building) is null!");
            return;
        }

        int countVrag = 0;
        foreach (Transform child in Vrag)
        {
            if (child.name == "Town_Center")
            {
                countVrag++;
            }
        }
        Debug.Log($"{countPlayer}  {gameObject.tag}   {gameObject.name}");
        // ������ ����������� ���� ������
        if (gameObject.name == "Town_Center" && gameObject.tag == "BasePlayer" && countPlayer == 1)
        {
            Debug.Log("���������� ���� ������ (ZonePlayer)");
            Destroy(attakaScript.ZonePlayer);
            attakaScript.PanelPlayerUI.SetActive(false);
            attakaScript.PanelEnd.SetActive(true);
            GameObject GameEnd = attakaScript.PanelEnd.transform.Find("GameOver")?.gameObject;
            GameObject MenuEnd = attakaScript.PanelEnd.transform.Find("Pause")?.gameObject;
            MenuEnd.SetActive(false);
            GameEnd.SetActive(true);
        }
        // ������ ����������� ���� �����
        else if (gameObject.name == "Town_Center" && gameObject.tag == "UnitVragBase" && countVrag == 1)
        {
            Debug.Log("���������� ���� ����� (Vrag)");
            Destroy(Vrag.gameObject);
        }
        // ������� ����������� ������
        else
        {
            Debug.Log($"{gameObject.name} ���������!");
            Destroy(gameObject);
        }
    }
}
