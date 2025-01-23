using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GoUnits : MonoBehaviour
{
    public RectTransform selectionBoxUI; // RectTransform ��� ������������ selection box
    public Camera mainCamera;

    private List<GameObject> selectedUnits = new List<GameObject>(); // ������ ���������� ������
    private Vector2 startMousePos; // ��������� ������� ����
    private Vector2 endMousePos; // �������� ������� ����
    private bool isSelecting = false; // ���� ��� ������������ ��������� ���������
    private bool isInNoSelectionZone = false; // ���� ��� ������������ ���������� � ����������� ����
    private void Start()
    {
        selectionBoxUI.gameObject.SetActive(false);
    }
    void Update()
    {
        if (!isSelecting)
        {
            if (IsPointerOverNoSelectionZone())
            {
                isInNoSelectionZone = true;
                return;
            }
            else
            {
                isInNoSelectionZone = false;
            }
        }

        HandleSelection();
        HandleMovement();
    }

    void HandleSelection()
    {
        if (Input.GetMouseButtonDown(0) && !isInNoSelectionZone)
        {
            startMousePos = Input.mousePosition;
            isSelecting = true;
            selectionBoxUI.gameObject.SetActive(true);
            selectionBoxUI.sizeDelta = Vector2.zero; // ����� �������
            selectionBoxUI.anchoredPosition = startMousePos; // ���������� ��������� �������
        }

        if (Input.GetMouseButton(0) && isSelecting)
        {
            endMousePos = Input.mousePosition;
            UpdateSelectionBox();
        }

        if (Input.GetMouseButtonUp(0) && isSelecting)
        {
            isSelecting = false;
            selectionBoxUI.gameObject.SetActive(false);
            SelectUnitsInBox();
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

    void SelectUnitsInBox()
    {
        DeselectAllUnits();

        Vector2 min = Vector2.Min(startMousePos, endMousePos);
        Vector2 max = Vector2.Max(startMousePos, endMousePos);

        foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Unit"))
        {
            Vector3 screenPos = mainCamera.WorldToScreenPoint(unit.transform.position);

            if (screenPos.x >= min.x && screenPos.x <= max.x &&
                screenPos.y >= min.y && screenPos.y <= max.y)
            {
                SelectUnit(unit);
            }
        }
    }

    void HandleMovement()
    {
        if (selectedUnits.Count > 0 && Input.GetMouseButtonDown(1))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                foreach (GameObject unit in selectedUnits)
                {
                    NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();
                    if (agent != null)
                    {
                        agent.SetDestination(hit.point);
                    }
                }
            }
        }
    }

    void SelectUnit(GameObject unit)
    {
        selectedUnits.Add(unit);
        Transform selectionSprite = unit.transform.Find("SelectionSprite");
        if (selectionSprite != null)
        {
            selectionSprite.gameObject.SetActive(true);
        }
    }

    void DeselectAllUnits()
    {
        foreach (GameObject unit in selectedUnits)
        {
            Transform selectionSprite = unit.transform.Find("SelectionSprite");
            if (selectionSprite != null)
            {
                selectionSprite.gameObject.SetActive(false);
            }
        }
        selectedUnits.Clear();
    }

    /// <summary>
    /// ���������, ��������� �� ��������� ��� �����, ��� ��������� ���������.
    /// </summary>
    /// <returns>True, ���� ��������� ��������� ��� �����, ��� ��������� ���������, ����� false.</returns>
    private bool IsPointerOverNoSelectionZone()
    {
        PointerEventData eventDataCurrentPos = new PointerEventData(EventSystem.current);
        eventDataCurrentPos.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPos, results);
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("NoSelectionZone"))
            {
                return true;
            }
        }
        return false;
    }
}

//private Camera MainCamera;
//private NavMeshAgent Agent;

//// Start is called before the first frame update
//void Start()
//{
//    MainCamera = Camera.main;
//    Agent = GetComponent<NavMeshAgent>();
//}

//// Update is called once per frame
//void Update()
//{
//    if (Input.GetMouseButtonDown(0))
//    {
//        RaycastHit hit;
//        if (Physics.Raycast(MainCamera.ScreenPointToRay(Input.mousePosition), out hit))
//        {
//            Agent.SetDestination(hit.point);
//        }
//    }

//}