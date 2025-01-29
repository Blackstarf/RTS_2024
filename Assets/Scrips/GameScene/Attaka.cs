//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;

//public class Attaka : MonoBehaviour
//{
//    public GameObject ZonePlayer, Vrags; // GameObject to which units will move
//    public float detectionRadius = 10f; // Radius for detecting enemy units/buildings
//    public float minAttackDistance = 2f; // Minimum distance for attacking
//    public float maxAttackDistance = 5f; // Maximum distance for attacking
//    public float buildProgress = 0f; // Progress of building construction
//    public float buildTime = 10f; // Time required to build a building
//    public int maxUnits = 5; // Maximum number of units to train

//    private List<GameObject> enemyUnits = new List<GameObject>(); // List to hold enemy units
//    private List<GameObject> playerUnits = new List<GameObject>(); // List to hold player units
//    private List<GameObject> playerBuildings = new List<GameObject>(); // List to hold player buildings

//    void Start()
//    {
//        // Initialize enemy units and buildings
//        InitializeEnemyUnits();
//    }

//    void Update()
//    {
//        // Check if there are 5 or more units under Vrags and attack ZonePlayer
//        CheckAndAttackZonePlayer();

//        // Update building progress
//        UpdateBuildingProgress();

//        // Check for enemy units in detection radius
//        DetectAndAttackPlayerUnits();
//    }

//    void InitializeEnemyUnits()
//    {
//        Transform vrag1Transform = Vrags.transform.Find("Vrag_1");
//        if (vrag1Transform != null)
//        {
//            foreach (Transform child in vrag1Transform)
//            {
//                GameObject unit = child.gameObject;
//                if (unit.CompareTag("Unit"))
//                {
//                    enemyUnits.Add(unit);
//                }
//            }
//        }
//    }

//    void CheckAndAttackZonePlayer()
//    {
//        if (enemyUnits.Count >= maxUnits)
//        {
//            foreach (GameObject unit in enemyUnits)
//            {
//                NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();
//                if (agent != null && agent.isActiveAndEnabled)
//                {
//                    agent.SetDestination(ZonePlayer.transform.position);
//                    Debug.Log("Unit: " + unit.name + " is attacking ZonePlayer");
//                }
//                else
//                {
//                    Debug.LogWarning("Unit: " + unit.name + " is not active or not on NavMesh");
//                }
//            }
//        }
//    }

//    void UpdateBuildingProgress()
//    {
//        // Simulate building progress
//        buildProgress += Time.deltaTime / buildTime;
//        buildProgress = Mathf.Clamp01(buildProgress);

//        // Update building progress UI (if any)
//        // Example: Update a progress bar or text display
//    }

//    void DetectAndAttackPlayerUnits()
//    {
//        foreach (GameObject unit in enemyUnits)
//        {
//            foreach (GameObject playerUnit in playerUnits)
//            {
//                float distance = Vector3.Distance(unit.transform.position, playerUnit.transform.position);
//                if (distance <= detectionRadius)
//                {
//                    NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();
//                    if (agent != null && agent.isActiveAndEnabled)
//                    {
//                        if (distance > maxAttackDistance)
//                        {
//                            agent.SetDestination(playerUnit.transform.position);
//                        }
//                        else if (distance < minAttackDistance)
//                        {
//                            agent.SetDestination(unit.transform.position + (unit.transform.position - playerUnit.transform.position).normalized * minAttackDistance);
//                        }
//                        else
//                        {
//                            // Attack logic here
//                            Debug.Log("Unit: " + unit.name + " is attacking " + playerUnit.name);
//                        }
//                    }
//                }
//            }

//            foreach (GameObject playerBuilding in playerBuildings)
//            {
//                float distance = Vector3.Distance(unit.transform.position, playerBuilding.transform.position);
//                if (distance <= detectionRadius)
//                {
//                    NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();
//                    if (agent != null && agent.isActiveAndEnabled)
//                    {
//                        if (distance > maxAttackDistance)
//                        {
//                            agent.SetDestination(playerBuilding.transform.position);
//                        }
//                        else if (distance < minAttackDistance)
//                        {
//                            agent.SetDestination(unit.transform.position + (unit.transform.position - playerBuilding.transform.position).normalized * minAttackDistance);
//                        }
//                        else
//                        {
//                            // Attack logic here
//                            Debug.Log("Unit: " + unit.name + " is attacking " + playerBuilding.name);
//                        }
//                    }
//                }
//            }
//        }
//    }

//    void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("PlayerUnit"))
//        {
//            playerUnits.Add(other.gameObject);
//        }
//        else if (other.CompareTag("PlayerBuilding"))
//        {
//            playerBuildings.Add(other.gameObject);
//        }
//    }

//    void OnTriggerExit(Collider other)
//    {
//        if (other.CompareTag("PlayerUnit"))
//        {
//            playerUnits.Remove(other.gameObject);
//        }
//        else if (other.CompareTag("PlayerBuilding"))
//        {
//            playerBuildings.Remove(other.gameObject);
//        }
//    }
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Attaka : MonoBehaviour
{
    public GameObject ZonePlayer, Vrags; // GameObject to which units will move
    private bool hasAttacked = false; // Flag to track if the attack log has been printed

    void Update()
    {
        // Check if there are 5 or more units under Vrags and attack ZonePlayer
        CheckAndAttackZonePlayer();
    }

    void CheckAndAttackZonePlayer()
    {
        Transform vrag1Transform = Vrags.transform.Find("Vrag_1");
        if (vrag1Transform != null)
        {
            List<GameObject> unitsUnderVrag1 = new List<GameObject>();
            foreach (Transform child in vrag1Transform)
            {
                GameObject unit = child.gameObject;
                if (unit.CompareTag("UnitVrag"))
                {
                    unitsUnderVrag1.Add(unit);
                }
            }

            if (unitsUnderVrag1.Count >= 5)
            {
                if (!hasAttacked)
                {
                    Debug.Log("Vrag_1 is attacking ZonePlayer");
                    hasAttacked = true; // Set the flag to true after printing the log
                }

                foreach (GameObject unit in unitsUnderVrag1)
                {
                    NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();
                    if (agent != null && agent.isActiveAndEnabled)
                    {
                        agent.SetDestination(ZonePlayer.transform.position);
                       // Debug.Log("Unit: " + unit.name + " is attacking ZonePlayer");
                    }
                    else
                    {
                        Debug.LogWarning("Unit: " + unit.name + " is not active or not on NavMesh");
                    }
                }
            }
        }
    }
}

