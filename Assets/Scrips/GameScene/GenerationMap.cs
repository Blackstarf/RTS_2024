using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine;

public class GenerationMap : MonoBehaviour
{
    public GameObject Tree, Rock, Town_Center, ZoneVrag;
    public Renderer CylinderRenderer;
    public GameObject plane;
    public GameObject ZonesVrags;

    private GameObject[] models;

    private float density = 0.005f; // Плотность объектов (например, 0.01 = 1 объект на 100 единиц площади)
    private int objectCount; // Количество объектов, рассчитывается автоматически
    private Vector2 reservedAreaSize = new Vector2(25, 25); // Размер зарезервированной области
    private float perlinScale = 10f; // Масштаб шума Перлина
    private float perlinThreshold = 0.5f; // Порог значения шума для размещения объекта
    private float minDistance = 5f; // Минимальное расстояние между объектами
    private List<Vector3> placedObjectPositions = new List<Vector3>(); // Список позиций размещённых объектов

    // Новые поля для размещения Town_Center
    private int townCenterCount; // Количество ратуш
    private float townCenterRadiusMultiplier = 0.4f;
    private List<Vector3> townCenterPositions = new List<Vector3>(); // Список позиций Town_Center

    void Start()
    {
        models = new GameObject[] { Tree, Rock };
        townCenterCount = PlayerPrefs.GetInt("CountVrags");

        // Получаем размеры Plane
        Vector3 planeSize = plane.GetComponent<Renderer>().bounds.size;

        // Рассчитываем радиус для размещения Town_Center
        float radius = Mathf.Min(planeSize.x, planeSize.z) * townCenterRadiusMultiplier;

        // Размещаем ратуши по окружности
        for (int i = 0; i < townCenterCount; i++)
        {
            float angle = i * (360f / townCenterCount); // Расстояние между объектами на окружности
            float angleRad = angle * Mathf.Deg2Rad; // Преобразуем угол в радианы

            float x = Mathf.Cos(angleRad) * radius;
            float z = Mathf.Sin(angleRad) * radius;

            Vector3 position = new Vector3(x, 1.7f, z); // Позиция ратуши

            // Instantiate Town_Center на рассчитанной позиции
            GameObject townCenterInstance = Instantiate(Town_Center, position, Quaternion.identity);
            townCenterInstance.transform.SetParent(ZonesVrags.transform);

            // Назначаем имя Town_Center
            townCenterInstance.name = "Vrag_" + (i + 1);

            // Находим TMP_Text компонент с именем Player
            TMP_Text playerText = townCenterInstance.GetComponentInChildren<TMP_Text>();
            if (playerText != null)
            {
                // Обновляем текст
                playerText.text = townCenterInstance.name;
            }

            // Добавляем позицию Town_Center в список
            townCenterPositions.Add(position);
        }

        Color color = Color.white;
        string colorPlayer = PlayerPrefs.GetString("ColorPlayer");
        switch (colorPlayer)
        {
            case "Green":
                color = Color.green;
                break;
            case "Blue":
                color = Color.blue;
                break;
            case "Red":
                color = Color.red;
                break;
            case "Orange":
                Color customOrange = new Color(1f, 0.5f, 0f);
                color = customOrange;
                break;
            case "Purple":
                color = Color.magenta;
                break;
            case "Yellow":
                color = Color.yellow;
                break;
        }
        CylinderRenderer.material.color = color;

        // Рассчитываем количество объектов на основе плотности
        float planeArea = planeSize.x * planeSize.z; // Площадь плоскости
        objectCount = Mathf.RoundToInt(planeArea * density); // Количество объектов пропорционально плотности

        // Вычисляем границы резервируемой области в центре
        Vector2 reservedMin = new Vector2(-reservedAreaSize.x / 2, -reservedAreaSize.y / 2);
        Vector2 reservedMax = new Vector2(reservedAreaSize.x / 2, reservedAreaSize.y / 2);

        // Размещаем объекты
        int placedObjects = 0;
        while (placedObjects < objectCount)
        {
            // Генерируем случайные координаты
            float randomX = UnityEngine.Random.Range(-planeSize.x / 2, planeSize.x / 2);
            float randomZ = UnityEngine.Random.Range(-planeSize.z / 2, planeSize.z / 2);

            Vector3 randomPosition = new Vector3(randomX, 1, randomZ);

            // Проверяем, чтобы объект не попадал в зарезервированные области вокруг Town_Center
            bool isInReservedArea = false;
            foreach (Vector3 townCenterPosition in townCenterPositions)
            {
                if (Vector3.Distance(randomPosition, townCenterPosition) < reservedAreaSize.x / 2)
                {
                    isInReservedArea = true;
                    break;
                }
            }

            // Проверяем, чтобы объект не попадал в центральную зарезервированную область
            if (randomX > reservedMin.x && randomX < reservedMax.x && randomZ > reservedMin.y && randomZ < reservedMax.y)
            {
                isInReservedArea = true;
            }

            if (isInReservedArea)
            {
                continue; // Пропускаем, если в зарезервированной области
            }

            // Генерируем значение шума Перлина для координат
            float perlinValue = Mathf.PerlinNoise( (randomX + planeSize.x / 2) / perlinScale,  (randomZ + planeSize.z / 2) / perlinScale);

            // Проверяем порог значения шума
            if (perlinValue >= perlinThreshold)
            {
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
        ZoneVrag.SetActive(false);
    }
}