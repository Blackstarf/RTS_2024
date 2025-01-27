using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class BreakRockTree : MonoBehaviour
{
    public TMP_Text NumberWood, NumberWoodUnit, NumberRock, NumberRockUnit;
    private GameObject selectedObject; // Ссылка на выделенный объект

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
                if (clickedObject.CompareTag("Selectable"))
                {
                    SelectObject(clickedObject);

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

        // Проверяем целевой объект перед использованием его имени
        if (targetObject != null)
        {
            if (targetObject.name == "Forest" || targetObject.name == "Forest(Clone)")
            {
                NumberWood.text = Convert.ToString(int.Parse(NumberWood.text) + 1);
                NumberWoodUnit.text = Convert.ToString(int.Parse(NumberWoodUnit.text) + 1);
            }
            else if (targetObject.name == "Rocks" || targetObject.name == "Rocks(Clone)")
            {
                NumberRock.text = Convert.ToString(int.Parse(NumberRock.text) + 1);
                NumberRockUnit.text = Convert.ToString(int.Parse(NumberRockUnit.text) + 1);
            }

            // Уничтожаем объект
            Destroy(targetObject);
        }

        // Разрешаем рабочему двигаться снова
        if (agent != null)
        {
            agent.isStopped = false;
        }
    }

    IEnumerator FreezeUnitForSeconds(GameObject unit, float seconds)
    {
        // Отключаем способность юнита двигаться
        NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = true; // Останавливаем движение
        }

        // Ждём указанное количество секунд
        yield return new WaitForSeconds(seconds);

        // Возвращаем возможность двигаться
        if (agent != null)
        {
            agent.isStopped = false; // Разрешаем движение
        }
    }
    void SelectObject(GameObject obj)
    {
        // Выделяем новый объект
        selectedObject = obj;

        Debug.Log("Selected: " + obj.name);
    }
}