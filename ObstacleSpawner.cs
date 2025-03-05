using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;  // 장애물 프리팹
    public float minSpawnRate = 0.2f;  // 최소 스폰 간격
    public float maxSpawnRate = 3f;    // 최대 스폰 간격
    public float spawnY = -3f;         // 장애물 고정 위치 (바닥 위)

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(SpawnObstacles());
    }

    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            float randomSpawnTime = Random.Range(minSpawnRate, maxSpawnRate); // 0.2초 ~ 3초 사이 랜덤
            yield return new WaitForSeconds(randomSpawnTime); 

            float spawnX = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + 1f; // 화면 오른쪽 끝 + 여유 공간

            Instantiate(obstaclePrefab, new Vector3(spawnX, spawnY, 0), Quaternion.identity);
        }
    }
}
