using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;

public class GoUnits : MonoBehaviour
{
    private Camera MainCamera;
    private NavMeshAgent Agent;
    public Material selectedMaterial;
    private GameObject selectedObject; // ������� ���������� ������
    private Material originalMaterial;
    public GameObject plane;

    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main;
        Agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        HandleSelection();
        HandleMovement();
    }

    void HandleSelection()
    {
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftControl)) // ��������� ������� ��� + Ctrl
        {
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;

                // ���������, ���� ������ ��� �������
                if (selectedObject != null)
                {
                    // ���� �������� �� ��� ����������� �������, ������� ���������
                    if (selectedObject == clickedObject)
                    {
                        DeselectObject(selectedObject);
                        selectedObject = null;
                        return;
                    }
                    else
                    {
                        // ������� ��������� � ����������� �������
                        DeselectObject(selectedObject);
                    }
                }

                // �������� ����� ������
                selectedObject = clickedObject;
                SelectObject(selectedObject);
            }
        }
    }

    void SelectObject(GameObject obj)
    {
        // ��������� ������������ ��������
        originalMaterial = obj.GetComponent<Renderer>().material;

        // ��������� NavMeshAgent, ���� ��� ���
        if (obj.GetComponent<NavMeshAgent>() == null)
        {
            obj.AddComponent<NavMeshAgent>();
        }
    }

    void DeselectObject(GameObject obj)
    {
        // ���������� ������������ ��������
        if (originalMaterial != null)
        {
            obj.GetComponent<Renderer>().material = originalMaterial;
        }
    }

    void HandleMovement()
    {
        if (Input.GetMouseButtonDown(0) && selectedObject != null) // ����������� ������ ��� ����������� �������
        {
            RaycastHit hit;
            if (Physics.Raycast(MainCamera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Agent.SetDestination(hit.point); // ������������� ���� ��� NavMeshAgent
            }
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

