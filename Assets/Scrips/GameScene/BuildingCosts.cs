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

    void Update()
    {
        // ��������� �������
        WoodCount = int.Parse(WoodMain.text);
        CropCount = int.Parse(CropMain.text);
        RockCount = int.Parse(RockMain.text);
        FoodCount = int.Parse(FoodMain.text);

        // ������ ��������� ������ �����
        if (WoodCount >= 5 && RockCount >= 5)
        {
            Transform selectionSprite = Farm.transform.Find("PanelNo");
            selectionSprite.gameObject.SetActive(false);
        }
        else
        {
            Transform selectionSprite = Farm.transform.Find("PanelNo");
            selectionSprite.gameObject.SetActive(true);
        }

        // ������ ��������� ������ ������
        if (WoodCount >= 7 && RockCount >= 7)
        {
            Transform selectionSprite = Granary.transform.Find("PanelNo");
            selectionSprite.gameObject.SetActive(false);
        }
        else
        {
            Transform selectionSprite = Granary.transform.Find("PanelNo");
            selectionSprite.gameObject.SetActive(true);
        }

        // ������ ���������� �����
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

    public void BuildFarm()
    {
        if (WoodCount < 5 || RockCount < 5)
        {
            Debug.Log("������������ �������� ��� ��������� ����� ������ ������� ��� � �����!!!");
            return;
        }

        if (isPlacing) return; // ���� ��� ���������, ������ �� ������

        // �������� �������
        WoodCount -= 5;
        RockCount -= 5;
        WoodMain.text = WoodCount.ToString();
        RockMain.text = RockCount.ToString();

        // �������� ���������� �����
        currentFarm = Instantiate(Farm3d);
        isPlacing = true;
    }
    public void BuildGranary()
    {
        if (WoodCount < 7 || RockCount < 7)
        {
            Debug.Log("������������ �������� ��� ��������� �������� ������ ������� ��� � �����!!!");
            return;
        }

        if (isPlacing) return; // ���� ��� ���������, ������ �� ������

        // �������� �������
        WoodCount -= 7;
        RockCount -= 7;
        WoodMain.text = WoodCount.ToString();
        RockMain.text = RockCount.ToString();

        // �������� ���������� �����
        currentFarm = Instantiate(Granary3d);
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
}
