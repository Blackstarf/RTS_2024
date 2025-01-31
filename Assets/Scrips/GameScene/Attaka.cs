using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Attaka : MonoBehaviour
{
    public GameObject ZonePlayer, Vrags; // GameObject to which units will move
    public GameObject UnitPrefab; // Prefab for the enemy units
    private float spawnInterval = 1500f; // Interval for spawning units
    private bool hasAttacked = false; // Flag to track if the attack log has been printed

    void Start()
    {
        // Start the coroutine to spawn units
        StartCoroutine(SpawnUnits());
    }

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
                if (unit.CompareTag("UnitVrag") && unit.name != "Worker")
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
                        StartCoroutine(CheckForObstacles(agent, ZonePlayer.transform.position));
                    }
                    else
                    {
                        Debug.LogWarning("Unit: " + unit.name + " is not active or not on NavMesh");
                    }
                }
            }
        }
    }

    IEnumerator SpawnUnits()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            Transform vrag1Transform = Vrags.transform.Find("Vrag_1");
            if (vrag1Transform != null)
            {
                Vector3 spawnPosition = vrag1Transform.position + new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
                GameObject newUnit = Instantiate(UnitPrefab, spawnPosition, Quaternion.identity);
                newUnit.transform.SetParent(vrag1Transform);
                newUnit.tag = "UnitVrag";

                NavMeshAgent agent = newUnit.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    agent.SetDestination(ZonePlayer.transform.position);
                }
            }
        }
    }

    IEnumerator CheckForObstacles(NavMeshAgent agent, Vector3 destination)
    {
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        if (agent.pathStatus == NavMeshPathStatus.PathInvalid || agent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            Debug.LogWarning("Unit: " + agent.name + " encountered an obstacle and is stuck");
            // Logic to handle the unit being stuck
            // For example, you can try to find an alternative path or reset the destination
            agent.ResetPath();
            agent.SetDestination(destination);
        }
    }
}