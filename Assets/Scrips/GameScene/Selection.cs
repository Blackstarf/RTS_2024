using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{
    public RectTransform selectionBoxUI; // RectTransform для визуализации selection box выделяемая картинка
    public Camera mainCamera;

    public List<GameObject> selectedBuldings = new List<GameObject>(); // Список выделенных юнитов
    private Vector2 startMousePos; // Начальная позиция мыши
    private Vector2 endMousePos; // Конечная позиция мыши
    private bool isSelecting = false; // Флаг для отслеживания состояния выделения

    void Update()
    {
        // Проверка на правый клик мыши
        if (Input.GetMouseButtonDown(1)) // 1 - это правый клик
        {
            HandleRightClick();
        }

        if (Input.GetMouseButtonDown(0)) // Начало выделения (левая кнопка)
        {
            startMousePos = Input.mousePosition;
            isSelecting = true;
        }

        if (Input.GetMouseButtonUp(0)) // Завершение выделения (левая кнопка)
        {
            endMousePos = Input.mousePosition;
            isSelecting = false;
            UpdateSelectionBox(); // Обновить размер selection box
            SelectBuildingInBox(); // Выделить здания в области
        }

        if (isSelecting)
        {
            endMousePos = Input.mousePosition;
            UpdateSelectionBox();
        }
    }

    void HandleRightClick()
    {
        // Перевести экранные координаты в мир
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Проверяем, попали ли в объект с тегом "Unit"
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.CompareTag("UnitVragBase") || hitObject.CompareTag("BasePlayer"))
            {
                if (!selectedBuldings.Contains(hitObject)) // Если объект ещё не выбран
                {
                    SelectBuild(hitObject);
                }
                else
                {
                    DeselectUnit(hitObject); // Если объект уже выбран, снимаем выделение
                }
            }
        }
    }

    void UpdateSelectionBox()
    {
        float width = endMousePos.x - startMousePos.x;
        float height = endMousePos.y - startMousePos.y;

        // Установите размер selectionBox
        selectionBoxUI.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));

        // Преобразуйте экранные координаты в локальные
        Vector2 localStart;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(selectionBoxUI.parent.GetComponent<RectTransform>(), startMousePos, null, out localStart);

        // Установите позицию selectionBox в центр между startMousePos и endMousePos
        selectionBoxUI.anchoredPosition = localStart + new Vector2(width / 2, height / 2);
    }

    void SelectBuildingInBox()
    {
        DeselectAllUnits();

        Vector2 min = Vector2.Min(startMousePos, endMousePos);
        Vector2 max = Vector2.Max(startMousePos, endMousePos);

        foreach (GameObject unit in GameObject.FindGameObjectsWithTag("UnitVragBase"))
        {
            Vector3 screenPos = mainCamera.WorldToScreenPoint(unit.transform.position);

            if (screenPos.x >= min.x && screenPos.x <= max.x &&
                screenPos.y >= min.y && screenPos.y <= max.y)
            {
                SelectBuild(unit);
            }
        }
        foreach (GameObject unit in GameObject.FindGameObjectsWithTag("BasePlayer"))
        {
            Vector3 screenPos = mainCamera.WorldToScreenPoint(unit.transform.position);

            if (screenPos.x >= min.x && screenPos.x <= max.x &&
                screenPos.y >= min.y && screenPos.y <= max.y)
            {
                SelectBuild(unit);
            }
        }
    }

    void SelectBuild(GameObject unit)
    {
        selectedBuldings.Add(unit);
        Transform selectionSprite = unit.transform.Find("SelectionSprite");
        if (selectionSprite != null)
        {
            selectionSprite.gameObject.SetActive(true);
        }
    }

    void DeselectAllUnits()
    {
        foreach (GameObject unit in selectedBuldings)
        {
            Transform selectionSprite = unit.transform.Find("SelectionSprite");
            if (selectionSprite != null)
            {
                selectionSprite.gameObject.SetActive(false);
            }
        }
        selectedBuldings.Clear();
    }

    void DeselectUnit(GameObject unit)
    {
        selectedBuldings.Remove(unit);
        Transform selectionSprite = unit.transform.Find("SelectionSprite");
        if (selectionSprite != null)
        {
            selectionSprite.gameObject.SetActive(false);
        }
    }
}
