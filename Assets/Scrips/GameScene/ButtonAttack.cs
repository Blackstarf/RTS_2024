using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.AI;

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

        // ���������� ��� ������� � ����� "Unit" (����� ������)
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

        // ���� ��� ������ ��� �����
        if (attackUnits.Length == 0)
        {
            Debug.Log("��� ��������� ������ ��� �����!");
            return;
        }

        // ������� ���� ��� �����
        GameObject target = FindBestAttackTarget();
        if (target == null)
        {
            Debug.Log("��� ����� ��� �����!");
            return;
        }

        // ���������� ������ � ����
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
        {
            Debug.Log("������ ��������� ���� ��� �����: " + closestEnemyUnit.name);
            return closestEnemyUnit;
        }

        // ���� ��������� ������ ���, ���� ��������� Town_Center ������ Vrag_1
        GameObject vrag1 = GameObject.Find("Vrag_1");
        if (vrag1 != null)
        {
            Transform townCenter = vrag1.transform.Find("Town_Center");
            if (townCenter != null)
            {
                Debug.Log("������� ���� ��� �����: " + townCenter.name);
                return townCenter.gameObject;
            }
        }

        Debug.Log("����� ��� ����� �� �������.");
        return null; // ���� ����� ���
    }

    private void SendUnitsToAttack(GameObject target)
    {
        if (attackUnits == null || attackUnits.Length == 0)
        {
            Debug.Log("��� ������ ��� �����!");
            return;
        }

        foreach (GameObject unit in attackUnits)
        {
            // �������� ��������� NavMeshAgent ��� ����������� �����
            NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                // ������������� ���� ��� �����������
                agent.SetDestination(target.transform.position);
            }

            // �������� ��������� UnitHpAndCommand ��� �����
            UnitHpAndCommand unitScript = unit.GetComponent<UnitHpAndCommand>();
            if (unitScript != null)
            {
                // ������������� ���� ��� �����
                unitScript.SetAttackTarget(target);
            }
            else
            {
                Debug.LogWarning($"� ����� {unit.name} ����������� ��������� UnitHpAndCommand.");
            }
        }
    }
}