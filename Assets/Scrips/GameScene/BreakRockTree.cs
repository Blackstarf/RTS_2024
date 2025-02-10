using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class BreakRockTree : MonoBehaviour
{
    public TMP_Text NumberWood, NumberWoodUnit, NumberRock, NumberRockUnit;
    public GameObject ZonePlayer;
    public GameObject TownHall;
    private GameObject selectedObject; // Ссылка на выделенный объект
    private Transform selectionMine1 = null;
    void Update()
    {
        // Проверяем, нажата ли правая кнопка мыши
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Луч от камеры к курсору
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) // Проверяем, попал ли луч в объект
            {
                GameObject clickedObject = hit.collider.gameObject;
                // Если объект можно выделить
                if (clickedObject.CompareTag("Selectable") || clickedObject.CompareTag("Unit"))
                {
                    // Если выделен один объект, и это рабочий
                    if (GoUnits.selectedUnits.Count == 1 && GoUnits.selectedUnits[0].name == "Worker")
                    {
                        // Получаем рабочего
                        GameObject worker = GoUnits.selectedUnits[0];

                        // Двигаем рабочего к clickedObject
                        NavMeshAgent agent = worker.GetComponent<NavMeshAgent>();
                        if (agent != null)
                        {
                            agent.SetDestination(clickedObject.transform.position);

                            // Ожидаем, пока рабочий дойдёт до объекта
                            StartCoroutine(WaitUntilWorkerArrives(worker, clickedObject, agent));
                        }
                    }
                }
            }
        }
    }

    IEnumerator WaitUntilWorkerArrives(GameObject worker, GameObject targetObject, NavMeshAgent agent)
    {
        // Ожидаем, пока рабочий не приблизится к объекту на заданное расстояние
        while (Vector3.Distance(worker.transform.position, targetObject.transform.position) > 5f)
        {
            yield return null; // Ждём следующий кадр
        }

        // Проверяем, существует ли рабочий и целевой объект
        if (worker == null || targetObject == null)
        {
            yield break; // Выходим из корутины, если один из объектов уничтожен
        }

        // Когда рабочий достиг цели
        agent.isStopped = true; // Останавливаем движение рабочего
        
        // Замораживаем рабочего на 2 секунды
        yield return new WaitForSeconds(2f);
        Transform selectionVillager = worker.transform.Find("Villager");
       
        // Проверяем целевой объект перед использованием его имени
        if (targetObject != null)
        {
            if (targetObject.name == "Forest" || targetObject.name == "Forest(Clone)")
            {
                NumberWood.text = Convert.ToString(int.Parse(NumberWood.text) + 1);
                NumberWoodUnit.text = Convert.ToString(int.Parse(NumberWoodUnit.text) + 1);
                selectionMine1 = selectionVillager.transform.Find("wood");
                GameOver.AddResource("wood", 1);
            }
            else if (targetObject.name == "Rocks" || targetObject.name == "Rocks(Clone)")
            {
                NumberRock.text = Convert.ToString(int.Parse(NumberRock.text) + 1);
                NumberRockUnit.text = Convert.ToString(int.Parse(NumberRockUnit.text) + 1);
                selectionMine1 = selectionVillager.transform.Find("rock");
                GameOver.AddResource("rock", 1);

            }
            else if(targetObject.name == "farm_model(Clone)")
            {
                Transform selectionSprite = ZonePlayer.transform.Find("granary_model(Clone)");
                if (selectionSprite != null)
                {
                    Debug.Log("Мне есть куда сдавать зерно");
                }
                else
                {
                    Debug.Log("Слыш ты сначал построй мельницу");
                }
            }
            selectionMine1.gameObject.SetActive(true);
            // Уничтожаем объект
            if (targetObject.name != "farm_model(Clone)")
            {
                Destroy(targetObject);
            }
        }

        // Разрешаем рабочему двигаться снова
        if (agent != null)
        {
            agent.isStopped = false;
        }
        // Отправляем рабочего к ратуше
        if (TownHall != null)
        {
            StartCoroutine(MoveToTownHall(worker, agent));
        }
        
    }
    IEnumerator MoveToTownHall(GameObject worker, NavMeshAgent agent)
    {
        // Устанавливаем цель для рабочего
        agent.SetDestination(TownHall.transform.position);

        // Ожидаем, пока рабочий не приблизится к ратуше на заданное расстояние
        while (Vector3.Distance(worker.transform.position, TownHall.transform.position) > 5f)
        {
            yield return null; // Ждём следующий кадр
        }

        // Останавливаем движение рабочего
        agent.isStopped = true;

        // Отключаем объект `selectionMine1` только после прибытия
        if (selectionMine1 != null)
        {
            selectionMine1.gameObject.SetActive(false);
        }

        // Возвращаем возможность двигаться рабочему
        agent.isStopped = false;
    }
}