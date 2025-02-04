using UnityEngine;
using UnityEngine.AI;

public class UnitHpAndCommand : MonoBehaviour
{
    [Header("Настройки")]
    public ConfigManager configManager;

    [Header("Боевые параметры")]
    private float attackCooldown = 1.5f; // Задержка между атаками

    // Приватные переменные
    private NavMeshAgent agent;
    private int currentHP;
    private int maxHP;
    private float detectionRange;
    private float attackRange;
    private float attackDamage;
    private float lastAttackTime;
    private GameObject currentTarget; // Текущая цель для атаки

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

        // Если это вражеский юнит, обрабатываем его логику
        if (gameObject.CompareTag("UnitVrag"))
        {
            HandleEnemyLogic();
        }
        // Если это юнит игрока, обрабатываем атаку цели
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
            // Враги ищут юнитов игрока (Unit)
            FindTargetByTag("Unit");
        }
        else
        {
            // Юниты игрока ищут врагов (UnitVrag)
            FindTargetByTag("UnitVrag");
        }

        // Если не нашли юнитов, ищем здания
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
        // Ищем ближайший объект с тегом "Buildings"
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
            // Двигаемся к цели
            agent.SetDestination(currentTarget.transform.position);
            agent.isStopped = false;
        }
        else
        {
            // Останавливаемся, если цель в пределах досягаемости
            agent.isStopped = true;
        }
    }

    void TryAttack()
    {
        if (currentTarget == null) return;

        // Проверяем, прошло ли достаточно времени с последней атаки
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
        Debug.Log($"Атакую цель: {currentTarget.name}");

        // Проверяем, есть ли у цели компонент UnitHpAndCommand (для юнитов)
        UnitHpAndCommand targetUnit = currentTarget.GetComponent<UnitHpAndCommand>();
        if (targetUnit != null)
        {
            Debug.Log("Цель - юнит. Наношу урон.");
            targetUnit.TakeDamage((int)attackDamage);
        }
        else
        {
            // Если у цели нет компонента UnitHpAndCommand, проверяем наличие компонента BuildingsHP (для зданий)
            BuildingsHP targetBuilding = currentTarget.GetComponent<BuildingsHP>();
            if (targetBuilding != null)
            {
                Debug.Log("Цель - здание. Наношу урон.");
                targetBuilding.TakeDamage((int)attackDamage);
            }
            else
            {
                Debug.Log("У цели нет компонента UnitHpAndCommand или BuildingsHP.");
            }
        }

        // Если цель уничтожена, сбрасываем текущую цель
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
