using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using System.Collections;
using System.Linq;

public class ButtonAttack : MonoBehaviour
{
    public GameObject ZonePlayer, Vrags;
    private GameObject[] attackUnits;
    private Selection arrayselecion;
    string NameBaseVrag;
    private void Start()
    {
        arrayselecion= FindObjectOfType<Selection>();
    }
    public void Attack()
    {

        GameObject firstUnit = arrayselecion.selectedBuldings.FirstOrDefault(building => building.CompareTag("UnitVragBase"));

        if (firstUnit != null)
        {
            NameBaseVrag = firstUnit.transform.parent.name;
        }
        else
        {
            Debug.LogWarning("Вражеская база не выбрана! Атака отменена.");
            return;
        }
        Debug.Log("Атака началась!");

        List<GameObject> unitsList = new List<GameObject>();
        string[] unitNames = { "Siege_Tower", "Light_infantry", "Heavy", "Catapult", "Archer" };

        foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Unit"))
        {
            foreach (string name in unitNames)
            {
                if (unit.name.Contains(name))
                {
                    unitsList.Add(unit);
                    Transform sprite = unit.transform.Find("SelectionSprite");
                    if (sprite != null) sprite.gameObject.SetActive(true);
                }
            }
        }

        attackUnits = unitsList.ToArray();
        if (attackUnits.Length == 0)
        {
            Debug.Log("Нет доступных юнитов!");
            return;
        }

        GameObject target = FindPriorityTarget();
        if (target != null)
        {
            StartCoroutine(AttackSequence(target));
        }
    }


    GameObject FindPriorityTarget()
    {
        if (Vrags == null)
        {
            Debug.LogError("Объект Vrags не назначен в инспекторе!");
            return null;
        }

        Transform vrag1 = Vrags.transform.Find(NameBaseVrag);

        // Если враг уже уничтожен — отправляем юнитов на ZonePlayer
        if (vrag1 == null)
        {
            Debug.Log("Вражеская база уничтожена! Юниты направляются на базу игрока.");
            MoveUnitsToBase();
            return null;
        }

        // Поиск врагов внутри Vrag_1
        List<GameObject> enemies = new List<GameObject>();
        foreach (Transform child in vrag1)
        {
            if (child.CompareTag("UnitVrag")) enemies.Add(child.gameObject);
        }

        if (enemies.Count > 0)
        {
            return GetClosestTarget(enemies);
        }

        // Если юнитов нет - ищем Town_Center
        Transform baseTransform = vrag1.Find("Town_Center");

        if (baseTransform == null || !baseTransform.gameObject.activeSelf)
        {
            Debug.Log("Вражеская база уничтожена! Переключаемся на ZonePlayer.");
            MoveUnitsToBase();
            return null;
        }
        return baseTransform.gameObject;
    }
     GameObject GetClosestTarget(List<GameObject> targets)
    {
        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        targets = targets.FindAll(target => target != null && target.activeSelf);

        if (targets.Count == 0)
        {
            Debug.LogWarning("Все цели уничтожены!");
            return null;
        }

        GameObject firstUnit = attackUnits[0];
        if (firstUnit == null || !firstUnit.activeSelf)
        {
            Debug.LogWarning("Первый атакующий юнит уничтожен!");
            return null;
        }

        foreach (GameObject target in targets)
        {
            if (target == null || !target.activeSelf) continue;

            float distance = Vector3.Distance(
                firstUnit.transform.position,
                target.transform.position
            );

            if (distance < minDistance)
            {
                minDistance = distance;
                closest = target;
            }
        }

        return closest;
    }

    IEnumerator AttackSequence(GameObject target)
    {
        if (target == null || !target.activeSelf)
        {
            Debug.LogWarning("Цель уничтожена до начала атаки!");
            yield break;
        }

        Debug.Log($"Цель атаки: {target.name}");

        while (target != null && target.activeSelf)
        {
            attackUnits = System.Array.FindAll(attackUnits, unit => unit != null && unit.activeSelf);

            if (attackUnits.Length == 0)
            {
                Debug.LogWarning("Все атакующие юниты уничтожены! Атака отменена.");
                yield break;
            }

            foreach (GameObject unit in attackUnits)
            {
                if (unit == null || !unit.activeSelf) continue;

                NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();
                UnitHpAndCommand controller = unit.GetComponent<UnitHpAndCommand>();

                if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
                {
                    agent.SetDestination(target.transform.position);
                    if (controller != null) controller.SetAttackTarget(target);
                }
            }

            yield return new WaitForSeconds(1f);

            if (target == null || !target.activeSelf)
            {
                Debug.Log("Цель уничтожена! Ищем новую...");
                GameObject newTarget = FindPriorityTarget();
                if (newTarget != null) StartCoroutine(AttackSequence(newTarget));
                yield break;
            }
        }
    }

    void MoveUnitsToBase()
    {
        if (ZonePlayer == null)
        {
            Debug.LogError("ZonePlayer не назначен в инспекторе!");
            return;
        }

        foreach (GameObject unit in attackUnits)
        {
            if (unit == null || !unit.activeSelf) continue;

            NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();
            if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
            {
                agent.SetDestination(ZonePlayer.transform.position);
            }
        }

        Debug.Log("Юниты перемещаются к базе игрока.");
    }
}