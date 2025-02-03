using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Attaka : MonoBehaviour
{
    public GameObject ZonePlayer, Vrags; // ���� ���� �����
    public GameObject PanelPlayerUI,PanelEnd;
    public GameObject UnitPrefab; // ������ �����
    private float spawnInterval = 15f; // �������� ������ (��� ������� �������)
    private bool hasAttacked = false;

    void Start()
    {
        StartCoroutine(SpawnUnits());
    }

    void Update()
    {
        CheckAndAttackZonePlayer(); // ��������� ����� �� ������ �����
    }

    void CheckAndAttackZonePlayer()
    {
        Transform vrag1Transform = Vrags.transform.Find("Vrag_1");
        if (vrag1Transform == null) return;

        List<GameObject> unitsUnderVrag1 = new List<GameObject>();
        foreach (Transform child in vrag1Transform)
        {
            GameObject unit = child.gameObject;
            if (unit.CompareTag("UnitVrag") && unit.name != "Worker")
            {
                unitsUnderVrag1.Add(unit);
            }
        }

        // ����� ������ ���������� (5 ��� ������), �������� �����
        if (unitsUnderVrag1.Count >= 5 && !hasAttacked)
        {
            Debug.Log("Vrag_1 is attacking ZonePlayer");
            hasAttacked = true;

            // ���� ���� ������ ���� � ���� ������
            foreach (GameObject unit in unitsUnderVrag1)
            {
                if (unit == null) continue;

                NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();
                if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
                {
                    agent.SetDestination(ZonePlayer.transform.position);
                    StartCoroutine(CheckForObstacles(agent, ZonePlayer.transform.position));
                }
            }
        }
    }

    IEnumerator SpawnUnits()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            Transform vrag1Transform = Vrags.transform.Find("Vrag_1");
            if (vrag1Transform == null) continue;

            Vector3 spawnPosition = vrag1Transform.position + new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
            GameObject newUnit = Instantiate(UnitPrefab, spawnPosition, Quaternion.identity);
            newUnit.transform.SetParent(vrag1Transform);
            newUnit.tag = "UnitVrag";

            // �� ���� ������ ����� ���� � ����� ����� ����� ������
            NavMeshAgent agent = newUnit.GetComponent<NavMeshAgent>();
            if (agent != null && agent.isOnNavMesh)
            {
            }
            else
            {
                Debug.LogWarning("Spawned unit is not on NavMesh and cannot move. Destroying unit: " + newUnit.name);
                Destroy(newUnit); // �������, ���� �� ����� ������
            }

            // ����� ������ ����� ��������� ������ � ���������, ����� �� ���������
            CheckAndAttackZonePlayer();
        }
    }

    IEnumerator CheckForObstacles(NavMeshAgent agent, Vector3 destination)
    {
        // ���� ����� ��� ��� ���������, �� ��������� ��������
        if (agent == null || agent.gameObject == null)
        {
            yield break;
        }

        while (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh && agent.pathPending)
        {
            yield return null;
        }

        while (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh && agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        // ��������� �������� ����� ������� � �������
        if (agent == null || agent.gameObject == null || !agent.isOnNavMesh || !agent.isActiveAndEnabled)
        {
            yield break;
        }

        if (agent.pathStatus == NavMeshPathStatus.PathInvalid || agent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            Debug.LogWarning("Agent encountered an obstacle, resetting path for unit: " + agent.gameObject.name);
            agent.ResetPath();
            agent.SetDestination(destination);
        }
    }
}
