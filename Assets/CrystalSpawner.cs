using UnityEngine;
using System.Collections.Generic;

public class CrystalSpawner : MonoBehaviour
{
    [Header("��������� ������")]
    public GameObject[] crystalPrefabs; // ������� ��������, ��������, ������ ����������
    public float spawnInterval = 5f; // �������� ������ � ��������
    public int maxCrystals = 10; // ������������ ���������� ����������

    [Header("��������� ���������")]
    public Transform spawnPlane; // ����� ��� ������
    public float yOffset = 0.5f; // ������ ��� ����������

    private List<GameObject> spawnedCrystals = new List<GameObject>();
    private float planeWidth;
    private float planeLength;

    private void Start()
    {
        // �������� ������� ���������
        Collider planeCollider = spawnPlane.GetComponent<Collider>();
        planeWidth = planeCollider.bounds.size.x / 2;
        planeLength = planeCollider.bounds.size.z / 2;

        // ��������� �����
        InvokeRepeating(nameof(SpawnCrystal), 0f, spawnInterval);
    }

    private void SpawnCrystal()
    {
        if (spawnedCrystals.Count >= maxCrystals) return;

        // �������� ��������� ������
        int randomIndex = Random.Range(0, crystalPrefabs.Length);
        GameObject crystalPrefab = crystalPrefabs[randomIndex];

        // ���������� �������
        Vector3 spawnPosition = new Vector3(
            Random.Range(-planeWidth, planeWidth),
            yOffset,
            Random.Range(-planeLength, planeLength)
        ) + spawnPlane.position;

        // ������� ��������
        GameObject crystal = Instantiate(crystalPrefab, spawnPosition, Quaternion.identity);
        spawnedCrystals.Add(crystal);
    }

    // �������� ���������� ��� ����������� (��������� �� ContainerController)
    public void RemoveCrystal(GameObject crystal)
    {
        if (spawnedCrystals.Contains(crystal))
        {
            spawnedCrystals.Remove(crystal);
        }
    }
}