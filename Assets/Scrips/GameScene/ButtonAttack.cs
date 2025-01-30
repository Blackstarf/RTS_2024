//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;

//public class ButtonAttack : MonoBehaviour
//{
//    public GameObject ZonePlayer;  // Мы его используем для нахождения цели (если нужно)
//    private GameObject[] attackunits;
//    private GameObject townCenter; // Ссылка на Town_Center, чтобы атаковать его

//    void Start()
//    {
//        // Найдем объект с именем Vrag_1 и получим ссылку на Town_Center
//        GameObject vrag1 = GameObject.Find("Vrag_1");
//        if (vrag1 != null)
//        {
//            townCenter = vrag1.transform.Find("Town_Center")?.gameObject;
//        }
//    }

//    public void Attack()
//    {
//        // Логика для атаки
//        Debug.Log("Атака на юнитов!");

//        // Список имен юнитов для атаки
//        string[] unitNames = { "Siege_Tower", "Light_infantry", "Heavy", "Catapult", "Archer" };

//        // Ищем все объекты с нужными именами и добавляем их в массив
//        List<GameObject> unitsList = new List<GameObject>();

//        foreach (string unitName in unitNames)
//        {
//            GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");

//            foreach (GameObject unit in units)
//            {
//                if (unit.name.Contains(unitName)) // Если имя объекта совпадает с нужным
//                {
//                    unitsList.Add(unit);

//                    // Ищем и активируем "SelectionSprite"
//                    Transform selectionSprite = unit.transform.Find("SelectionSprite");
//                    if (selectionSprite != null)
//                    {
//                        selectionSprite.gameObject.SetActive(true);
//                    }
//                }
//            }
//        }

//        // Преобразуем список в массив
//        attackunits = unitsList.ToArray();

//        // Перебираем все найденные юниты и ставим их цель
//        foreach (GameObject unit in attackunits)
//        {
//            Debug.Log($"Юнит {unit.name} теперь атакует!");

//            // Устанавливаем цель для каждого юнита (движемся к Town_Center)
//            MoveUnitToTarget(unit);
//            AttackEnemy(unit);
//        }
//    }

//    void MoveUnitToTarget(GameObject unit)
//    {
//        if (townCenter != null)
//        {
//            // Получаем NavMeshAgent и задаем цель
//            NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();
//            if (agent != null)
//            {
//                agent.SetDestination(townCenter.transform.position);
//                agent.isStopped = false; // Убеждаемся, что агент двигается
//            }
//        }
//        else
//        {
//            Debug.LogWarning("Town_Center не найден!");
//        }
//    }

//    void AttackEnemy(GameObject unit)
//    {
//        // Находим вражеские юниты для атаки (пока ищем по тегу UnitVrag)
//        GameObject[] enemyUnits = GameObject.FindGameObjectsWithTag("UnitVrag");
//        foreach (GameObject enemy in enemyUnits)
//        {
//            // Проверяем, есть ли на вражеском объекте нужный компонент для получения урона
//            UnitHpAndCommand enemyCommand = enemy.GetComponent<UnitHpAndCommand>();
//            if (enemyCommand != null)
//            {
//                // Если вражеский юнит найден, атакуем его
//                enemyCommand.TakeDamage(10); // Пример урона
//                Debug.Log($"{unit.name} атакует {enemy.name}");
//                break; // Если юнит атакует одного врага, прерываем цикл (можно расширить для нескольких)
//            }
//        }
//    }
//}
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

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

        // Перебираем все объекты с тегом "Unit"
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

        // Находим цель для атаки
        GameObject target = FindBestAttackTarget(attackUnits);
        if (target == null)
        {
            Debug.Log("Нет целей для атаки!");
            return;
        }

        //// Отправляем юнитов к цели
        //foreach (GameObject unit in attackUnits)
        //{
        //    UnitHpAndCommand unitScript = unit.GetComponent<UnitHpAndCommand>();
        //    if (unitScript != null)
        //    {
        //        unitScript.SetAttackTarget(target);
        //    }
        //}
    }

    private GameObject FindBestAttackTarget(GameObject[] potentialAttackers)
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
            return closestEnemyUnit;

        // Если вражеских юнитов нет, ищем ближайший Town_Center внутри Vrag_1
        GameObject vrag1 = GameObject.Find("Vrag_1");
        if (vrag1 != null)
        {
            Transform townCenter = vrag1.transform.Find("Town_Center");
            if (townCenter != null)
            {
                return townCenter.gameObject;
            }
        }

        return null; // Если целей нет
    }
}