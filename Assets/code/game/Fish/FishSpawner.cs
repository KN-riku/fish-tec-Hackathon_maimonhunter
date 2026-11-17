using UnityEngine;
using System.Collections; // Coroutineを使うために必要
using System.Collections.Generic;

public class FishSpawner : MonoBehaviour
{
    public List<GameObject> allFishPrefabs;
    public SeasonManager seasonManager;

    [Header("生成の設定")]
    public float minSpawnDelay = 0.2f; // 最短の間隔
    public float maxSpawnDelay = 1.0f; // 最長の間隔
    public int minBurstCount = 1;      // 一度に最低何匹出すか
    public int maxBurstCount = 3;      // 一度に最大何匹出すか

    [Header("出現位置（右端の上下範囲）")]
    public float spawnX = 10f;         // 画面右外のX座標
    public float minY = -4.0f;         // 出現する高さの下限
    public float maxY = 2.0f;          // 出現する高さの上限

    void Start()
    {
        // コルーチンを開始してループさせる
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // 1. 現在の季節に合う魚を抽出
            List<GameObject> seasonalFish = GetSeasonalFish();

            if (seasonalFish.Count > 0)
            {
                // 2. 一回に生成する数をランダムに決定（これで「多く」見える）
                int burst = Random.Range(minBurstCount, maxBurstCount + 1);

                for (int i = 0; i < burst; i++)
                {
                    GameObject selected = seasonalFish[Random.Range(0, seasonalFish.Count)];

                    // 3. 高さをバラバラにする
                    float randomY = Random.Range(minY, maxY);
                    Vector3 pos = new Vector3(spawnX, randomY, 0);

                    Instantiate(selected, pos, Quaternion.identity);
                }
            }

            // 4. 次の生成までの待ち時間をランダムにして「不規則さ」を出す
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
        }
    }

    // 季節判定ロジックを分離（コードをスッキリさせるため）
    List<GameObject> GetSeasonalFish()
    {
        List<GameObject> validFish = new List<GameObject>();
        foreach (GameObject fishObj in allFishPrefabs)
        {
            Fish fish = fishObj.GetComponent<Fish>();
            if (System.Array.Exists(fish.availableSeasons, s => s == seasonManager.currentSeason))
            {
                validFish.Add(fishObj);
            }
        }
        return validFish;
    }
}