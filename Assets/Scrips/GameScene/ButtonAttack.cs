//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;

//public class ButtonAttack : MonoBehaviour
//{
//    public GameObject ZonePlayer;  // �� ��� ���������� ��� ���������� ���� (���� �����)
//    private GameObject[] attackunits;
//    private GameObject townCenter; // ������ �� Town_Center, ����� ��������� ���

//    void Start()
//    {
//        // ������ ������ � ������ Vrag_1 � ������� ������ �� Town_Center
//        GameObject vrag1 = GameObject.Find("Vrag_1");
//        if (vrag1 != null)
//        {
//            townCenter = vrag1.transform.Find("Town_Center")?.gameObject;
//        }
//    }

//    public void Attack()
//    {
//        // ������ ��� �����
//        Debug.Log("����� �� ������!");

//        // ������ ���� ������ ��� �����
//        string[] unitNames = { "Siege_Tower", "Light_infantry", "Heavy", "Catapult", "Archer" };

//        // ���� ��� ������� � ������� ������� � ��������� �� � ������
//        List<GameObject> unitsList = new List<GameObject>();

//        foreach (string unitName in unitNames)
//        {
//            GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");

//            foreach (GameObject unit in units)
//            {
//                if (unit.name.Contains(unitName)) // ���� ��� ������� ��������� � ������
//                {
//                    unitsList.Add(unit);

//                    // ���� � ���������� "SelectionSprite"
//                    Transform selectionSprite = unit.transform.Find("SelectionSprite");
//                    if (selectionSprite != null)
//                    {
//                        selectionSprite.gameObject.SetActive(true);
//                    }
//                }
//            }
//        }

//        // ����������� ������ � ������
//        attackunits = unitsList.ToArray();

//        // ���������� ��� ��������� ����� � ������ �� ����
//        foreach (GameObject unit in attackunits)
//        {
//            Debug.Log($"���� {unit.name} ������ �������!");

//            // ������������� ���� ��� ������� ����� (�������� � Town_Center)
//            MoveUnitToTarget(unit);
//            AttackEnemy(unit);
//        }
//    }

//    void MoveUnitToTarget(GameObject unit)
//    {
//        if (townCenter != null)
//        {
//            // �������� NavMeshAgent � ������ ����
//            NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();
//            if (agent != null)
//            {
//                agent.SetDestination(townCenter.transform.position);
//                agent.isStopped = false; // ����������, ��� ����� ���������
//            }
//        }
//        else
//        {
//            Debug.LogWarning("Town_Center �� ������!");
//        }
//    }

//    void AttackEnemy(GameObject unit)
//    {
//        // ������� ��������� ����� ��� ����� (���� ���� �� ���� UnitVrag)
//        GameObject[] enemyUnits = GameObject.FindGameObjectsWithTag("UnitVrag");
//        foreach (GameObject enemy in enemyUnits)
//        {
//            // ���������, ���� �� �� ��������� ������� ������ ��������� ��� ��������� �����
//            UnitHpAndCommand enemyCommand = enemy.GetComponent<UnitHpAndCommand>();
//            if (enemyCommand != null)
//            {
//                // ���� ��������� ���� ������, ������� ���
//                enemyCommand.TakeDamage(10); // ������ �����
//                Debug.Log($"{unit.name} ������� {enemy.name}");
//                break; // ���� ���� ������� ������ �����, ��������� ���� (����� ��������� ��� ����������)
//            }
//        }
//    }
//}
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class ButtonAttack : MonoBehaviour
{
    public GameObject ZonePlayer; // ������ ZonePlayer
    private GameObject[] attackUnits; // ������ ������ ��� �����

    public void Attack()
    {
        Debug.Log("����� �� ������!");

        // ���� ��� ������� � ������� ������� � ��������� �� � ������
        List<GameObject> unitsList = new List<GameObject>();

        // ������ ���� ������, ������� ����� ��������
        string[] unitNames = { "Siege_Tower", "Light_infantry", "Heavy", "Catapult", "Archer" };

        // ���������� ��� ������� � ����� "Unit"
        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");

        foreach (GameObject unit in units)
        {
            foreach (string unitName in unitNames)
            {
                if (unit.name.Contains(unitName))
                {
                    unitsList.Add(unit);

                    // ���� � ���������� "SelectionSprite"
                    Transform selectionSprite = unit.transform.Find("SelectionSprite");
                    if (selectionSprite != null)
                    {
                        selectionSprite.gameObject.SetActive(true);
                    }
                }
            }
        }

        // ����������� ������ � ������
        attackUnits = unitsList.ToArray();

        // ������� ���� ��� �����
        GameObject target = FindBestAttackTarget(attackUnits);
        if (target == null)
        {
            Debug.Log("��� ����� ��� �����!");
            return;
        }

        //// ���������� ������ � ����
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

        // ������� ���� ��������� ��������� ������ (UnitVrag)
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

        // ���� ��������� ������ ���, ���� ��������� Town_Center ������ Vrag_1
        GameObject vrag1 = GameObject.Find("Vrag_1");
        if (vrag1 != null)
        {
            Transform townCenter = vrag1.transform.Find("Town_Center");
            if (townCenter != null)
            {
                return townCenter.gameObject;
            }
        }

        return null; // ���� ����� ���
    }
}