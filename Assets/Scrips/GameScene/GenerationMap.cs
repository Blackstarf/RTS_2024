using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationMap : MonoBehaviour
{
    public GameObject Tree, Rock, Treasure;
    public GameObject plane;
    private GameObject[] models;
    public GameObject townHall; // ������ ������
    public int objectCount = 50; // ���������� ��������
    public Vector2 reservedAreaSize = new Vector2(3, 3);
    void Start()
    {
        models= new GameObject[] { Tree, Rock, Treasure };
        // �������� ������� Plane
        Vector3 planeSize = plane.GetComponent<Renderer>().bounds.size;

        // ��������� ������� ������������� �������
        Vector2 reservedMin = new Vector2(-reservedAreaSize.x / 2, -reservedAreaSize.y / 2);
        Vector2 reservedMax = new Vector2(reservedAreaSize.x / 2, reservedAreaSize.y / 2);

        // ������������� ������ � �����
        Instantiate(townHall, new Vector3(0, 0, 0), Quaternion.identity);

        // ��������� ��������� ������
        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition;
            do
            {
                // ���������� ��������� ���������� �� ���������
                float randomX = Random.Range(-planeSize.x / 2, planeSize.x / 2);
                float randomZ = Random.Range(-planeSize.z / 2, planeSize.z / 2);
                randomPosition = new Vector3(randomX, 0, randomZ);
            }
            // ���������, ����� ������ �� ������� � ����������� �������
            while (randomPosition.x > reservedMin.x && randomPosition.x < reservedMax.x &&
                   randomPosition.z > reservedMin.y && randomPosition.z < reservedMax.y);

            // ��������� ������ �� ������
            GameObject randomModel = models[Random.Range(0, models.Length)];

            // ������� ������ �� ��������������� �����������
            Instantiate(randomModel, randomPosition, Quaternion.identity);
        }
    }
}
