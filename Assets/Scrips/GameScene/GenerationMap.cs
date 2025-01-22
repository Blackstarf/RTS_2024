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
    public GameObject townHall; // ������ ������
    public int objectCount = 100; // ���������� ��������
    private Vector2 reservedAreaSize = new Vector2(3, 3);
    public NavMeshSurface navMeshSurface;
    void Start()
    {
        // �������� ��������� NavMeshSurface
        navMeshSurface = GetComponent<NavMeshSurface>();
        models = new GameObject[] { Tree, Rock};
        // �������� ������� Plane
        Vector3 planeSize = plane.GetComponent<Renderer>().bounds.size;

        // ��������� ������� ������������� �������
        Vector2 reservedMin = new Vector2(-reservedAreaSize.x / 2, -reservedAreaSize.y / 2);
        Vector2 reservedMax = new Vector2(reservedAreaSize.x / 2, reservedAreaSize.y / 2);

        //// ������������� ������ � �����
        //Instantiate(townHall, new Vector3(0, 1, 0), Quaternion.identity);

        // ��������� ��������� ������
        for (int i = 0; i < objectCount; i++)
        {
            float randomX;
            float randomZ;
            Vector3 randomPosition;
            do
            {
                // ���������� ��������� ���������� �� ���������
                 randomX = UnityEngine.Random.Range(-planeSize.x / 2, planeSize.x / 2);
                 randomZ = UnityEngine.Random.Range(-planeSize.z / 2, planeSize.z / 2);
                randomPosition = new Vector3(randomX, 2, randomZ);
            }
            // ���������, ����� ������ �� ������� � ����������� �������
            while (randomPosition.x > reservedMin.x && randomPosition.x < reservedMax.x &&
                   randomPosition.z > reservedMin.y && randomPosition.z < reservedMax.y);

            // ��������� ������ �� ������
            GameObject randomModel = models[UnityEngine.Random.Range(0, models.Length)];
            if(randomModel == Tree)
            {
                randomPosition = new Vector3(randomX, 2, randomZ);
            }else randomPosition = new Vector3(randomX, 1, randomZ);
            // ������� ������ �� ��������������� �����������
            Instantiate(randomModel, randomPosition, Quaternion.identity);
        }
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
        }
    }
}
