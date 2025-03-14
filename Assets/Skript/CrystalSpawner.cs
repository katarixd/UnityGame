using UnityEngine;
using System.Collections.Generic;

public class CrystalSpawner : MonoBehaviour
{
    [Header("Настройки спавна")]
    public GameObject[] crystalPrefabs; // Префабы красного, зеленого, синего кристаллов
    public float spawnInterval = 5f; // Интервал спавна в секундах
    public int maxCrystals = 10; // Максимальное количество кристаллов

    [Header("Параметры плоскости")]
    public Transform spawnPlane; // Плейн для спавна
    public float yOffset = 0.5f; // Высота над плоскостью

    private List<GameObject> spawnedCrystals = new List<GameObject>();
    private float planeWidth;
    private float planeLength;

    private void Start()
    {
        // Получаем размеры плоскости
        Collider planeCollider = spawnPlane.GetComponent<Collider>();
        planeWidth = planeCollider.bounds.size.x / 2;
        planeLength = planeCollider.bounds.size.z / 2;

        // Запускаем спавн
        InvokeRepeating(nameof(SpawnCrystal), 0f, spawnInterval);
    }

    private void SpawnCrystal()
    {
        if (spawnedCrystals.Count >= maxCrystals) return;

        // Выбираем случайный префаб
        int randomIndex = Random.Range(0, crystalPrefabs.Length);
        GameObject crystalPrefab = crystalPrefabs[randomIndex];

        // Генерируем позицию
        Vector3 spawnPosition = new Vector3(
            Random.Range(-planeWidth, planeWidth),
            yOffset,
            Random.Range(-planeLength, planeLength)
        ) + spawnPlane.position;

        // Создаем кристалл
        GameObject crystal = Instantiate(crystalPrefab, spawnPosition, Quaternion.identity);
        spawnedCrystals.Add(crystal);
    }

    // Удаление кристаллов при уничтожении (вызывайте из ContainerController)
    public void RemoveCrystal(GameObject crystal)
    {
        if (spawnedCrystals.Contains(crystal))
        {
            spawnedCrystals.Remove(crystal);
        }
    }
}