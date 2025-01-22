using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoUnits : MonoBehaviour
{
    private Camera mainCamera;
    private List<GameObject> selectedObjects = new List<GameObject>(); // ������ ���������� ��������

    private Vector2 startMousePos; // ��������� ����� ��� ��������� ������
    private bool isSelecting = false;

    public Texture2D selectionTexture; // �������� ����� ���������

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        HandleSelectionArea(); // ��������� ��������� ������
        HandleMovement(); // ����������� ��������
    }
    void HandleSelectionArea()
    {
        if (Input.GetMouseButtonDown(0)) // ������ ��������� ������
        {
            isSelecting = true;
            startMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0)) // ���������� ��������� ������
        {
            isSelecting = false;
            Debug.Log("�������� SelectObjectsInArea"); // �������� ������
            SelectObjectsInArea(); // ����� ������
        }
    }
    void SelectObjectsInArea()
    {
        Debug.Log("SelectObjectsInArea ������."); // ��������� ����� ������
        Vector2 endMousePos = Input.mousePosition;

        // ���� ������� ��������� ����� ���������, ��������� ��������� ���������
        if (Vector2.Distance(startMousePos, endMousePos) < 10f)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;

                // ���� ������ ��� �������, ������� ���������
                if (selectedObjects.Contains(clickedObject))
                {
                    selectedObjects.Remove(clickedObject);
                    Debug.Log($"����� ��������� � �������: {clickedObject.name}");
                }
                else
                {
                    selectedObjects.Add(clickedObject);
                    Debug.Log($"������� ������: {clickedObject.name}");
                }
            }
            return;
        }

        // ��������� ��������� � �������
        Rect selectionRect = new Rect(
            Mathf.Min(startMousePos.x, endMousePos.x),
            Mathf.Min(startMousePos.y, endMousePos.y),
            Mathf.Abs(startMousePos.x - endMousePos.x),
            Mathf.Abs(startMousePos.y - endMousePos.y)
        );

        // ��������� ������� � ����������� NavMeshAgent
        foreach (NavMeshAgent agent in FindObjectsOfType<NavMeshAgent>())
        {
            Vector3 screenPos = mainCamera.WorldToScreenPoint(agent.transform.position);
            if (selectionRect.Contains(screenPos) && !selectedObjects.Contains(agent.gameObject))
            {
                selectedObjects.Add(agent.gameObject);
                Debug.Log($"������� ������ � �����: {agent.gameObject.name}");
            }
        }

        // ��� ���� ���������� ��������
        if (selectedObjects.Count > 0)
        {
            Debug.Log("���������� �������: " + string.Join(", ", selectedObjects.ConvertAll(obj => obj.name)));
        }
        else
        {
            Debug.Log("��� ���������� ��������.");
        }
    }
    // ����������� ���������� ��������
    void HandleMovement()
    {
        if (Input.GetMouseButtonDown(0) && selectedObjects.Count > 0) // ���
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 targetPoint = hit.point;

                // ������������� ����� ���������� ��� ���� ���������� ��������
                foreach (GameObject obj in selectedObjects)
                {
                    NavMeshAgent agent = obj.GetComponent<NavMeshAgent>();
                    if (agent != null)
                    {
                        agent.SetDestination(targetPoint);
                    }
                }
            }
        }
    }

    void OnGUI()
    {
        if (isSelecting)
        {
            // ������ ������������� ���������
            Rect selectionRect = new Rect(
                Mathf.Min(startMousePos.x, Input.mousePosition.x),
                Screen.height - Mathf.Max(startMousePos.y, Input.mousePosition.y),
                Mathf.Abs(startMousePos.x - Input.mousePosition.x),
                Mathf.Abs(startMousePos.y - Input.mousePosition.y)
            );

            // ���������� ��������
            GUI.DrawTexture(selectionRect, selectionTexture);
        }
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