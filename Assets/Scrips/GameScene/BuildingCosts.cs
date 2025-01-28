using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.UI;

public class BuildingCosts : MonoBehaviour
{
    public TMP_Text WoodMain, CropMain, RockMain, FoodMain;
    public Button Farm, Temple, Barrack1Lv, Granary, University, SiegeFactory, House, TownCenter, TowerLv1;
    public GameObject Farm3d, Temple3d, Barrack1Lv3d, Granary3d, University3d, SiegeFactory3d, House3d, TownCenter3d, TowerLv1_3d;
    public GameObject plane;
    public GameObject BuildingContainer;
    private int WoodCount, CropCount, RockCount, FoodCount;
    private GameObject currentFarm; // ������� ������������ ������
    private bool isPlacing = false; // ���� ��� ������������ ������ ���������

    private Dictionary<Button, BuildingData> buildings;

    void Start()
    {
        buildings = new Dictionary<Button, BuildingData>
        {
            { Farm, new BuildingData(Farm3d, 5, 5) },
            { Granary, new BuildingData(Granary3d, 7, 7) },
            { Barrack1Lv, new BuildingData(Barrack1Lv3d, 7, 15) },
            { SiegeFactory, new BuildingData(SiegeFactory3d, 15, 25) },
            { House, new BuildingData(House3d, 5, 10) },
            { Temple, new BuildingData(Temple3d, 7, 15) }
        };
    }

    void Update()
    {
        // ��������� �������
        WoodCount = int.Parse(WoodMain.text);
        CropCount = int.Parse(CropMain.text);
        RockCount = int.Parse(RockMain.text);
        FoodCount = int.Parse(FoodMain.text);

        // ��������� ����������� ������
        foreach (var entry in buildings)
        {
            UpdateButtonAvailability(entry.Key, entry.Value);
        }

        // ������ ����������
        if (isPlacing && currentFarm != null)
        {
            MoveBuildingWithCursor();

            if (Input.GetMouseButtonDown(0)) // ���� ������ ���
            {
                if (CanPlaceHere(currentFarm.transform.position))
                {
                    PlaceFarm();
                }
                else
                {
                    Debug.Log("������ ���������� ������ �����!"); // ����� ������
                }
            }

            if (Input.GetMouseButtonDown(1)) // ������ ��������� (���)
            {
                CancelPlacement();
            }
        }
    }

    private void UpdateButtonAvailability(Button button, BuildingData buildingData)
    {
        bool isAvailable = WoodCount >= buildingData.WoodCost && RockCount >= buildingData.RockCost;
        Transform selectionSprite = button.transform.Find("PanelNo");
        selectionSprite.gameObject.SetActive(!isAvailable);
    }

    public void Build(Button button)
    {
        if (!buildings.ContainsKey(button)) return;

        BuildingData buildingData = buildings[button];

        if (WoodCount < buildingData.WoodCost || RockCount < buildingData.RockCost)
        {
            Debug.Log($"������������ �������� ��� ��������� {buildingData.Prefab.name}! ������ ������� ��� � �����!!!");
            return;
        }

        if (isPlacing) return; // ���� ��� ���������, ������ �� ������

        // �������� �������
        WoodCount -= buildingData.WoodCost;
        RockCount -= buildingData.RockCost;
        WoodMain.text = WoodCount.ToString();
        RockMain.text = RockCount.ToString();

        // �������� ����������
        currentFarm = Instantiate(buildingData.Prefab);
        isPlacing = true;
    }

    void MoveBuildingWithCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject == plane) // ���������, ������ �� � ���������
            {
                Vector3 newPosition = hit.point; // �������� ����� �� ���������
                currentFarm.transform.position = newPosition;

                // ��������� ����������� ���������� �����
                Transform selectionSprite = currentFarm.transform.Find("SelectionSprite");
                if (selectionSprite != null)
                {
                    if (CanPlaceHere(newPosition))
                    {
                        // ���� ����� ���������, ������������� ������� ����
                        selectionSprite.GetComponent<Renderer>().material.color = Color.green;
                    }
                    else
                    {
                        // ���� ������ ���������, ������������� ������� ����
                        selectionSprite.GetComponent<Renderer>().material.color = Color.red;
                    }
                }
            }
        }
    }

    bool CanPlaceHere(Vector3 position)
    {
        float radius = 1f; // ������ ��������
        Collider[] colliders = Physics.OverlapSphere(position, radius);

        foreach (Collider col in colliders)
        {
            if (col.gameObject != plane && col.gameObject != currentFarm) // ���������� ��������� � ������� ������
            {
                return false; // ����� ������
            }
        }

        return true; // ����� ��������
    }

    void PlaceFarm()
    {
        Debug.Log("������ ��������!");

        // ������������� ����� ���� � �������� SelectionSprite
        Transform selectionSprite = currentFarm.transform.Find("SelectionSprite");
        if (selectionSprite != null)
        {
            selectionSprite.GetComponent<Renderer>().material.color = Color.white; // ������������� ����� ����
            selectionSprite.gameObject.SetActive(false); // �������� ������
        }

        // ��������� ������ � ���������
        currentFarm.transform.SetParent(BuildingContainer.transform);

        // ��������� NavMeshSurface
        NavMeshSurface[] surfaces = FindObjectsOfType<NavMeshSurface>();
        foreach (var surface in surfaces)
        {
            surface.BuildNavMesh();
        }

        // ��������� ����������
        isPlacing = false;
        currentFarm = null;
    }

    void CancelPlacement()
    {
        Debug.Log("������ ���������!");
        Destroy(currentFarm);
        currentFarm = null;
        isPlacing = false;
    }

    private class BuildingData
    {
        public GameObject Prefab { get; }
        public int WoodCost { get; }
        public int RockCost { get; }

        public BuildingData(GameObject prefab, int woodCost, int rockCost)
        {
            Prefab = prefab;
            WoodCost = woodCost;
            RockCost = rockCost;
        }
    }
}
