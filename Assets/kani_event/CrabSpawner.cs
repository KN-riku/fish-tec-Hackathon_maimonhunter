using UnityEngine;
using System.Collections;

public class CrabSpawner : MonoBehaviour
{
    [Header("Crab Prefabs (6 types)")]
    public GameObject crabPrefab1;
    public GameObject crabPrefab2;
    public GameObject crabPrefab3;
    public GameObject crabPrefab4;
    public GameObject crabPrefab5;
    public GameObject crabPrefab6;

    [Header("Spawn Weights")]
    public int weight1 = 30;
    public int weight2 = 25;
    public int weight3 = 20;
    public int weight4 = 10;
    public int weight5 = 8;
    public int weight6 = 7;

    // --- 追加：得点の設定 ---
    [Header("Points By Type")]
    public int score1 = 10;
    public int score2 = 20;
    public int score3 = 30;
    public int score4 = 100; // Kagayaki (Rare)
    public int score5 = 200; // IchibanOshi (Rare)
    public int score6 = 500; // Kaguyahime (Rare)

    [Header("Life Time By Type")]
    public float lifeTime1 = 12f;
    public float lifeTime2 = 11f;
    public float lifeTime3 = 10f;
    public float lifeTime4 = 8f;
    public float lifeTime5 = 6f;
    public float lifeTime6 = 5f;

    [Header("Net")]
    public Transform net;

    [Header("Crab Count")]
    public int initialCrabCount = 6;
    public int maxCrabCount = 10;

    [Header("Respawn Time")]
    public float minRespawnTime = 3f;
    public float maxRespawnTime = 10f;

    [Header("Spawn Area (World)")]
    public float minX = -2.5f;
    public float maxX = 2.5f;
    public float minY = -4.0f;
    public float maxY = -2.0f;

    private int currentCrabCount = 0;

    void Start()
    {
        if (net == null)
        {
            GameObject obj = GameObject.Find("Net");
            if (obj == null) obj = GameObject.Find("NET");
            if (obj != null) net = obj.transform;
        }

        for (int i = 0; i < initialCrabCount; i++)
        {
            SpawnCrab();
        }
    }

    public void OnCrabRemoved(Crab crab)
    {
        currentCrabCount--;
        if (currentCrabCount < 0) currentCrabCount = 0;
        StartCoroutine(RespawnCrabAfterDelay());
    }

    IEnumerator RespawnCrabAfterDelay()
    {
        float waitTime = Random.Range(minRespawnTime, maxRespawnTime);
        yield return new WaitForSeconds(waitTime);

        if (currentCrabCount < maxCrabCount)
        {
            SpawnCrab();
        }
    }

    public void SpawnCrab()
    {
        if (currentCrabCount >= maxCrabCount) return;

        int crabIndex = GetRandomCrabIndex();
        GameObject prefab = GetCrabPrefabByIndex(crabIndex);

        if (prefab == null) return;

        Vector2 pos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        GameObject obj = Instantiate(prefab, pos, Quaternion.identity);

        Crab crab = obj.GetComponent<Crab>();
        if (crab != null)
        {
            crab.spawner = this;
            crab.net = net;
            crab.lifeTime = GetLifeTimeByIndex(crabIndex);
            // --- 追加：生成時に得点を渡す ---
            crab.scorePoints = GetScoreByIndex(crabIndex);
            currentCrabCount++;
        }
    }

    int GetRandomCrabIndex()
    {
        int w1 = Mathf.Max(0, weight1);
        int w2 = Mathf.Max(0, weight2);
        int w3 = Mathf.Max(0, weight3);
        int w4 = Mathf.Max(0, weight4);
        int w5 = Mathf.Max(0, weight5);
        int w6 = Mathf.Max(0, weight6);

        int totalWeight = w1 + w2 + w3 + w4 + w5 + w6;
        if (totalWeight <= 0) return 1;

        int rand = Random.Range(0, totalWeight);
        if (rand < w1) return 1; rand -= w1;
        if (rand < w2) return 2; rand -= w2;
        if (rand < w3) return 3; rand -= w3;
        if (rand < w4) return 4; rand -= w4;
        if (rand < w5) return 5;
        return 6;
    }

    GameObject GetCrabPrefabByIndex(int index)
    {
        switch (index)
        {
            case 1: return crabPrefab1;
            case 2: return crabPrefab2;
            case 3: return crabPrefab3;
            case 4: return crabPrefab4;
            case 5: return crabPrefab5;
            case 6: return crabPrefab6;
            default: return crabPrefab1;
        }
    }

    float GetLifeTimeByIndex(int index)
    {
        switch (index)
        {
            case 1: return lifeTime1;
            case 2: return lifeTime2;
            case 3: return lifeTime3;
            case 4: return lifeTime4;
            case 5: return lifeTime5;
            case 6: return lifeTime6;
            default: return 10f;
        }
    }

    // --- 追加：インデックスから得点を取得するメソッド ---
    int GetScoreByIndex(int index)
    {
        switch (index)
        {
            case 1: return score1;
            case 2: return score2;
            case 3: return score3;
            case 4: return score4;
            case 5: return score5;
            case 6: return score6;
            default: return 10;
        }
    }
}