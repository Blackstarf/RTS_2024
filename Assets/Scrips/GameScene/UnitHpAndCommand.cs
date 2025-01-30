using UnityEngine;
using UnityEngine.AI;

public class UnitHpAndCommand : MonoBehaviour
{
    [Header("���������")]
    public ConfigManager configManager;
    // public GameObject ZonePlayer; // ZonePlayer, ������ �������� ���� Archer � Worker

    [Header("������ ���������")]
    public float attackCooldown = 1.5f; // �������� ����� �������

    // ��������� ����������
    private NavMeshAgent agent;
    private int currentHP;
    private int maxHP;
    private float detectionRange;
    private float attackRange;
    private float attackDamage;
    private float lastAttackTime;
    private GameObject currentTarget;

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

        if (gameObject.CompareTag("UnitVrag")) // ���� ��� ��������� ����
        {
            HandleEnemyLogic();
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
        // ���� ��������� ������ � ����� "Unit"
        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
        float closestDistance = Mathf.Infinity;
        GameObject closestUnit = null;

        foreach (GameObject unit in units)
        {
            float distance = Vector3.Distance(transform.position, unit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestUnit = unit;
            }
        }

        if (closestUnit != null && closestDistance <= detectionRange)
        {
            // ���� ����� Unit � ���� �����������, �������� ��� ��� ����
            currentTarget = closestUnit;
        }
        else
        {
            // ���� �� ����� Unit, ���� ������� � ����� "Buildings"
            FindBuildingsTarget();
        }
    }

    void FindBuildingsTarget()
    {
        // ���� ��������� ������ � ����� "Buildings"
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Buildings");
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
            agent.SetDestination(currentTarget.transform.position);
            agent.isStopped = false;
        }
        else
        {
            agent.isStopped = true;
        }
    }

    void TryAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        if (Vector3.Distance(transform.position, currentTarget.transform.position) <= attackRange)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    void Attack()
    {
        UnitHpAndCommand target = currentTarget.GetComponent<UnitHpAndCommand>();
        if (target != null)
        {
            target.TakeDamage((int)attackDamage);
        }
        //else
        //{
        //    // ���� � ���� ��� ���������� UnitHpAndCommand, ������� ������� ���� ����� ������ ��������� (��������, BuildingHp)
        //    BuildingHp buildingTarget = currentTarget.GetComponent<BuildingHp>();
        //    if (buildingTarget != null)
        //    {
        //        buildingTarget.TakeDamage((int)attackDamage);
        //    }
        //}
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
//using UnityEngine;
//using UnityEngine.AI;

//public class UnitHpAndCommand : MonoBehaviour
//{
//    [Header("���������")]
//    public ConfigManager configManager;
//    public GameObject ZonePlayer; // ZonePlayer, ������ �������� ���� Archer � Worker

//    [Header("������ ���������")]
//    public float attackCooldown = 1.5f; // �������� ����� �������
//    public float attackRangeForBuildings = 10f; // ����������� ������ ����� ��� ������

//    // ��������� ����������
//    private NavMeshAgent agent;
//    private int currentHP;
//    private int maxHP;
//    private float detectionRange;
//    private float attackRange;
//    private float attackDamage;
//    private float lastAttackTime;
//    private GameObject currentTarget;

//    void Start()
//    {
//        agent = GetComponent<NavMeshAgent>();
//        InitializeUnitStats();
//    }

//    void InitializeUnitStats()
//    {
//        string unitName = gameObject.name.Split('(')[0];
//        UnitData config = configManager.GetUnitConfig(unitName);
//        if (config == null)
//        {
//            Debug.LogError($"������ ��� {unitName} �� ������!");
//            return;
//        }

//        maxHP = config.health;
//        currentHP = maxHP;
//        agent.speed = config.movementSpeed;
//        detectionRange = config.detectionRange;

//        if (config.attack != null)
//        {
//            attackRange = config.attack.maxRange;
//            attackDamage = config.attack.damage;
//        }
//    }

//    void Update()
//    {
//        if (currentHP <= 0)
//        {
//            Destroy(gameObject);
//            return;
//        }

//        if (gameObject.CompareTag("UnitVrag")) // ���� ��� ��������� ����
//        {
//            HandleEnemyLogic();
//        }
//    }

//    void HandleEnemyLogic()
//    {
//        //// ���� ������� ���� ����������, ���������� �
//        //if (currentTarget == null || currentTarget.GetComponent<UnitHpAndCommand>()?.currentHP <= 0 || currentTarget.GetComponent<BuildingsHP>()?.currentHP <= 0)
//        //{
//        //    currentTarget = null;
//        //}

//        if (currentTarget == null)
//        {
//            FindNewTarget();
//        }

//        if (currentTarget != null)
//        {
//            MoveToTarget();
//            TryAttack();
//        }
//    }

//    void FindNewTarget()
//    {
//        // ���� ��������� ������ � ����� "Unit"
//        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
//        float closestDistance = Mathf.Infinity;
//        GameObject closestUnit = null;

//        foreach (GameObject unit in units)
//        {
//            if (unit.GetComponent<UnitHpAndCommand>()?.currentHP > 0) // ���������, ��� ���� ���
//            {
//                float distance = Vector3.Distance(transform.position, unit.transform.position);
//                if (distance < closestDistance)
//                {
//                    closestDistance = distance;
//                    closestUnit = unit;
//                }
//            }
//        }

//        if (closestUnit != null && closestDistance >= detectionRange)
//        {
//            // ���� ����� Unit � ���� �����������, �������� ��� ��� ����
//            currentTarget = closestUnit;
//            Debug.Log($"{gameObject.name} ����� �����: {currentTarget.name}");
//        }
//        else
//        {
//            // ���� �� ����� Unit, ���� ������� � ����� "Buildings"
//            FindBuildingsTarget();
//        }
//    }

//    void FindBuildingsTarget()
//    {
//        // ���� ��������� ������ � ����� "Buildings"
//        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Buildings");
//        float closestDistance = Mathf.Infinity;
//        GameObject closestBuilding = null;

//        foreach (GameObject building in buildings)
//        {
//            //if (building.GetComponent<BuildingsHP>()?.currentHP > 0) // ���������, ��� ������ �� ���������
//            //{
//                float distance = Vector3.Distance(transform.position, building.transform.position);
//                if (distance < closestDistance)
//                {
//                    closestDistance = distance;
//                    closestBuilding = building;
//                }
//            //}
//        }

//        if (closestBuilding != null && closestDistance >= detectionRange)
//        {
//            // ���� ����� Building � ���� �����������, �������� ��� ��� ����
//            currentTarget = closestBuilding;
//            Debug.Log($"{gameObject.name} ����� ������: {currentTarget.name}");
//        }
//        else
//        {
//            Debug.Log($"{gameObject.name} �� ����� �� ������, �� ������.");
//        }
//    }

//    void MoveToTarget()
//    {
//        if (currentTarget == null || !agent.isOnNavMesh) return;

//        float distance = Vector3.Distance(transform.position, currentTarget.transform.position);
//        float effectiveAttackRange = currentTarget.CompareTag("Buildings") ? attackRangeForBuildings : attackRange;

//        if (distance > effectiveAttackRange)
//        {
//            agent.SetDestination(currentTarget.transform.position);
//            agent.isStopped = false;
//        }
//        else
//        {
//            agent.isStopped = true;
//        }
//    }

//    void TryAttack()
//    {
//        if (Time.time - lastAttackTime < attackCooldown) return;

//        float effectiveAttackRange = currentTarget.CompareTag("Buildings") ? attackRangeForBuildings : attackRange;
//        if (Vector3.Distance(transform.position, currentTarget.transform.position) <= effectiveAttackRange)
//        {
//            Attack();
//            lastAttackTime = Time.time;
//        }
//    }

//    void Attack()
//    {
//        UnitHpAndCommand target = currentTarget.GetComponent<UnitHpAndCommand>();
//        if (target != null)
//        {
//            target.TakeDamage((int)attackDamage);
//        }
//        else
//        {
//            // ���� � ���� ��� ���������� UnitHpAndCommand, ������� ������� ���� ����� BuildingHp
//            BuildingsHP buildingTarget = currentTarget.GetComponent<BuildingsHP>();
//            if (buildingTarget != null)
//            {
//                buildingTarget.TakeDamage((int)attackDamage);
//            }
//        }
//    }

//    public void TakeDamage(int damage)
//    {
//        currentHP -= damage;
//        if (currentHP <= 0)
//        {
//            Destroy(gameObject);
//        }
//    }
//}