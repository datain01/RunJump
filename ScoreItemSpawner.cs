using UnityEngine;
using System.Collections;

public class ScoreItemSpawner : MonoBehaviour
{
    public GameObject scoreItemPrefab;  // 스코어 아이템 프리팹
    public float minSpawnRate = 5f;
    public float maxSpawnRate = 10f;
    public float spawnY = -2.7f; // 바닥 위에서 생성

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(SpawnScoreItems());
    }

    IEnumerator SpawnScoreItems()
    {
        while (true)
        {
            float randomSpawnTime = Random.Range(minSpawnRate, maxSpawnRate);
            yield return new WaitForSeconds(randomSpawnTime);

            float spawnX = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + 1f;

            Instantiate(scoreItemPrefab, new Vector3(spawnX, spawnY, 0), Quaternion.identity);
        }
    }
}
