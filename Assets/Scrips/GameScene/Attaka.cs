using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Attaka : MonoBehaviour
{
    public GameObject ZonePlayer, Vrags;
    public GameObject PanelPlayerUI, PanelEnd;
    public GameObject UnitPrefab;
    private float spawnInterval = 15f;
    // Словарь для отслеживания, атаковала ли база
    private Dictionary<string, bool> hasAttackedDict = new Dictionary<string, bool>();

    void Start()
    {
        InitializeAttackDictionary();

        // Запускаем корутину для создания юнитов
        StartCoroutine(SpawnUnits());
    }

    void InitializeAttackDictionary()
    {
        // Проходим по всем дочерним объектам Vrags
        foreach (Transform vrag in Vrags.transform)
        {
            // Если имя объекта начинается с "Vrag_", добавляем его в словарь
            if (vrag.name.StartsWith("Vrag_"))
            {
                hasAttackedDict[vrag.name] = false; // Устанавливаем флаг атаки в false
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

            // Получаем список активных юнитов на базе
            List<GameObject> units = GetValidUnits(vragBase);

            if (units.Count >= 5 && !hasAttackedDict[vragBase.name])
            {
                // Выбираем случайную цель для атаки
                GameObject target = SelectRandomTarget(vragBase.gameObject);
                if (target != null)
                {
                    // Начинаем атаку
                    StartAttack(vragBase.gameObject, units, target);
                    hasAttackedDict[vragBase.name] = true; // Устанавливаем флаг атаки в true
                }
            }
        }
    }

    List<GameObject> GetValidUnits(Transform baseTransform)
    {
        List<GameObject> units = new List<GameObject>();

        foreach (Transform child in baseTransform)
        {
            // Если объект является вражеским юнитом и активен, добавляем его в список
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
            // Если объект не является атакующим и активен, добавляем его в список целей
            if (vrag.gameObject != attacker && vrag.gameObject.activeSelf)
            {
                possibleTargets.Add(vrag.gameObject);
            }
        }

        // Возвращаем случайную цель из списка
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
                // Устанавливаем цель для юнита
                agent.SetDestination(target.transform.position);

                // Запускаем корутину для проверки препятствий
                StartCoroutine(CheckForObstacles(agent, target.transform.position));
            }
        }

        // Запускаем корутину для сброса флага атаки
        StartCoroutine(ResetAttackFlag(attacker.name));
    }

    IEnumerator ResetAttackFlag(string baseName)
    {
        // Ждем 60 секунд перед сбросом флага атаки
        yield return new WaitForSeconds(60f);
        hasAttackedDict[baseName] = false;
    }

    IEnumerator SpawnUnits()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            foreach (Transform vragBase in Vrags.transform)
            {
                // Если объект не является вражеской базой или не активен, пропускаем его
                if (!vragBase.name.StartsWith("Vrag_") || !vragBase.gameObject.activeSelf)
                    continue;

                // Выбираем случайную позицию для создания юнита
                Vector3 spawnPosition = vragBase.position + new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));

                // Создаем новый юнит
                GameObject newUnit = Instantiate(UnitPrefab, spawnPosition, Quaternion.identity);
                newUnit.transform.SetParent(vragBase);
                newUnit.tag = "UnitVrag";

                NavMeshAgent agent = newUnit.GetComponent<NavMeshAgent>();
                if (agent == null || !agent.isOnNavMesh)
                {
                    // Если юнит не может перемещаться, уничтожаем его
                    Debug.LogWarning("Юнит не может перемещаться. Уничтожаем: " + newUnit.name);
                    Destroy(newUnit);
                }
            }
        }
    }

    IEnumerator CheckForObstacles(NavMeshAgent agent, Vector3 destination)
    {
        // Если агент не существует, выходим из корутины
        if (agent == null) yield break;

        // Ждем, пока агент не завершит расчет пути
        while (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh && agent.pathPending)
        {
            yield return null;
        }

        // Ждем, пока агент не достигнет цели
        while (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh && agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        // Если путь не завершен, сбрасываем путь и устанавливаем цель снова
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
