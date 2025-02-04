using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Attaka : MonoBehaviour
{
    public GameObject ZonePlayer, Vrags;
    public GameObject PanelPlayerUI, PanelEnd;
    public GameObject UnitPrefab;
    private float spawnInterval = 5f;
    private Dictionary<string, bool> hasAttackedDict = new Dictionary<string, bool>();

    void Start()
    {
        InitializeAttackDictionary();
        StartCoroutine(SpawnUnits());
    }

    void InitializeAttackDictionary()
    {
        foreach (Transform vrag in Vrags.transform)
        {
            if (vrag.name.StartsWith("Vrag_"))
            {
                hasAttackedDict[vrag.name] = false; // Добавляем все базы в словарь
            }
        }
    }

    void Update()
    {
        CheckAndAttackTargets();
    }

    void CheckAndAttackTargets()
    {
        foreach (Transform vragBase in Vrags.transform)
        {
            if (!vragBase.name.StartsWith("Vrag_")) continue;

            // Проверяем, есть ли база в словаре
            if (!hasAttackedDict.ContainsKey(vragBase.name))
            {
                Debug.LogWarning($"База {vragBase.name} не найдена в словаре. Добавляем её.");
                hasAttackedDict[vragBase.name] = false; // Добавляем базу, если её нет
            }

            List<GameObject> units = GetValidUnits(vragBase);
            if (units.Count >= 5 && !hasAttackedDict[vragBase.name])
            {
                GameObject target = SelectRandomTarget(vragBase.gameObject);
                if (target != null)
                {
                    StartAttack(vragBase.gameObject, units, target);
                    hasAttackedDict[vragBase.name] = true;
                }
            }
        }
    }

    List<GameObject> GetValidUnits(Transform baseTransform)
    {
        List<GameObject> units = new List<GameObject>();
        foreach (Transform child in baseTransform)
        {
            if (child.CompareTag("UnitVrag") && child.gameObject.activeSelf)
            {
                units.Add(child.gameObject);
            }
        }
        return units;
    }

    GameObject SelectRandomTarget(GameObject attacker)
    {
        List<GameObject> possibleTargets = new List<GameObject> { ZonePlayer };

        foreach (Transform vrag in Vrags.transform)
        {
            if (vrag.gameObject != attacker && vrag.gameObject.activeSelf)
            {
                possibleTargets.Add(vrag.gameObject);
            }
        }

        return possibleTargets.Count > 0 ?
            possibleTargets[Random.Range(0, possibleTargets.Count)] :
            null;
    }

    void StartAttack(GameObject attacker, List<GameObject> units, GameObject target)
    {
        Debug.Log($"{attacker.name} атакует {target.name}");

        foreach (GameObject unit in units)
        {
            if (unit == null) continue;

            NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();
            if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
            {
                agent.SetDestination(target.transform.position);
                StartCoroutine(CheckForObstacles(agent, target.transform.position));
            }
        }

        StartCoroutine(ResetAttackFlag(attacker.name));
    }

    IEnumerator ResetAttackFlag(string baseName)
    {
        yield return new WaitForSeconds(60f); // Задержка между атаками
        hasAttackedDict[baseName] = false;
    }

    IEnumerator SpawnUnits()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            foreach (Transform vragBase in Vrags.transform)
            {
                if (!vragBase.name.StartsWith("Vrag_") || !vragBase.gameObject.activeSelf)
                    continue;

                Vector3 spawnPosition = vragBase.position + new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
                GameObject newUnit = Instantiate(UnitPrefab, spawnPosition, Quaternion.identity);
                newUnit.transform.SetParent(vragBase);
                newUnit.tag = "UnitVrag";

                NavMeshAgent agent = newUnit.GetComponent<NavMeshAgent>();
                if (agent == null || !agent.isOnNavMesh)
                {
                    Debug.LogWarning("Юнит не может перемещаться. Уничтожаем: " + newUnit.name);
                    Destroy(newUnit);
                }
            }
        }
    }

    IEnumerator CheckForObstacles(NavMeshAgent agent, Vector3 destination)
    {
        if (agent == null) yield break;

        while (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh && agent.pathPending)
        {
            yield return null;
        }

        while (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh && agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            if (agent.pathStatus != NavMeshPathStatus.PathComplete)
            {
                agent.ResetPath();
                agent.SetDestination(destination);
            }
        }
    }
}