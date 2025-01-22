using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoUnits : MonoBehaviour
{
    private Camera mainCamera;
    private List<GameObject> selectedObjects = new List<GameObject>(); // Список выделенных объектов

    private Vector2 startMousePos; // Начальная точка для выделения рамкой
    private bool isSelecting = false;

    public Texture2D selectionTexture; // Текстура рамки выделения

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        HandleSelectionArea(); // Обработка выделения рамкой
        HandleMovement(); // Перемещение объектов
    }
    void HandleSelectionArea()
    {
        if (Input.GetMouseButtonDown(0)) // Начало выделения рамкой
        {
            isSelecting = true;
            startMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0)) // Завершение выделения рамкой
        {
            isSelecting = false;
            Debug.Log("Вызываем SelectObjectsInArea"); // Проверка вызова
            SelectObjectsInArea(); // Вызов метода
        }
    }
    void SelectObjectsInArea()
    {
        Debug.Log("SelectObjectsInArea вызван."); // Проверяем вызов метода
        Vector2 endMousePos = Input.mousePosition;

        // Если область выделения очень маленькая, выполняем одиночное выделение
        if (Vector2.Distance(startMousePos, endMousePos) < 10f)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;

                // Если объект уже выделен, снимаем выделение
                if (selectedObjects.Contains(clickedObject))
                {
                    selectedObjects.Remove(clickedObject);
                    Debug.Log($"Снято выделение с объекта: {clickedObject.name}");
                }
                else
                {
                    selectedObjects.Add(clickedObject);
                    Debug.Log($"Выделен объект: {clickedObject.name}");
                }
            }
            return;
        }

        // Обработка выделения в области
        Rect selectionRect = new Rect(
            Mathf.Min(startMousePos.x, endMousePos.x),
            Mathf.Min(startMousePos.y, endMousePos.y),
            Mathf.Abs(startMousePos.x - endMousePos.x),
            Mathf.Abs(startMousePos.y - endMousePos.y)
        );

        // Проверяем объекты с компонентом NavMeshAgent
        foreach (NavMeshAgent agent in FindObjectsOfType<NavMeshAgent>())
        {
            Vector3 screenPos = mainCamera.WorldToScreenPoint(agent.transform.position);
            if (selectionRect.Contains(screenPos) && !selectedObjects.Contains(agent.gameObject))
            {
                selectedObjects.Add(agent.gameObject);
                Debug.Log($"Выделен объект в рамке: {agent.gameObject.name}");
            }
        }

        // Лог всех выделенных объектов
        if (selectedObjects.Count > 0)
        {
            Debug.Log("Выделенные объекты: " + string.Join(", ", selectedObjects.ConvertAll(obj => obj.name)));
        }
        else
        {
            Debug.Log("Нет выделенных объектов.");
        }
    }
    // Перемещение выделенных объектов
    void HandleMovement()
    {
        if (Input.GetMouseButtonDown(0) && selectedObjects.Count > 0) // ПКМ
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 targetPoint = hit.point;

                // Устанавливаем точку назначения для всех выделенных объектов
                foreach (GameObject obj in selectedObjects)
                {
                    NavMeshAgent agent = obj.GetComponent<NavMeshAgent>();
                    if (agent != null)
                    {
                        agent.SetDestination(targetPoint);
                    }
                }
            }
        }
    }

    void OnGUI()
    {
        if (isSelecting)
        {
            // Рисуем прямоугольник выделения
            Rect selectionRect = new Rect(
                Mathf.Min(startMousePos.x, Input.mousePosition.x),
                Screen.height - Mathf.Max(startMousePos.y, Input.mousePosition.y),
                Mathf.Abs(startMousePos.x - Input.mousePosition.x),
                Mathf.Abs(startMousePos.y - Input.mousePosition.y)
            );

            // Отображаем текстуру
            GUI.DrawTexture(selectionRect, selectionTexture);
        }
    }
}

//private Camera MainCamera;
//private NavMeshAgent Agent;

//// Start is called before the first frame update
//void Start()
//{
//    MainCamera = Camera.main;
//    Agent = GetComponent<NavMeshAgent>();
//}

//// Update is called once per frame
//void Update()
//{
//    if (Input.GetMouseButtonDown(0))
//    {
//        RaycastHit hit;
//        if (Physics.Raycast(MainCamera.ScreenPointToRay(Input.mousePosition), out hit))
//        {
//            Agent.SetDestination(hit.point);
//        }
//    }

//}