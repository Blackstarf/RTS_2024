using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GenerationMap : MonoBehaviour
{
    public GameObject Tree, Rock;
    public GameObject plane;
    private GameObject[] models;
    public float density = 0.005f; // Плотность объектов (например, 0.01 = 1 объект на 100 единиц площади)
    private int objectCount; // Количество объектов, рассчитывается автоматически
    private Vector2 reservedAreaSize = new Vector2(20, 20); // Размер зарезервированной области
    public float perlinScale = 10f; // Масштаб шума Перлина
    public float perlinThreshold = 0.5f; // Порог значения шума для размещения объекта
    public float minDistance = 5f; // Минимальное расстояние между объектами
    public NavMeshSurface navMeshSurface;

    private List<Vector3> placedObjectPositions = new List<Vector3>(); // Список позиций размещённых объектов

    void Start()
    {
        // Получаем компонент NavMeshSurface
        navMeshSurface = GetComponent<NavMeshSurface>();
        models = new GameObject[] { Tree, Rock };

        // Получаем размеры Plane
        Vector3 planeSize = plane.GetComponent<Renderer>().bounds.size;

        // Рассчитываем количество объектов на основе плотности
        float planeArea = planeSize.x * planeSize.z; // Площадь плоскости
        objectCount = Mathf.RoundToInt(planeArea * density); // Количество объектов пропорционально плотности

        // Вычисляем границы резервируемой области
        Vector2 reservedMin = new Vector2(-reservedAreaSize.x / 2, -reservedAreaSize.y / 2);
        Vector2 reservedMax = new Vector2(reservedAreaSize.x / 2, reservedAreaSize.y / 2);

        // Размещаем объекты
        int placedObjects = 0;
        while (placedObjects < objectCount)
        {
            // Генерируем случайные координаты
            float randomX = UnityEngine.Random.Range(-planeSize.x / 2, planeSize.x / 2);
            float randomZ = UnityEngine.Random.Range(-planeSize.z / 2, planeSize.z / 2);

            // Проверяем, чтобы объект не попадал в центральную область
            if (randomX > reservedMin.x && randomX < reservedMax.x &&
                randomZ > reservedMin.y && randomZ < reservedMax.y)
            {
                continue; // Пропускаем, если в зарезервированной области
            }

            // Генерируем значение шума Перлина для координат
            float perlinValue = Mathf.PerlinNoise(
                (randomX + planeSize.x / 2) / perlinScale,
                (randomZ + planeSize.z / 2) / perlinScale);

            // Проверяем порог значения шума
            if (perlinValue >= perlinThreshold)
            {
                Vector3 randomPosition = new Vector3(randomX, 1, randomZ);

                // Проверяем, что новое место достаточно далеко от других объектов
                bool isTooClose = false;
                foreach (Vector3 placedPosition in placedObjectPositions)
                {
                    if (Vector3.Distance(randomPosition, placedPosition) < minDistance)
                    {
                        isTooClose = true;
                        break;
                    }
                }

                if (isTooClose)
                {
                    continue; // Пропускаем создание объекта, если он слишком близко к другому
                }

                // Проверяем границы Plane
                if (Mathf.Abs(randomPosition.x + 10) > planeSize.x / 2 || Mathf.Abs(randomPosition.z - 10) > planeSize.z / 2)
                {
                    continue; // Пропускаем создание объекта, если он выходит за границы Plane
                }

                // Случайная модель из списка
                GameObject randomModel = models[UnityEngine.Random.Range(0, models.Length)];
                if (randomModel == Tree)
                {
                    randomPosition.y = 2; // Устанавливаем высоту для деревьев
                }

                // Создаем модель на сгенерированных координатах
                Instantiate(randomModel, randomPosition, Quaternion.identity);

                // Добавляем позицию в список размещённых объектов
                placedObjectPositions.Add(randomPosition);

                placedObjects++;
            }
        }

        // Строим NavMesh
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
        }
    }
}
