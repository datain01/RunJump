using UnityEngine;
using System.Collections;

public class HealthItemSpawner : MonoBehaviour
{
    public GameObject healthItemPrefab;  // HP 회복 아이템 프리팹
    public float minSpawnRate = 5f;  // 최소 스폰 간격
    public float maxSpawnRate = 8f;  // 최대 스폰 간격
    public float spawnY = -2.7f; // 바닥 위에서 생성

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(SpawnHealthItems());
    }

    IEnumerator SpawnHealthItems()
    {
        while (true)
        {
            float randomSpawnTime = Random.Range(minSpawnRate, maxSpawnRate); // 5~8초 랜덤
            yield return new WaitForSeconds(randomSpawnTime); 

            float spawnX = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + 1f; // 오른쪽 바깥에서 등장

            Instantiate(healthItemPrefab, new Vector3(spawnX, spawnY, 0), Quaternion.identity);
        }
    }
}
