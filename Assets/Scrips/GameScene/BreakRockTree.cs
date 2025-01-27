using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class BreakRockTree : MonoBehaviour
{
    public TMP_Text NumberWood, NumberWoodUnit, NumberRock, NumberRockUnit;
    private GameObject selectedObject; // ������ �� ���������� ������

    void Update()
    {
        // ���������, ������ �� ������ ������ ����
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // ��� �� ������ � �������
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) // ���������, ����� �� ��� � ������
            {
                GameObject clickedObject = hit.collider.gameObject;

                // ���� ������ ����� ��������
                if (clickedObject.CompareTag("Selectable"))
                {
                    SelectObject(clickedObject);

                    // ���� ������� ���� ������, � ��� �������
                    if (GoUnits.selectedUnits.Count == 1 && GoUnits.selectedUnits[0].name == "Worker")
                    {
                        // �������� ��������
                        GameObject worker = GoUnits.selectedUnits[0];

                        // ������� �������� � clickedObject
                        NavMeshAgent agent = worker.GetComponent<NavMeshAgent>();
                        if (agent != null)
                        {
                            agent.SetDestination(clickedObject.transform.position);

                            // �������, ���� ������� ����� �� �������
                            StartCoroutine(WaitUntilWorkerArrives(worker, clickedObject, agent));
                            
                        }
                    }
                }
            }
        }
    }

    IEnumerator WaitUntilWorkerArrives(GameObject worker, GameObject targetObject, NavMeshAgent agent)
    {
        // �������, ���� ������� �� ����������� � ������� �� �������� ����������
        while (Vector3.Distance(worker.transform.position, targetObject.transform.position) > 5f)
        {
            yield return null; // ��� ��������� ����
        }

        // ���������, ���������� �� ������� � ������� ������
        if (worker == null || targetObject == null)
        {
            yield break; // ������� �� ��������, ���� ���� �� �������� ���������
        }

        // ����� ������� ������ ����
        agent.isStopped = true; // ������������� �������� ��������

        // ������������ �������� �� 2 �������
        yield return new WaitForSeconds(2f);

        // ��������� ������� ������ ����� �������������� ��� �����
        if (targetObject != null)
        {
            if (targetObject.name == "Forest" || targetObject.name == "Forest(Clone)")
            {
                NumberWood.text = Convert.ToString(int.Parse(NumberWood.text) + 1);
                NumberWoodUnit.text = Convert.ToString(int.Parse(NumberWoodUnit.text) + 1);
            }
            else if (targetObject.name == "Rocks" || targetObject.name == "Rocks(Clone)")
            {
                NumberRock.text = Convert.ToString(int.Parse(NumberRock.text) + 1);
                NumberRockUnit.text = Convert.ToString(int.Parse(NumberRockUnit.text) + 1);
            }

            // ���������� ������
            Destroy(targetObject);
        }

        // ��������� �������� ��������� �����
        if (agent != null)
        {
            agent.isStopped = false;
        }
    }

    IEnumerator FreezeUnitForSeconds(GameObject unit, float seconds)
    {
        // ��������� ����������� ����� ���������
        NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = true; // ������������� ��������
        }

        // ��� ��������� ���������� ������
        yield return new WaitForSeconds(seconds);

        // ���������� ����������� ���������
        if (agent != null)
        {
            agent.isStopped = false; // ��������� ��������
        }
    }
    void SelectObject(GameObject obj)
    {
        // �������� ����� ������
        selectedObject = obj;

        Debug.Log("Selected: " + obj.name);
    }
}