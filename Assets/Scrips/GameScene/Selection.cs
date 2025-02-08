using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{
    public RectTransform selectionBoxUI; // RectTransform ��� ������������ selection box ���������� ��������
    public Camera mainCamera;

    public List<GameObject> selectedBuldings = new List<GameObject>(); // ������ ���������� ������
    private Vector2 startMousePos; // ��������� ������� ����
    private Vector2 endMousePos; // �������� ������� ����
    private bool isSelecting = false; // ���� ��� ������������ ��������� ���������

    void Update()
    {
        // �������� �� ������ ���� ����
        if (Input.GetMouseButtonDown(1)) // 1 - ��� ������ ����
        {
            HandleRightClick();
        }

        if (Input.GetMouseButtonDown(0)) // ������ ��������� (����� ������)
        {
            startMousePos = Input.mousePosition;
            isSelecting = true;
        }

        if (Input.GetMouseButtonUp(0)) // ���������� ��������� (����� ������)
        {
            endMousePos = Input.mousePosition;
            isSelecting = false;
            UpdateSelectionBox(); // �������� ������ selection box
            SelectBuildingInBox(); // �������� ������ � �������
        }

        if (isSelecting)
        {
            endMousePos = Input.mousePosition;
            UpdateSelectionBox();
        }
    }

    void HandleRightClick()
    {
        // ��������� �������� ���������� � ���
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // ���������, ������ �� � ������ � ����� "Unit"
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.CompareTag("UnitVragBase") || hitObject.CompareTag("BasePlayer"))
            {
                if (!selectedBuldings.Contains(hitObject)) // ���� ������ ��� �� ������
                {
                    SelectBuild(hitObject);
                }
                else
                {
                    DeselectUnit(hitObject); // ���� ������ ��� ������, ������� ���������
                }
            }
        }
    }

    void UpdateSelectionBox()
    {
        float width = endMousePos.x - startMousePos.x;
        float height = endMousePos.y - startMousePos.y;

        // ���������� ������ selectionBox
        selectionBoxUI.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));

        // ������������ �������� ���������� � ���������
        Vector2 localStart;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(selectionBoxUI.parent.GetComponent<RectTransform>(), startMousePos, null, out localStart);

        // ���������� ������� selectionBox � ����� ����� startMousePos � endMousePos
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
