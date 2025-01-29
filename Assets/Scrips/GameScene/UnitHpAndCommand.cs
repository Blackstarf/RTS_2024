using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

//[Serializable]
//public class Attack
//{
//    public float minRange;
//    public float maxRange;
//    public float attackDelay;
//    public int damage;
//}
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
        nameUnit = gameObject.name;
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
        if (nameUnit!= "Worker"&& nameUnit != "Healer")
        {
            attackData["minRange"]=unitConfig.attack.minRange;
            attackData["maxRange"] = unitConfig.attack.maxRange;
            attackData["attackDelay"] = unitConfig.attack.attackDelay;
            attackData["damage"] = unitConfig.attack.damage;
        }else if(nameUnit== "Worker")
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
    }
}
