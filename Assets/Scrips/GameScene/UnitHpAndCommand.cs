using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class UnitHpAndCommand : MonoBehaviour
{
    public ConfigManager configManager;
    private NavMeshAgent agent;
    private int hp;
    private int maxHp;
    private float detectionRange;
    private float[] attack;
    private string nameUnit;
    private Dictionary<string, float> attackData = new Dictionary<string, float>
    {
        { "minRange", 0 },
        { "maxRange", 0 },
        { "attackDelay", 0 },
        { "damage", 0 }
    };
    private Dictionary<string, float> builder = new Dictionary<string, float>
    {
        { "resourceGatheringSpeed", 0 },
        { "repairSpeed", 0 },
        { "repairEfficiency", 0 }
    };

    private void Start()
    {
        // Получаем имя текущего GameObject
        string testname = gameObject.name;
        nameUnit = testname.Split('(')[0];

        agent = GetComponent<NavMeshAgent>();
        // Находим данные юнита в конфиге
        UnitData unitConfig = configManager.GetUnitConfig(nameUnit);

        if (unitConfig != null)
        {
            maxHp = unitConfig.health;
            hp = maxHp;
        }
        else
        {
            Debug.LogError($"Unit config for {nameUnit} not found!");
        }
        agent.speed = unitConfig.movementSpeed;
        detectionRange = unitConfig.detectionRange;
        if (nameUnit != "Worker" && nameUnit != "Healer")
        {
            attackData["minRange"] = unitConfig.attack.minRange;
            attackData["maxRange"] = unitConfig.attack.maxRange;
            attackData["attackDelay"] = unitConfig.attack.attackDelay;
            attackData["damage"] = unitConfig.attack.damage;
        }
        else if (nameUnit == "Worker")
        {
            builder["resourceGatheringSpeed"] = unitConfig.builder.resourceGatheringSpeed;
            builder["repairSpeed"] = unitConfig.builder.repairSpeed;
            builder["repairEfficiency"] = unitConfig.builder.repairEfficiency;
        }
    }

    private void Update()
    {
        if (hp == 0)
        {
            Destroy(gameObject);
        }

        // Check if the unit is an enemy unit (UnitVrag)
        if (nameUnit == "UnitVrag")
        {
            DetectAndAttackUnits();
        }
    }

    private void DetectAndAttackUnits()
    {
        // Detect units within the detection range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange);
        bool foundTarget = false;

        foreach (Collider hitCollider in hitColliders)
        {
            GameObject hitObject = hitCollider.gameObject;
            if (hitObject != gameObject && hitObject.CompareTag("Unit"))
            {
                // Ensure the target unit is active and enabled
                if (hitObject.activeInHierarchy && hitObject.GetComponent<NavMeshAgent>().isActiveAndEnabled)
                {
                    // Attack the detected unit
                    AttackUnit(hitObject);
                    foundTarget = true;
                    break; // Attack only the first detected unit
                }
            }
        }

        if (!foundTarget)
        {
            // If no units are found, attack the base
            AttackBase();
        }
    }

    private void AttackUnit(GameObject target)
    {
        // Set the target as the destination for the NavMeshAgent
        agent.SetDestination(target.transform.position);

        // Check if the unit is within attack range
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        if (distanceToTarget <= attackData["maxRange"] && distanceToTarget >= attackData["minRange"])
        {
            // Perform attack logic here
            Debug.Log("Attacking unit: " + target.name);
            // Apply damage to the target unit
            UnitHpAndCommand targetUnitScript = target.GetComponent<UnitHpAndCommand>();
            if (targetUnitScript != null)
            {
                targetUnitScript.TakeDamage((int)attackData["damage"]);
            }
        }
    }

    private void AttackBase()
    {
        // Set the base as the destination for the NavMeshAgent
        GameObject zonePlayer = GameObject.FindGameObjectWithTag("ZonePlayer");
        if (zonePlayer != null)
        {
            agent.SetDestination(zonePlayer.transform.position);
            Debug.Log("Attacking base: " + zonePlayer.name);
        }
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp < 0)
        {
            hp = 0;
        }
        Debug.Log(nameUnit + " took " + damage + " damage. HP: " + hp);
    }
}