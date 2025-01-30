using UnityEngine;
using UnityEngine.AI;

public class UnitHpAndCommand : MonoBehaviour
{
    [Header("Настройки")]
    public ConfigManager configManager;
    // public GameObject ZonePlayer; // ZonePlayer, внутри которого ищем Archer и Worker

    [Header("Боевые параметры")]
    public float attackCooldown = 1.5f; // Задержка между атаками

    // Приватные переменные
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
            Debug.LogError($"Конфиг для {unitName} не найден!");
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

        if (gameObject.CompareTag("UnitVrag")) // Если это вражеский юнит
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
        // Ищем ближайший объект с тегом "Unit"
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
            // Если нашли Unit в зоне обнаружения, выбираем его как цель
            currentTarget = closestUnit;
        }
        else
        {
            // Если не нашли Unit, ищем объекты с тегом "Buildings"
            FindBuildingsTarget();
        }
    }

    void FindBuildingsTarget()
    {
        // Ищем ближайший объект с тегом "Buildings"
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
            // Если нашли Building в зоне обнаружения, выбираем его как цель
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
        //    // Если у цели нет компонента UnitHpAndCommand, пробуем нанести урон через другой компонент (например, BuildingHp)
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
//    [Header("Настройки")]
//    public ConfigManager configManager;
//    public GameObject ZonePlayer; // ZonePlayer, внутри которого ищем Archer и Worker

//    [Header("Боевые параметры")]
//    public float attackCooldown = 1.5f; // Задержка между атаками
//    public float attackRangeForBuildings = 10f; // Увеличенный радиус атаки для зданий

//    // Приватные переменные
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
//            Debug.LogError($"Конфиг для {unitName} не найден!");
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

//        if (gameObject.CompareTag("UnitVrag")) // Если это вражеский юнит
//        {
//            HandleEnemyLogic();
//        }
//    }

//    void HandleEnemyLogic()
//    {
//        //// Если текущая цель уничтожена, сбрасываем её
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
//        // Ищем ближайший объект с тегом "Unit"
//        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
//        float closestDistance = Mathf.Infinity;
//        GameObject closestUnit = null;

//        foreach (GameObject unit in units)
//        {
//            if (unit.GetComponent<UnitHpAndCommand>()?.currentHP > 0) // Проверяем, что юнит жив
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
//            // Если нашли Unit в зоне обнаружения, выбираем его как цель
//            currentTarget = closestUnit;
//            Debug.Log($"{gameObject.name} нашел юнита: {currentTarget.name}");
//        }
//        else
//        {
//            // Если не нашли Unit, ищем объекты с тегом "Buildings"
//            FindBuildingsTarget();
//        }
//    }

//    void FindBuildingsTarget()
//    {
//        // Ищем ближайший объект с тегом "Buildings"
//        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Buildings");
//        float closestDistance = Mathf.Infinity;
//        GameObject closestBuilding = null;

//        foreach (GameObject building in buildings)
//        {
//            //if (building.GetComponent<BuildingsHP>()?.currentHP > 0) // Проверяем, что здание не разрушено
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
//            // Если нашли Building в зоне обнаружения, выбираем его как цель
//            currentTarget = closestBuilding;
//            Debug.Log($"{gameObject.name} нашел здание: {currentTarget.name}");
//        }
//        else
//        {
//            Debug.Log($"{gameObject.name} не нашел ни юнитов, ни зданий.");
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
//            // Если у цели нет компонента UnitHpAndCommand, пробуем нанести урон через BuildingHp
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