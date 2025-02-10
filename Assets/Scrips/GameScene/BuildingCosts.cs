using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.UI;

public class BuildingCosts : MonoBehaviour
{
    public ConfigManager configManager;

    public TMP_Text WoodMain, CropMain, RockMain, FoodMain;
    public Button Farm, Temple, Barrack1Lv, Granary, University, SiegeFactory, House, TownCenter, TowerLv1;
    public GameObject Farm3d, Temple3d, Barrack1Lv3d, Granary3d, University3d, SiegeFactory3d, House3d, TownCenter3d, TowerLv1_3d;
    public GameObject plane;
    public GameObject BuildingContainer;
    private int WoodCount, CropCount, RockCount, FoodCount;
    private GameObject currentFarm;
    private bool isPlacing = false;

    private Dictionary<Button, BuildingInfo> buildings;

    // Вспомогательный класс для хранения информации о префабе и имени здания
    private class BuildingInfo
    {
        public GameObject Prefab { get; }
        public string BuildingName { get; }

        public BuildingInfo(GameObject prefab, string buildingName)
        {
            Prefab = prefab;
            BuildingName = buildingName;
        }
    }

    void Start()
    {
        buildings = new Dictionary<Button, BuildingInfo>
        {
            { Farm, new BuildingInfo(Farm3d, "Farm") },
            { Granary, new BuildingInfo(Granary3d, "Granary") },
            { Barrack1Lv, new BuildingInfo(Barrack1Lv3d, "Barrack") },
            { SiegeFactory, new BuildingInfo(SiegeFactory3d, "Siege_Factory") },
            { House, new BuildingInfo(House3d, "House") },
            { Temple, new BuildingInfo(Temple3d, "Temple") },
            { University, new BuildingInfo(University3d, "University") },
            { TowerLv1, new BuildingInfo(TowerLv1_3d, "Tower") },
            { TownCenter, new BuildingInfo(TownCenter3d, "Town_Center") }
        };
    }

    void Update()
    {
        WoodCount = int.Parse(WoodMain.text);
        CropCount = int.Parse(CropMain.text);
        RockCount = int.Parse(RockMain.text);
        FoodCount = int.Parse(FoodMain.text);

        foreach (var entry in buildings)
        {
            UpdateButtonAvailability(entry.Key, entry.Value);
        }

        if (isPlacing && currentFarm != null)
        {
            MoveBuildingWithCursor();

            if (Input.GetMouseButtonDown(0))
            {
                if (CanPlaceHere(currentFarm.transform.position))
                {
                    PlaceFarm();
                }
                else
                {
                    Debug.Log("Нельзя разместить объект здесь!");
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("Отмена постройки!");
                Destroy(currentFarm);
                currentFarm = null;
                isPlacing = false;
            }
        }
    }

    private void UpdateButtonAvailability(Button button, BuildingInfo buildingInfo)
    {
        BuildingData config = configManager.GetBuildingConfig(buildingInfo.BuildingName);
        if (config == null) return;

        var cost = config.constructionCost;
        bool canAfford = WoodCount >= cost.wood
                      && RockCount >= cost.rock;

        Transform selectionSprite = button.transform.Find("PanelNo");
        selectionSprite.gameObject.SetActive(!canAfford);
    }

    private void UpdateUI()
    {
        WoodMain.text = WoodCount.ToString();
        RockMain.text = RockCount.ToString();
        CropMain.text = CropCount.ToString();
        FoodMain.text = FoodCount.ToString();
    }

    void MoveBuildingWithCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject == plane)
            {
                Vector3 newPosition = hit.point;
                currentFarm.transform.position = newPosition;

                Transform selectionSprite = currentFarm.transform.Find("SelectionSprite");
                if (selectionSprite != null)
                {
                    selectionSprite.GetComponent<Renderer>().material.color =
                        CanPlaceHere(newPosition) ? Color.green : Color.red;
                }
            }
        }
    }

    bool CanPlaceHere(Vector3 position)
    {
        float radius = 1f;
        Collider[] colliders = Physics.OverlapSphere(position, radius);

        foreach (Collider col in colliders)
        {
            if (col.gameObject != plane && col.gameObject != currentFarm)
            {
                return false;
            }
        }
        return true;
    }

    void PlaceFarm()
    {
        Debug.Log("Объект построен!");

        Transform selectionSprite = currentFarm.transform.Find("SelectionSprite");
        if (selectionSprite != null)
        {
            selectionSprite.GetComponent<Renderer>().material.color = Color.white;
            selectionSprite.gameObject.SetActive(false);
        }

        currentFarm.transform.SetParent(BuildingContainer.transform);

        isPlacing = false;
        currentFarm = null;
    }
    public void Build(Button button)
    {
        if (!buildings.ContainsKey(button)) return;

        var buildingInfo = buildings[button];
        var config = configManager.GetBuildingConfig(buildingInfo.BuildingName);
        if (config == null) return;

        var cost = config.constructionCost;

        if (WoodCount < cost.wood || RockCount < cost.rock)
        {
            Debug.Log($"Недостаточно ресурсов для постройки {config.name}!");
            return;
        }

        if (isPlacing) return;

        // Вычитаем ресурсы
        WoodCount -= cost.wood;
        RockCount -= cost.rock;

        UpdateUI();

        currentFarm = Instantiate(buildingInfo.Prefab);
        isPlacing = true;
    }
}