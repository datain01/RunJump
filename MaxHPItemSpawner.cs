using UnityEngine;
using System.Collections;

public class MaxHPItemSpawner : MonoBehaviour
{
    public GameObject maxHpItemPrefab; // HP 증가 아이템 프리팹
    public float minSpawnRate = 15f;
    public float maxSpawnRate = 20f;
    public float spawnY = -2.7f;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(SpawnMaxHPItems());
    }

    IEnumerator SpawnMaxHPItems()
    {
        while (true)
        {
            if (GameManager.instance.speed >= 4f) // ✅ 속도 4 이상일 때부터 스폰
            {
                float randomSpawnTime = Random.Range(minSpawnRate, maxSpawnRate);
                yield return new WaitForSeconds(randomSpawnTime);

                float spawnX = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + 1f;
                Instantiate(maxHpItemPrefab, new Vector3(spawnX, spawnY, 0), Quaternion.identity);
            }
            else
            {
                yield return new WaitForSeconds(5f); // 속도 4 미만일 때는 대기
            }
        }
    }
}
