using UnityEngine;

public class ObstracleSpwner : MonoBehaviour
{
    public GameObject obstaclPrefab;
    public int spawnCount = 12;

    private bool hasSpawned = false;

    void Update()
    {
        // 게임 시작 전이면 대기
        if (GameManager.instance == null) return;
        if (!GameManager.instance.IsGameStarted()) return;
        if (hasSpawned) return;

        // 게임 시작되면 장애물 생성!
        hasSpawned = true;
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnObstacle();
        }
    }

    void SpawnObstacle()
    {
        Vector2 randomPos = Random.insideUnitCircle.normalized * Random.Range(2f, 5f);
        GameObject obs = Instantiate(obstaclPrefab, randomPos, Quaternion.identity);

        Rigidbody2D rb = obs.GetComponent<Rigidbody2D>();
        Vector2 outwardDir = randomPos.normalized;
        float speed = Random.Range(3f, 6f);
        rb.AddForce(outwardDir * speed, ForceMode2D.Impulse);
    }
}