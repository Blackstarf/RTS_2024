using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GenerationMap : MonoBehaviour
{
    public GameObject Tree, Rock, Town_Center;
    public Renderer CylinderRenderer;
    public GameObject plane;
    private GameObject[] models;
    private float density = 0.005f; // ��������� �������� (��������, 0.01 = 1 ������ �� 100 ������ �������)
    private int objectCount; // ���������� ��������, �������������� �������������
    private Vector2 reservedAreaSize = new Vector2(40, 40); // ������ ����������������� �������
    private float perlinScale = 10f; // ������� ���� �������
    private float perlinThreshold = 0.5f; // ����� �������� ���� ��� ���������� �������
    private float minDistance = 5f; // ����������� ���������� ����� ���������
    public NavMeshSurface navMeshSurface;

    private List<Vector3> placedObjectPositions = new List<Vector3>(); // ������ ������� ����������� ��������

    // ����� ���� ��� ���������� Town_Center
    private int townCenterCount; // ���������� �����
    public float townCenterRadiusMultiplier = 0.4f;

    void Start()
    {
        // �������� ��������� NavMeshSurface
        navMeshSurface = GetComponent<NavMeshSurface>();
        models = new GameObject[] { Tree, Rock };
        townCenterCount = PlayerPrefs.GetInt("CountVrags");
        // �������� ��������� NavMeshSurface
        navMeshSurface = GetComponent<NavMeshSurface>();
        models = new GameObject[] { Tree, Rock };

        // �������� ������� Plane
        Vector3 planeSize = plane.GetComponent<Renderer>().bounds.size;

        // ������������ ������ ��� ���������� Town_Center
        float radius = Mathf.Min(planeSize.x, planeSize.z) * townCenterRadiusMultiplier;

        // ��������� ������ �� ����������
        for (int i = 0; i < townCenterCount; i++)
        {
            float angle = i * (360f / townCenterCount); // ���������� ����� ��������� �� ����������
            float angleRad = angle * Mathf.Deg2Rad; // ����������� ���� � �������

            float x = Mathf.Cos(angleRad) * radius;
            float z = Mathf.Sin(angleRad) * radius;

            Vector3 position = new Vector3(x, 1.7f, z); // ������� ������

            // Instantiate Town_Center �� ������������ �������
            Instantiate(Town_Center, position, Quaternion.identity);
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
        // �������� ������� Plane
        planeSize = plane.GetComponent<Renderer>().bounds.size;

        // ������������ ���������� �������� �� ������ ���������
        float planeArea = planeSize.x * planeSize.z; // ������� ���������
        objectCount = Mathf.RoundToInt(planeArea * density); // ���������� �������� ��������������� ���������

        // ��������� ������� ������������� �������
        Vector2 reservedMin = new Vector2(-reservedAreaSize.x / 2, -reservedAreaSize.y / 2);
        Vector2 reservedMax = new Vector2(reservedAreaSize.x / 2, reservedAreaSize.y / 2);

        // ��������� �������
        int placedObjects = 0;
        while (placedObjects < objectCount)
        {
            // ���������� ��������� ����������
            float randomX = UnityEngine.Random.Range(-planeSize.x / 2, planeSize.x / 2);
            float randomZ = UnityEngine.Random.Range(-planeSize.z / 2, planeSize.z / 2);

            // ���������, ����� ������ �� ������� � ����������� �������
            if (randomX > reservedMin.x && randomX < reservedMax.x &&
                randomZ > reservedMin.y && randomZ < reservedMax.y)
            {
                continue; // ����������, ���� � ����������������� �������
            }

            // ���������� �������� ���� ������� ��� ���������
            float perlinValue = Mathf.PerlinNoise(
                (randomX + planeSize.x / 2) / perlinScale,
                (randomZ + planeSize.z / 2) / perlinScale);

            // ��������� ����� �������� ����
            if (perlinValue >= perlinThreshold)
            {
                Vector3 randomPosition = new Vector3(randomX, 1, randomZ);

                // ���������, ��� ����� ����� ���������� ������ �� ������ ��������
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
                    continue; // ���������� �������� �������, ���� �� ������� ������ � �������
                }

                // ��������� ������� Plane
                if (Mathf.Abs(randomPosition.x + 10) > planeSize.x / 2 || Mathf.Abs(randomPosition.z - 10) > planeSize.z / 2)
                {
                    continue; // ���������� �������� �������, ���� �� ������� �� ������� Plane
                }

                // ��������� ������ �� ������
                GameObject randomModel = models[UnityEngine.Random.Range(0, models.Length)];
                if (randomModel == Tree)
                {
                    randomPosition.y = 2; // ������������� ������ ��� ��������
                }

                // ������� ������ �� ��������������� �����������
                Instantiate(randomModel, randomPosition, Quaternion.identity);

                // ��������� ������� � ������ ����������� ��������
                placedObjectPositions.Add(randomPosition);

                placedObjects++;
            }
        }
    }
}
