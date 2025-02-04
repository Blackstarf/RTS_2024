using UnityEngine;
using UnityEngine.AI;

public class UnitHpAndCommand : MonoBehaviour
{
    [Header("���������")]
    public ConfigManager configManager;

    [Header("������ ���������")]
    private float attackCooldown = 1.5f; // �������� ����� �������

    // ��������� ����������
    private NavMeshAgent agent;
    private int currentHP;
    private int maxHP;
    private float detectionRange;
    private float attackRange;
    private float attackDamage;
    private float lastAttackTime;
    private GameObject currentTarget; // ������� ���� ��� �����

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        InitializeUnitStats();
    }

    void InitializeUnitStats()
    {
        string unitName = gameObject.name.Split('(')[0];
        UnitData config = configManager.GetUnitConfig(unitName);
        if (config == null)
        {
            Debug.LogError($"������ ��� {unitName} �� ������!");
            return;
        }

        maxHP = config.health;
        currentHP = maxHP;
        agent.speed = config.movementSpeed;
        detectionRange = config.detectionRange;

        if (config.attack != null)
        {
            attackRange = config.attack.maxRange;
            attackDamage = config.attack.damage;
        }
    }

    void Update()
    {
        if (currentHP <= 0)
        {
            Destroy(gameObject);
            return;
        }

        // ���� ��� ��������� ����, ������������ ��� ������
        if (gameObject.CompareTag("UnitVrag"))
        {
            HandleEnemyLogic();
        }
        // ���� ��� ���� ������, ������������ ����� ����
        else if (currentTarget != null)
        {
            MoveToTarget();
            TryAttack();
        }
    }

    void HandleEnemyLogic()
    {
        if (currentTarget == null)
        {
            FindNewTarget();
        }

        if (currentTarget != null)
        {
            MoveToTarget();
            TryAttack();
        }
    }

    void FindNewTarget()
    {
        if (gameObject.CompareTag("UnitVrag"))
        {
            // ����� ���� ������ ������ (Unit)
            FindTargetByTag("Unit");
        }
        else
        {
            // ����� ������ ���� ������ (UnitVrag)
            FindTargetByTag("UnitVrag");
        }

        // ���� �� ����� ������, ���� ������
        if (currentTarget == null)
        {
            FindBuildingsTarget();
        }
    }

    void FindTargetByTag(string targetTag)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
        float closestDistance = Mathf.Infinity;
        GameObject closestTarget = null;

        foreach (GameObject target in targets)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = target;
            }
        }

        if (closestTarget != null && closestDistance <= detectionRange)
        {
            currentTarget = closestTarget;
        }
    }

    void FindBuildingsTarget()
    {
        // ���� ��������� ������ � ����� "Buildings"
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("BasePlayer");
        float closestDistance = Mathf.Infinity;
        GameObject closestBuilding = null;

        foreach (GameObject building in buildings)
        {
            float distance = Vector3.Distance(transform.position, building.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestBuilding = building;
            }
        }

        if (closestBuilding != null && closestDistance <= detectionRange)
        {
            // ���� ����� Building � ���� �����������, �������� ��� ��� ����
            currentTarget = closestBuilding;
        }
    }

    void MoveToTarget()
    {
        if (currentTarget == null || !agent.isOnNavMesh) return;

        float distance = Vector3.Distance(transform.position, currentTarget.transform.position);
        if (distance > attackRange)
        {
            // ��������� � ����
            agent.SetDestination(currentTarget.transform.position);
            agent.isStopped = false;
        }
        else
        {
            // ���������������, ���� ���� � �������� ������������
            agent.isStopped = true;
        }
    }

    void TryAttack()
    {
        if (currentTarget == null) return;

        // ���������, ������ �� ���������� ������� � ��������� �����
        if (Time.time - lastAttackTime < attackCooldown) return;

        float distance = Vector3.Distance(transform.position, currentTarget.transform.position);
        if (distance <= attackRange)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    void Attack()
    {
        Debug.Log($"������ ����: {currentTarget.name}");

        // ���������, ���� �� � ���� ��������� UnitHpAndCommand (��� ������)
        UnitHpAndCommand targetUnit = currentTarget.GetComponent<UnitHpAndCommand>();
        if (targetUnit != null)
        {
            Debug.Log("���� - ����. ������ ����.");
            targetUnit.TakeDamage((int)attackDamage);
        }
        else
        {
            // ���� � ���� ��� ���������� UnitHpAndCommand, ��������� ������� ���������� BuildingsHP (��� ������)
            BuildingsHP targetBuilding = currentTarget.GetComponent<BuildingsHP>();
            if (targetBuilding != null)
            {
                Debug.Log("���� - ������. ������ ����.");
                targetBuilding.TakeDamage((int)attackDamage);
            }
            else
            {
                Debug.Log("� ���� ��� ���������� UnitHpAndCommand ��� BuildingsHP.");
            }
        }

        // ���� ���� ����������, ���������� ������� ����
        if (currentTarget == null || currentTarget.GetComponent<UnitHpAndCommand>() == null && currentTarget.GetComponent<BuildingsHP>() == null)
        {
            currentTarget = null;
        }
    }

    public void SetAttackTarget(GameObject target)
    {
        currentTarget = target;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            //if (gameObject.tag == "BasePlayer") { 
            //}
            Destroy(gameObject);
        }
    }
}
