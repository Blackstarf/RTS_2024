using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.AI;

public class ButtonAttack : MonoBehaviour
{
    public GameObject ZonePlayer; // Объект ZonePlayer
    private GameObject[] attackUnits; // Массив юнитов для атаки

    public void Attack()
    {
        Debug.Log("Атака на врагов!");

        // Ищем все объекты с нужными именами и добавляем их в массив
        List<GameObject> unitsList = new List<GameObject>();

        // Список имен юнитов, которые нужно добавить
        string[] unitNames = { "Siege_Tower", "Light_infantry", "Heavy", "Catapult", "Archer" };

        // Перебираем все объекты с тегом "Unit" (юниты игрока)
        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");

        foreach (GameObject unit in units)
        {
            foreach (string unitName in unitNames)
            {
                if (unit.name.Contains(unitName))
                {
                    unitsList.Add(unit);

                    // Ищем и активируем "SelectionSprite"
                    Transform selectionSprite = unit.transform.Find("SelectionSprite");
                    if (selectionSprite != null)
                    {
                        selectionSprite.gameObject.SetActive(true);
                    }
                }
            }
        }

        // Преобразуем список в массив
        attackUnits = unitsList.ToArray();

        // Если нет юнитов для атаки
        if (attackUnits.Length == 0)
        {
            Debug.Log("Нет доступных юнитов для атаки!");
            return;
        }

        // Находим цель для атаки
        GameObject target = FindBestAttackTarget();
        if (target == null)
        {
            Debug.Log("Нет целей для атаки!");
            return;
        }

        // Отправляем юнитов к цели
        SendUnitsToAttack(target);
    }
    GameObject FindClosestEnemy(GameObject[] enemies)
    {
        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = enemy;
            }
        }
        return closest;
    }

    GameObject FindEnemyBase()
    {
        GameObject vrag1 = GameObject.Find("Vrag_1");
        if (vrag1 != null)
        {
            Transform townCenter = vrag1.transform.Find("Town_Center");
            return townCenter?.gameObject;
        }
        return null;
    }

    private GameObject FindBestAttackTarget()
    {
        GameObject closestEnemyUnit = null;
        float closestDistance = Mathf.Infinity;

        // Сначала ищем ближайших вражеских юнитов (UnitVrag)
        GameObject[] enemyUnits = GameObject.FindGameObjectsWithTag("UnitVrag");
        foreach (GameObject enemyUnit in enemyUnits)
        {
            float distance = Vector3.Distance(enemyUnit.transform.position, transform.position);
            if (distance < closestDistance)
            {
                closestEnemyUnit = enemyUnit;
                closestDistance = distance;
            }
        }

        if (closestEnemyUnit != null)
        {
            Debug.Log("Найден вражеский юнит для атаки: " + closestEnemyUnit.name);
            return closestEnemyUnit;
        }

        // Если вражеских юнитов нет, ищем ближайший Town_Center внутри Vrag_1
        GameObject vrag1 = GameObject.Find("Vrag_1");
        if (vrag1 != null)
        {
            Transform townCenter = vrag1.transform.Find("Town_Center");
            if (townCenter != null)
            {
                Debug.Log("Найдена база для атаки: " + townCenter.name);
                return townCenter.gameObject;
            }
        }

        Debug.Log("Целей для атаки не найдено.");
        return null; // Если целей нет
    }

    private void SendUnitsToAttack(GameObject target)
    {
        if (attackUnits == null || attackUnits.Length == 0)
        {
            Debug.Log("Нет юнитов для атаки!");
            return;
        }

        foreach (GameObject unit in attackUnits)
        {
            // Получаем компонент NavMeshAgent для перемещения юнита
            NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                // Устанавливаем цель для перемещения
                agent.SetDestination(target.transform.position);
            }

            // Получаем компонент UnitHpAndCommand для атаки
            UnitHpAndCommand unitScript = unit.GetComponent<UnitHpAndCommand>();
            if (unitScript != null)
            {
                // Устанавливаем цель для атаки
                unitScript.SetAttackTarget(target);
            }
            else
            {
                Debug.LogWarning($"У юнита {unit.name} отсутствует компонент UnitHpAndCommand.");
            }
        }
    }
}