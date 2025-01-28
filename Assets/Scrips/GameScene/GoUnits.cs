using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GoUnits : MonoBehaviour
{
    public RectTransform selectionBoxUI; // RectTransform ��� ������������ selection box
    public Camera mainCamera;
    public Sprite Healer, Orc, Archer, Heavy, Light_infantry, Catapult, SiegeTower, Converter, Worker, Town_Center;
    public Image UnitOrBuilds;
    public TMP_Text NameUnit;
    public GameObject Panel,PanellotUnits,PanelBuidings,PanelCommand;
    public Image[] Images, ImagesHp;
    private Sprite[] UnitsObject;
    public static List<GameObject> selectedUnits = new List<GameObject>(); // ������ ���������� ������
    private Vector2 startMousePos; // ��������� ������� ����
    private Vector2 endMousePos; // �������� ������� ����
    private bool isSelecting = false; // ���� ��� ������������ ��������� ���������
    private bool isInNoSelectionZone = false; // ���� ��� ������������ ���������� � ����������� ����
    private void Start()
    {
        UnitsObject= new Sprite[] { Archer, Orc, Healer, Heavy, Light_infantry, Converter, Worker, Town_Center };
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
        //������ ��������
        foreach (Image obj in Images)
        {
            obj.gameObject.SetActive(false);
        }
        for (int i = 0; i < selectedUnits.Count; i++)
        {
            if (selectedUnits.Count == 1)
            {
                Panel.SetActive(true);
                PanellotUnits.SetActive(false);
                for (int j = 0; j < UnitsObject.Length; j++)
                {
                    if (selectedUnits[0].name == UnitsObject[j].name)
                    {
                        UnitOrBuilds.sprite = UnitsObject[j];
                        if(UnitsObject[j].name == "Town_Center")
                        {
                            NameUnit.text = "������";
                        }
                        else
                        {
                            NameUnit.text = UnitsObject[j].name;
                        }
                    }
                }
                if (gameObject.name != "Worker")
                {
                    PanelBuidings.SetActive(true);
                }
            }
            else if(selectedUnits.Count > 1)
            {
                Panel.SetActive(false);
                PanellotUnits.SetActive(true);
                
                for (int j = 0;j < selectedUnits.Count; j++)
                {
                    Images[i].gameObject.SetActive(true);
                    for (int x = 0; x < UnitsObject.Length; x++)
                    {
                        if (selectedUnits[i].name == UnitsObject[x].name)
                        {
                            Images[i].sprite = UnitsObject[x];
                            break;
                        }
                        
                    }
                }
            } 
        }
        Transform transform = PanelCommand.transform.Find("PanelAtatka");
        foreach (GameObject gameObject in selectedUnits)
        {
            if (gameObject.name != "Worker")
            {
                PanelBuidings.SetActive(false);
            }
            if(gameObject.name == "Knight"|| gameObject.name == "Archer" || gameObject.name == "Heavy" || gameObject.name == "Catapult"|| gameObject.name == "SiegeTower")
            {
                transform.gameObject.SetActive(true);
            }else
            {
                transform.gameObject.SetActive(false);
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
        DeselectAllUnits(); // ���������� ���������� ���������

        Vector2 min = Vector2.Min(startMousePos, endMousePos); // ������ ����� ���� ���������� �������
        Vector2 max = Vector2.Max(startMousePos, endMousePos); // ������� ������ ���� ���������� �������

        // ���� ��� ������� � ����� "Unit"
        foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Unit"))
        {
            Vector3 screenPos = mainCamera.WorldToScreenPoint(unit.transform.position); // ������� ������� �� ������

            // ���������, �������� �� ������ � ���������� �������
            if (screenPos.x >= min.x && screenPos.x <= max.x &&
                screenPos.y >= min.y && screenPos.y <= max.y)
            {
                SelectUnit(unit); // ��������� ������ � ������ ����������
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
                        Debug.Log(unit);
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
    public List<string> CheckNames(List<GameObject> objects)
    {
        List<string> resultNames = new List<string>();
        int status = 1; // ��������� ������ � ��� ����� ����������

        if (objects.Count == 0)
        {
            return resultNames; // ���� ������ ������, ���������� ������ ������
        }

        string firstName = objects[0].name; // ���� ��� ������� �������

        // �������� �� ���� �������� � ���������, ���������� �� �����
        foreach (GameObject obj in objects)
        {
            if (obj.name != firstName)
            {
                status = 2; // ���� ���� ���� ���� �������, ������ ������ �� 2
                break; // ���������� ��������, ��� ��� ���������� ������ �������
            }
        }

        // ���� ��� ����� ����������
        if (status == 1)
        {
            resultNames.Add(firstName); // ��������� ��� � ������
        }
        else
        {
            // ���� ����� ������, ��������� ��� ����� � ������
            foreach (var obj in objects)
            {
                resultNames.Add(obj.name);
            }
        }

        return resultNames; // ���������� ������ ���
    }
}