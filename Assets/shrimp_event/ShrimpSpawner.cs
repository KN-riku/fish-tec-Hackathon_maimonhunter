using UnityEngine;

/// <summary>
/// 3種類のエビプレハブを常に5匹以上スポーンさせるスポーナー
/// 空のGameObjectにアタッチして使用してください
/// </summary>
public class ShrimpSpawner : MonoBehaviour
{
    [Header("Shrimp Prefabs (3種類をセット)")]
    [SerializeField] private GameObject[] shrimpPrefabs;   // 3種類のエビプレハブ

    [Header("Spawn Settings")]
    [SerializeField] private int minShrimpCount = 5;       // 常に維持する最小匹数
    [SerializeField] private int maxShrimpCount = 8;       // 最大匹数
    [SerializeField] private float spawnInterval = 2.0f;   // 不足時のスポーン間隔(秒)

    private float spawnTimer;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        // 起動時に最小匹数まで一気にスポーン
        int initialCount = Random.Range(minShrimpCount, maxShrimpCount + 1);
        for (int i = 0; i < initialCount; i++)
        {
            SpawnShrimp();
        }
    }

    void Update()
    {
        // 現在のエビの数を確認（タグで管理）
        int current = GameObject.FindGameObjectsWithTag("Shrimp").Length;

        if (current < minShrimpCount)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                spawnTimer = 0f;
                SpawnShrimp();
            }
        }
        else
        {
            spawnTimer = 0f;
        }
    }

    private void SpawnShrimp()
    {
        if (shrimpPrefabs == null || shrimpPrefabs.Length == 0) return;
        if (mainCamera == null) return;

        // ランダムな種類を選択
        int index = Random.Range(0, shrimpPrefabs.Length);
        GameObject prefab = shrimpPrefabs[index];
        if (prefab == null) return;

        // カメラ範囲内のランダムな位置にスポーン
        Vector3 spawnPos = GetRandomSpawnPosition();
        GameObject shrimp = Instantiate(prefab, spawnPos, Quaternion.identity);
        shrimp.tag = "Shrimp";
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float halfH = mainCamera.orthographicSize;
        float halfW = halfH * mainCamera.aspect;
        Vector3 cp = mainCamera.transform.position;

        float x = Random.Range(cp.x - halfW + 0.5f, cp.x + halfW - 0.5f);
        float y = Random.Range(cp.y - halfH + 0.5f, cp.y + halfH - 0.5f);

        return new Vector3(x, y, 0f);
    }
}