using UnityEngine;
using UnityEngine.AI;

public class UnitHpAndCommand : MonoBehaviour
{
    public ConfigManager configManager;
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
            attackCooldown = config.attack.attackDelay;
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
        // ���� ��� ���� ������, ������������ ����� ����
        else if (currentTarget != null)
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
            FindTargetByTag("BasePlayer");
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
        }
        //// ���� ���� ����������, ���������� ������� ����
        //if (currentTarget == null || currentTarget.GetComponent<UnitHpAndCommand>() == null && currentTarget.GetComponent<BuildingsHP>() == null)
        //{
        //    currentTarget = null;
        //}
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
            Destroy(gameObject);
        }
    }
}
