using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class BreakRockTree : MonoBehaviour
{
    public TMP_Text NumberWood, NumberWoodUnit, NumberRock, NumberRockUnit;
    public GameObject ZonePlayer;
    public GameObject TownHall;
    private GameObject selectedObject; // ������ �� ���������� ������
    private Transform selectionMine1 = null;
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
                if (clickedObject.CompareTag("Selectable") || clickedObject.CompareTag("Unit"))
                {
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
        Transform selectionVillager = worker.transform.Find("Villager");
       
        // ��������� ������� ������ ����� �������������� ��� �����
        if (targetObject != null)
        {
            if (targetObject.name == "Forest" || targetObject.name == "Forest(Clone)")
            {
                NumberWood.text = Convert.ToString(int.Parse(NumberWood.text) + 1);
                NumberWoodUnit.text = Convert.ToString(int.Parse(NumberWoodUnit.text) + 1);
                selectionMine1 = selectionVillager.transform.Find("wood");
                GameOver.AddResource("wood", 1);
            }
            else if (targetObject.name == "Rocks" || targetObject.name == "Rocks(Clone)")
            {
                NumberRock.text = Convert.ToString(int.Parse(NumberRock.text) + 1);
                NumberRockUnit.text = Convert.ToString(int.Parse(NumberRockUnit.text) + 1);
                selectionMine1 = selectionVillager.transform.Find("rock");
                GameOver.AddResource("rock", 1);

            }
            else if(targetObject.name == "farm_model(Clone)")
            {
                Transform selectionSprite = ZonePlayer.transform.Find("granary_model(Clone)");
                if (selectionSprite != null)
                {
                    Debug.Log("��� ���� ���� ������� �����");
                }
                else
                {
                    Debug.Log("���� �� ������ ������� ��������");
                }
            }
            selectionMine1.gameObject.SetActive(true);
            // ���������� ������
            if (targetObject.name != "farm_model(Clone)")
            {
                Destroy(targetObject);
            }
        }

        // ��������� �������� ��������� �����
        if (agent != null)
        {
            agent.isStopped = false;
        }
        // ���������� �������� � ������
        if (TownHall != null)
        {
            StartCoroutine(MoveToTownHall(worker, agent));
        }
        
    }
    IEnumerator MoveToTownHall(GameObject worker, NavMeshAgent agent)
    {
        // ������������� ���� ��� ��������
        agent.SetDestination(TownHall.transform.position);

        // �������, ���� ������� �� ����������� � ������ �� �������� ����������
        while (Vector3.Distance(worker.transform.position, TownHall.transform.position) > 5f)
        {
            yield return null; // ��� ��������� ����
        }

        // ������������� �������� ��������
        agent.isStopped = true;

        // ��������� ������ `selectionMine1` ������ ����� ��������
        if (selectionMine1 != null)
        {
            selectionMine1.gameObject.SetActive(false);
        }

        // ���������� ����������� ��������� ��������
        agent.isStopped = false;
    }
}