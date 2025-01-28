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
    private GameObject currentFarm; // Текущий перемещаемый объект
    private bool isPlacing = false; // Флаг для отслеживания режима постройки

    void Update()
    {
        // Обновляем ресурсы
        WoodCount = int.Parse(WoodMain.text);
        CropCount = int.Parse(CropMain.text);
        RockCount = int.Parse(RockMain.text);
        FoodCount = int.Parse(FoodMain.text);

        // Делаем доступной кнопку фермы
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

        // Делаем доступной кнопку амбара
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

        // Логика размещения фермы
        if (isPlacing && currentFarm != null)
        {
            MoveBuildingWithCursor();
            
            if (Input.GetMouseButtonDown(0)) // Если нажали ЛКМ
            {
                if (CanPlaceHere(currentFarm.transform.position))
                {
                    PlaceFarm();
                }
                else
                {
                    Debug.Log("Нельзя разместить объект здесь!"); // Место занято
                }
            }

            if (Input.GetMouseButtonDown(1)) // Отмена постройки (ПКМ)
            {
                CancelPlacement();
            }
        }
    }

    public void BuildFarm()
    {
        if (WoodCount < 5 || RockCount < 5)
        {
            Debug.Log("Недостаточно ресурсов для постройки фермы БОЛЬШЕ работай или в Купер!!!");
            return;
        }

        if (isPlacing) return; // Если уже размещаем, ничего не делаем

        // Вычитаем ресурсы
        WoodCount -= 5;
        RockCount -= 5;
        WoodMain.text = WoodCount.ToString();
        RockMain.text = RockCount.ToString();

        // Начинаем размещение фермы
        currentFarm = Instantiate(Farm3d);
        isPlacing = true;
    }
    public void BuildGranary()
    {
        if (WoodCount < 7 || RockCount < 7)
        {
            Debug.Log("Недостаточно ресурсов для постройки мельницы БОЛЬШЕ работай или в Купер!!!");
            return;
        }

        if (isPlacing) return; // Если уже размещаем, ничего не делаем

        // Вычитаем ресурсы
        WoodCount -= 7;
        RockCount -= 7;
        WoodMain.text = WoodCount.ToString();
        RockMain.text = RockCount.ToString();

        // Начинаем размещение фермы
        currentFarm = Instantiate(Granary3d);
        isPlacing = true;
    }
    void MoveBuildingWithCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject == plane) // Проверяем, попали ли в плоскость
            {
                Vector3 newPosition = hit.point; // Получаем точку на плоскости
                currentFarm.transform.position = newPosition;

                // Проверяем возможность размещения фермы
                Transform selectionSprite = currentFarm.transform.Find("SelectionSprite");
                if (selectionSprite != null)
                {
                    if (CanPlaceHere(newPosition))
                    {
                        // Если можно поставить, устанавливаем зеленый цвет
                        selectionSprite.GetComponent<Renderer>().material.color = Color.green;
                    }
                    else
                    {
                        // Если нельзя поставить, устанавливаем красный цвет
                        selectionSprite.GetComponent<Renderer>().material.color = Color.red;
                    }
                }
            }
        }
    }

    bool CanPlaceHere(Vector3 position)
    {
        float radius = 1f; // Радиус проверки
        Collider[] colliders = Physics.OverlapSphere(position, radius);

        foreach (Collider col in colliders)
        {
            if (col.gameObject != plane && col.gameObject != currentFarm) // Игнорируем плоскость и текущий объект
            {
                return false; // Место занято
            }
        }

        return true; // Место свободно
    }


    void PlaceFarm()
    {
        Debug.Log("Объект построен!");

        // Устанавливаем белый цвет и скрываем SelectionSprite
        Transform selectionSprite = currentFarm.transform.Find("SelectionSprite");
        if (selectionSprite != null)
        {
            selectionSprite.GetComponent<Renderer>().material.color = Color.white; // Устанавливаем белый цвет
            selectionSprite.gameObject.SetActive(false); // Скрываем объект
        }

        // Добавляем объект в контейнер
        currentFarm.transform.SetParent(BuildingContainer.transform);

        // Обновляем NavMeshSurface
        NavMeshSurface[] surfaces = FindObjectsOfType<NavMeshSurface>();
        foreach (var surface in surfaces)
        {
            surface.BuildNavMesh();
        }

        // Завершаем размещение
        isPlacing = false;
        currentFarm = null;
    }


    void CancelPlacement()
    {
        Debug.Log("Отмена постройки!");
        Destroy(currentFarm);
        currentFarm = null;
        isPlacing = false;
    }
}
